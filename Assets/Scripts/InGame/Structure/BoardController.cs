using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]
public class BoardController : MonoBehaviour
{
    public static BoardController board;

    public Transform playerParent;
    public Transform endPlayerParent;

    public Transform dareUI;
    public Transform playerUI;
    public Transform settingsButton;

    public static BoardData boardData;

    public GameObject[] nodePrefabs;
    public GameObject playerPrefab;

    [HideInInspector] public List<int> queue;

    public string filepath;

    [HideInInspector] public BoardState state = BoardState.PlayerChoose;

    [HideInInspector] public Transform currentPlayer;

    [HideInInspector] public int queueNum = 0;

    [HideInInspector] private int dicenum;

    private int tonode = -1;

    private int finishedCount = 0;

    public List<Color> playerColors;

    public enum BoardState
    {
        PlayerChoose = 0,
        DiceRoll = 1,
        PlayerMove = 2,
        ChoosePath = 3,
        Card = 4,
        NextPlayerAnimate = 5,
        End = 6
    }

    void Start()
    {
        Generate();
        SetState(BoardState.PlayerChoose);
    }

    private void Update()
    {
        Process();
    }

    public void Generate()
    {
        board = this;

        if (boardData == null)
        {
            boardData = JsonUtility.FromJson<BoardData>(File.ReadAllText(Application.streamingAssetsPath + "/DefaultBoards/OG.json"));
        }

        foreach(NodeData node in boardData.nodes)
        {
            GameObject nodeObject;

            switch (node.type)
            {
                case "START":
                    nodeObject = Instantiate(nodePrefabs[0]);
                    Camera.main.GetComponent<CameraController>().target = nodeObject.transform.GetChild(3);
                    Camera.main.GetComponent<CameraController>().overHead = nodeObject.transform.GetChild(3);
                    break;
                case "DARE":
                    nodeObject = Instantiate(nodePrefabs[1]);
                    break;
                case "END":
                    nodeObject = Instantiate(nodePrefabs[2]);
                    break;
                default:
                    nodeObject = Instantiate(nodePrefabs[1]);
                    break;
            }
            nodeObject.transform.parent = transform;
            nodeObject.GetComponent<NodeController>().data = node;
            nodeObject.GetComponent<NodeController>().Init();
        }

        foreach(NodeController node in transform.GetComponentsInChildren<NodeController>())
        {
            node.CreatePaths();
        }

        
    }

    public Transform GetNode(int nodenum)
    {
        return transform.GetChild(nodenum);
    }

    public NodeData GetNodeData(int nodenum)
    {
        return GetNode(nodenum).GetComponent<NodeController>().data;
    }

    public void StartGame()
    {
        foreach (Transform player in PlayerMenuController.playerList) if (player.GetChild(2).GetChild(0).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text.Length < 3) return;

        int i = 0;
        queue = new List<int>();
        foreach (Transform player in PlayerMenuController.playerList)
        {
            PlayerData pd = new PlayerData();
            pd.name = player.GetChild(2).GetChild(0).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text;

            pd.color = playerColors[i];
            pd.score = 0;
            pd.place = boardData.properties.startNodeID;
            pd.id = i;
            pd.finished = false;

            i++;

            queue.Add(pd.id);

            GameObject newPlayer = Instantiate(playerPrefab) as GameObject;
            newPlayer.transform.SetParent(playerParent);

            newPlayer.transform.GetComponent<PlayerController>().Init(pd);
        }

        Camera.main.GetComponent<CameraController>().target = GetPlayer(0).GetChild(0);

        PlayerMenuController.playerMenuController.transform.gameObject.SetActive(false);

        currentPlayer = playerParent.GetChild(queueNum);

        playerUI.gameObject.SetActive(true);

        settingsButton.gameObject.SetActive(true);

        playerUI.GetComponent<PlayerUIController>().CreateUI();
        playerUI.GetComponent<PlayerUIController>().UpdateUI();



        SetState(BoardState.DiceRoll);
    }

    public Transform GetPlayer(int id)
    {
        return board.playerParent.GetChild(id);
    }

    public PlayerData GetPlayerData(int id)
    {
        return GetPlayer(id).GetComponent<PlayerController>().data;
    }

    public void SetPlayerData(int id, PlayerData pd)
    {
        GetPlayer(id).GetComponent<PlayerController>().data = pd;
    }

    public void SetState(BoardState newstate)
    {
        state = newstate;
        Camera.main.GetComponent<CameraController>().rate = Camera.main.GetComponent<CameraController>().GetRates()[(int)state];

        switch (state)
        {
            case BoardState.DiceRoll:
                Camera.main.GetComponent<CameraController>().target = currentPlayer.GetChild(0);
                break;
            case BoardState.PlayerMove:
                DiceController.dice.Show();
                Camera.main.GetComponent<CameraController>().target = currentPlayer.GetChild(0);
                break;
            case BoardState.ChoosePath:
                DiceController.dice.Hide();
                Camera.main.GetComponent<CameraController>().target = GetNode(currentPlayer.GetComponent<PlayerController>().data.place).GetChild(3);
                foreach(Transform path in GetNode(currentPlayer.GetComponent<PlayerController>().data.place).GetComponent<NodeController>().paths)
                {
                    path.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Selectable);
                }
                break;
            case BoardState.Card:
                DiceController.dice.Hide();
                Camera.main.GetComponent<CameraController>().target = GetNode(currentPlayer.GetComponent<PlayerController>().data.place).GetChild(1);
                dareUI.gameObject.SetActive(true);
                dareUI.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText("" + GetNode(currentPlayer.GetComponent<PlayerController>().data.place).GetComponent<NodeController>().data.value);
                break;
            case BoardState.NextPlayerAnimate:
                dareUI.gameObject.SetActive(false);
                int temp = queue[0];
                queue.RemoveAt(0);
                queue.Insert(queue.Count - finishedCount, temp);
                if (GetPlayerData(temp).finished)
                {
                    finishedCount++;
                }
                if (queue.Count == finishedCount)
                {
                    SetState(BoardState.End);
                    break;
                }
                currentPlayer = GetPlayer(queue[0]);
                Camera.main.GetComponent<CameraController>().target = currentPlayer.GetChild(0);
                SetState(BoardState.DiceRoll);
                break;
            case BoardState.End:
                Camera.main.GetComponent<CameraController>().target = Camera.main.GetComponent<CameraController>().overHead;
                break;
        }

        playerUI.GetComponent<PlayerUIController>().UpdateUI();
    }

    public void RolledNumber(int num)
    {
        SetState(BoardState.PlayerMove);
        dicenum = num;
    }

    private void Process()
    {
        switch (state)
        {
            case BoardState.DiceRoll:
                if (DiceController.dice.hidden)
                {
                        DiceController.dice.Standby();
                }
                break;
            case BoardState.PlayerMove:

                PlayerController pc = currentPlayer.GetComponent<PlayerController>();
                if (pc.stationary)
                {
                    if (dicenum > 0)
                    {
                        if (GetNodeData(pc.data.place).exits.Count == 1)
                        {
                            pc.SetPosition(GetNode(GetNodeData(pc.data.place).exits[0]));
                            dicenum--;
                        }
                        else
                        {
                            if (tonode != -1)
                            {
                                pc.SetPosition(GetNode(tonode));
                                tonode = -1;
                                dicenum--;
                            }
                            else
                            {
                                SetState(BoardState.ChoosePath);
                            }
                        }

                        if (GetNodeData(pc.data.place).type == "END")
                        {
                            PlayerData newpd = GetPlayerData(queue[0]);
                            newpd.finished = true;
                            SetPlayerData(queue[0], newpd);
                            dicenum = 0;
                        }

                        if (dicenum == 0)
                        {
                            DiceController.dice.Hide();
                        }
                        else
                        {
                            DiceController.dice.currentNum = dicenum;
                        }
                    } else
                    {
                        if (GetNodeData(pc.data.place).type == "END")
                        {
                            Continue(false);
                        }
                        else if (GetNodeData(pc.data.place).type == "DARE")
                        {
                            SetState(BoardState.Card);
                        }
                    }
                }
                break;
            case BoardState.ChoosePath:
                break;
            case BoardState.Card:
                break;
            case BoardState.NextPlayerAnimate:
                break;
            case BoardState.End:
                break;
        }
    }

    public void SelectPath(int nodeid)
    {

        foreach (Transform path in GetNode(currentPlayer.GetComponent<PlayerController>().data.place).GetComponent<NodeController>().paths)
        {
            path.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Idle);
        }
        SetState(BoardState.PlayerMove);
        tonode = nodeid;
    }

    public void Continue(bool addPoints)
    {
        if (addPoints)
        {
            currentPlayer.GetComponent<PlayerController>().data.score += transform.GetChild(currentPlayer.GetComponent<PlayerController>().data.place).GetComponent<NodeController>().data.value;
        }
        SetState(BoardState.NextPlayerAnimate);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public BoardData GetData()
    {
        return boardData;
    }

    public void SetData(BoardData data)
    {
        boardData = data;
    }
}
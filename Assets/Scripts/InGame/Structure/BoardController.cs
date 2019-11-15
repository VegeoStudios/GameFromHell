using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class BoardController : MonoBehaviour
{
    public static BoardController board;

    public Transform playerParent;
    public Transform endPlayerParent;

    public Transform dareUI;

    public static BoardData boardData;

    public GameObject[] nodePrefabs;
    public GameObject playerPrefab;

    public static List<PlayerData> playerData;

    public string filepath;

    public static BoardState state = BoardState.PlayerChoose;

    public static Transform currentPlayer;

    public static int queueNum = 0;

    private static int dicenum;

    private static int tonode = -1;

    public enum BoardState
    {
        PlayerChoose,
        DiceRoll,
        PlayerMove,
        ChoosePath,
        Card,
        NextPlayerAnimate
    }

    void Start()
    {
        Generate();
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
            boardData = JsonUtility.FromJson<BoardData>(File.ReadAllText(Application.dataPath + "/Boards/OG.json"));
        }

        foreach(NodeData node in boardData.nodes)
        {
            GameObject nodeObject;
            switch (node.type)
            {
                case "START":
                    nodeObject = Instantiate(nodePrefabs[0]);
                    Camera.main.GetComponent<CameraController>().target = nodeObject.transform.GetChild(3);
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

    public static Transform GetNode(int nodenum)
    {
        return board.transform.GetChild(nodenum);
    }

    public static NodeData GetNodeData(int nodenum)
    {
        return GetNode(nodenum).GetComponent<NodeController>().data;
    }

    public void StartGame()
    {
        int i = 0;
        playerData = new List<PlayerData>();
        foreach(Transform player in PlayerMenuController.playerList)
        {
            PlayerData pd = new PlayerData();
            pd.name = player.GetChild(2).GetChild(0).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text;
            if (pd.name.Length < 3) return;

            pd.color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
            pd.score = 0;
            pd.place = boardData.properties.startNodeID;
            pd.id = i;

            i++;

            playerData.Add(pd);
        }

        foreach (PlayerData pd in playerData)
        {
            GameObject player = Instantiate(playerPrefab) as GameObject;
            player.transform.SetParent(playerParent);

            player.transform.GetComponent<PlayerController>().Init(pd);
        }

        Camera.main.GetComponent<CameraController>().target = GetPlayer(0).GetChild(0);

        PlayerMenuController.playerMenuController.transform.gameObject.SetActive(false);

        currentPlayer = playerParent.GetChild(queueNum);
        SetState(BoardState.DiceRoll);
    }

    public static Transform GetPlayer(int id)
    {
        return board.playerParent.GetChild(id);
    }

    public static PlayerData GetPlayerData(int id)
    {
        return GetPlayer(id).GetComponent<PlayerController>().data;
    }

    public static void SetState(BoardState newstate)
    {
        state = newstate;

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
                Camera.main.GetComponent<CameraController>().target = GetNode(currentPlayer.GetComponent<PlayerController>().data.place).GetChild(2);
                foreach(Transform path in GetNode(currentPlayer.GetComponent<PlayerController>().data.place).GetComponent<NodeController>().paths)
                {
                    path.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Selectable);
                }
                break;
            case BoardState.Card:
                DiceController.dice.Hide();
                Camera.main.GetComponent<CameraController>().target = GetNode(currentPlayer.GetComponent<PlayerController>().data.place).GetChild(1);
                board.dareUI.gameObject.SetActive(true);
                break;
            case BoardState.NextPlayerAnimate:
                board.dareUI.gameObject.SetActive(false);
                queueNum++;
                if (queueNum == board.playerParent.childCount) queueNum = 0;
                currentPlayer = GetPlayer(queueNum);
                Camera.main.GetComponent<CameraController>().target = currentPlayer.GetChild(0);
                SetState(BoardState.DiceRoll);
                break;
        }
    }

    public static void RolledNumber(int num)
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
                            RemoveFromQueue(pc.data.id);
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
                            Continue();
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
        }
    }

    public static void SelectPath(int nodeid)
    {
        SetState(BoardState.PlayerMove);
        tonode = nodeid;
    }

    public void Continue()
    {
        SetState(BoardState.NextPlayerAnimate);
    }

    public void RemoveFromQueue(int id)
    {
        GetPlayer(id).SetParent(endPlayerParent);
    }
}
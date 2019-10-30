using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;


    public Transform currentPlayer;

    public GameObject playerPrefab;

    public List<Transform> playerList = new List<Transform>();

    public Transform startNode;
    public Transform endNode;

    public Transform playerParent;

    public GameState state;

    public enum GameState
    {
        SelectPlayers,
        Preview,
        PlayerRoll,
        MovePlayer,
        ChoosePath,
        Card
    }

    public void SetGameState(GameState state)
    {
        this.state = state;

        switch (state)
        {
            case GameState.SelectPlayers:

                break;
            case GameState.Preview:

                break;
            case GameState.PlayerRoll:

                break;
            case GameState.MovePlayer:

                break;
            case GameState.ChoosePath:

                break;
            case GameState.Card:

                break;
        }
    }

    private void Start()
    {

        Init();
        CreatePlayers();
    }

    private void Init()
    {
        gm = this;
    }

    private void CreatePlayers()
    {
        startNode = transform.GetChild(0).GetChild(0);
        endNode = transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1);

        playerParent = transform.GetChild(1);
        GameObject player1 = Instantiate(playerPrefab) as GameObject;
        player1.transform.parent = playerParent;
        player1.transform.GetComponent<PlayerController>().Initiate("Player 1", Color.red);
        playerList.Add(player1.transform);
        GameObject player2 = Instantiate(playerPrefab) as GameObject;
        player2.transform.parent = playerParent;
        player2.transform.GetComponent<PlayerController>().Initiate("Player 2", Color.blue);
        playerList.Add(player2.transform);

        SetGameState(GameState.PlayerRoll);
    }
}

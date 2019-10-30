using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public static UIcontroller ui;

    private Transform player;

    private Transform topBar;
    private Transform nameText;
    private Transform scoreText;


    private void Start()
    {
        ui = this;

        topBar = transform.GetChild(0);
        nameText = transform.GetChild(0).GetChild(0);
        scoreText = transform.GetChild(0).GetChild(1).GetChild(0);
    }

    public void UpdatePlayer()
    {
        this.player = GameManager.gm.currentPlayer;

        topBar.GetComponent<MeshRenderer>().material.color = player.GetComponent<PlayerController>().color;
        nameText.GetComponent<TextMesh>().text = player.GetComponent<PlayerController>().playerName;
        scoreText.GetComponent<TextMesh>().text = player.GetComponent<PlayerController>().score.ToString();
    }

    public void UpdateScore()
    {
        scoreText.GetComponent<TextMesh>().text = player.GetComponent<PlayerController>().score.ToString();
    }
}

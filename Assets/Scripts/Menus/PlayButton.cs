using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public BoardData board;

    public void Play()
    {
        BoardController.boardData = board;
        SceneManager.LoadScene(1);
    }
}

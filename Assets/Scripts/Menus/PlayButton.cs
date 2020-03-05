using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public BoardData board;

    public bool create;

    public void Play()
    {
        if (!create)
        {
            BoardController.boardData = board;
            SceneManager.LoadScene(1);
        } else
        {
            CreationController.loading = true;
            CreationController.boardData = board;
            SceneManager.LoadScene(2);
        }
    }

    public void Delete()
    {
        System.IO.File.Delete(Application.persistentDataPath + "/BOARDS/CUSTOM/" + board.properties.name + ".json");
        transform.parent.GetComponent<BoardSelect>().ClearButtons();
        transform.parent.GetComponent<BoardSelect>().LoadBoards("CUSTOM");
    }
}

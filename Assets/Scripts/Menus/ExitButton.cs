using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void Exit()
    {
        CreationController.boardData = null;
        CreationController.loading = false;
        PathAnimator.paths = null;
        SceneManager.LoadScene(0);
    }
}

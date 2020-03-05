using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateButton : MonoBehaviour
{
    public void Create()
    {
        if (new System.IO.DirectoryInfo(Application.persistentDataPath + "/BOARDS/CUSTOM/").GetFiles().Length >= 5)
        {
            Warning.w.Warn("You can not have more than 5 boards");
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
}

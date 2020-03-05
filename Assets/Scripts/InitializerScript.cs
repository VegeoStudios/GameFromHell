using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class InitializerScript : MonoBehaviour
{
    private List<BoardData> defaultBoards = new List<BoardData>();

    public void Start()
    {
        //Debug.Log(Application.platform);
        if (System.IO.File.Exists(Application.persistentDataPath + "/BOARDS/")) return;
        GetDefaultBoards();
        CreateFiles();
    }

    private void CreateFiles()
    {

        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/BOARDS/DEFAULT/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/BOARDS/CUSTOM/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/BOARDS/PURCHASED/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/BOARDS/COMMUNITY/");

        foreach (BoardData data in defaultBoards)
        {
            Debug.Log(data.properties.name);
            string path = Application.persistentDataPath + "/BOARDS/DEFAULT/" + data.properties.name + ".json";
            File.WriteAllText(path, JsonUtility.ToJson(data));
        }
    }

    private void GetDefaultBoards()
    {

        defaultBoards.Add(JsonUtility.FromJson<BoardData>(Resources.Load<TextAsset>("OG").text));
    }
}

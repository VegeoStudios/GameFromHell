using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BoardSelect : MonoBehaviour
{

    public GameObject buttonPrefab;

    public bool create;

    private int count;

    public void LoadBoards(string type)
    {
        string datapath = Application.persistentDataPath;

        DirectoryInfo dir = new DirectoryInfo(datapath + "/BOARDS/" + type + "/");
        FileInfo[] files = dir.GetFiles();

        count = 0;

        foreach (FileInfo file in files)
        {
            if (file.Extension == ".meta") continue;

            AddButton(file.FullName);
        }
    }

    private void AddButton(string dir)
    {
        BoardData board = JsonUtility.FromJson<BoardData>(File.ReadAllText(dir));

        GameObject button = Instantiate(buttonPrefab) as GameObject;
        button.transform.SetParent(transform);
        button.transform.GetComponent<RectTransform>().anchoredPosition= new Vector2(0, -350 - (125 * count));
        button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = board.properties.name;
        button.transform.GetComponent<PlayButton>().board = board;
        button.transform.GetComponent<PlayButton>().create = create;
        button.transform.GetChild(1).gameObject.SetActive(create);

        count++;
    }

    public void ClearButtons()
    {
        for (int i = transform.childCount; i > 1; i--)
        {
            Destroy(transform.GetChild(i - 1).gameObject);
        }
    }

    public void SetCreate(bool c)
    {
        create = c;
    }
}

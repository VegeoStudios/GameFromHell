using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BoardSelect : MonoBehaviour
{

    public GameObject buttonPrefab;

    private List<BoardData> loadedBoards;

    private List<Transform> buttons = new List<Transform>();

    public void LoadBoards(string type)
    {
        loadedBoards = new List<BoardData>();
        foreach (Transform button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons = new List<Transform>();
        switch (type)
        {
            case "DEFAULT":
                int i = 0;
                foreach(string dir in Directory.GetFiles(Application.dataPath + "/Boards/"))
                {
                    if (dir.IndexOf(".meta") != -1) continue;
                    BoardData board = JsonUtility.FromJson<BoardData>(File.ReadAllText(dir));

                    loadedBoards.Add(board);

                    GameObject button = Instantiate(buttonPrefab) as GameObject;
                    buttons.Add(button.transform);
                    button.transform.SetParent(transform);
                    button.transform.localPosition = new Vector3(0, 300 + 120 * i, 0);

                    button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = loadedBoards[i].properties.name;
                    button.transform.GetComponent<PlayButton>().board = board;

                    i++;
                }
                break;
        }
    }
}

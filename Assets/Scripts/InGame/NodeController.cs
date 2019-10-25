using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NodeController : MonoBehaviour
{
    public List<Transform> exits = new List<Transform>();

    public int pointValue;
    public bool shown;

    public List<GameObject> extras = new List<GameObject>();


    private void Start()
    {
        SetVisibility();
        ProcessText();
    }

    public void Show()
    {
        transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

    public void Hide()
    {
        transform.localEulerAngles = new Vector3(90, 0, 0) ;
    }

    private void SetVisibility()
    {
        if (!shown)
        {
            Hide();
        }
    }

    private void ProcessText()
    {
        string text = transform.GetChild(0).GetComponent<TextMesh>().text;
        Regex filter = new Regex("\\[(.*?)\\]");
        Regex split = new Regex(":");
        MatchCollection mc = filter.Matches(text);

        List<string> components = new List<string>();
        foreach(Match m in mc)
        {
            components.Add(m.Value);
        }

        foreach(string c in components)
        {
            string cc = c.Substring(1, c.Length - 2);
            string[] parts = split.Split(cc);

            switch (parts[0])
            {
                case "time":
                    text = text.Replace(c, parts[1] + " seconds");
                    print(c);
                    print(text);
                    transform.GetChild(0).GetComponent<TextMesh>().text = text;
                    GameObject timer = Instantiate(extras[0]) as GameObject;
                    timer.transform.parent = this.transform;
                    timer.GetComponent<TimerController>().SetTime(int.Parse(parts[1]));
                    break;
                default:

                    break;
            }
        }

    }
}

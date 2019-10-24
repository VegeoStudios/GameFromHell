using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NodeController : MonoBehaviour
{
    public List<Transform> exits = new List<Transform>();

    public int pointValue;
    public bool shown;


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
            string[] parts = split.Split(c);

            switch (parts[0])
            {
                case "time":

                    break;
                default:

                    break;
            }
        }

    }
}

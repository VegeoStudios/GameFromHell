using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class DareNode : MonoBehaviour
{

    public List<GameObject> extras = new List<GameObject>();

    public void Init()
    {
        ProcessText();
    }

    private void ProcessText()
    {
        string text = transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text;
        Regex filter = new Regex("\\[(.*?)\\]");
        Regex split = new Regex(":");
        MatchCollection mc = filter.Matches(text);

        List<string> components = new List<string>();
        foreach (Match m in mc)
        {
            components.Add(m.Value);
        }

        foreach (string c in components)
        {
            string cc = c.Substring(1, c.Length - 2);
            string[] parts = split.Split(cc);

            switch (parts[0])
            {
                case "time":
                    text = text.Replace(c, parts[1] + " seconds");
                    transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = text;
                    GameObject timer = Instantiate(extras[0]) as GameObject;
                    timer.transform.parent = this.transform;
                    timer.transform.localPosition = Vector3.zero;
                    timer.GetComponent<TimerController>().SetTime(int.Parse(parts[1]));
                    break;
                default:

                    break;
            }
        }

    }
}

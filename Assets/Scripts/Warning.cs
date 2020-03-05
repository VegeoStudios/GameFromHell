using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    private int timer = 0;
    private float pos = 50;
    private float hiddenpos = -100;
    private float target = -100;

    public static Warning w;

    public void Warn(string warning)
    {
        timer = 120;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = warning;
    }


    public void Start()
    {
        hiddenpos = GetComponent<RectTransform>().localPosition.y;
        pos = hiddenpos + 150;
        target = hiddenpos;
        w = this;
    }
    public void Update()
    {
        Process();
        Animate();
    }

    private void Process()
    {
        if (timer > 0)
        {
            timer--;
            target = pos;
        }
        else
        {
            target = hiddenpos;
        }
    }

    private void Animate()
    {
        GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Lerp(GetComponent<RectTransform>().localPosition.y, target, 0.1f));
    }
}

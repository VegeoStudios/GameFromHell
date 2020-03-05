using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardAnimator : MonoBehaviour
{
    private float target;
    private float start;

    private void Update()
    {
        Process();
        Animate();
    }

    private void Start()
    {
        start = GetComponent<RectTransform>().anchoredPosition.y;
    }

    private void Process()
    {
        if (TouchScreenKeyboard.visible)
        {
            target = TouchScreenKeyboard.area.yMin;
        } else
        {
            target = start;
        }
    }

    private void Animate()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, new Vector2(rt.anchoredPosition.x, target), 0.5f);
    }
}

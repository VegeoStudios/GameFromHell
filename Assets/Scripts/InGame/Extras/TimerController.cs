using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public int timeValue;
    private int time;
    public TextMesh text;

    public void StartTime()
    {
        time = timeValue;
        text.color = Color.green;
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    public void SetTime(int newtime)
    {
        timeValue = newtime;
        time = newtime;
        text.text = time.ToString();
    }

    IEnumerator LoseTime()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            text.text = time.ToString();
        }
        text.color = Color.red;
    }
}

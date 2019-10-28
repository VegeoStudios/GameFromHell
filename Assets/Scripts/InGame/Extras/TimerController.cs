using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public int timeValue;
    private int time;
    public TextMesh text;
    private bool started = false;
    private int position = 0;
    private bool activated = false;

    private void Update()
    {
        if (activated) DetectTouch();
        ChangePosition();
    }

    public void StartTime()
    {
        time = timeValue;
        text.color = Color.green;
        activated = false;
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    public void Activate()
    {
        activated = true;
    }

    public void ResetTimer()
    {
        time = timeValue;
    }

    public void SetTime(int newtime)
    {
        timeValue = newtime;
        time = newtime;
        text.text = time.ToString();
    }

    IEnumerator LoseTime()
    {
        started = true;
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            text.text = time.ToString();
        }
        position = 0;
        text.color = Color.red;
    }

    private void DetectTouch()
    {
        if (started) return;

        position = 0;

        foreach (Touch touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.transform == transform)
            {
                position = 1;

                if (touch.phase == TouchPhase.Ended)
                {
                    position = 2;
                    StartTime();
                }
            }
        }
    }

    private void ChangePosition()
    {
        Vector3 positionA = new Vector3(0, -1.5f, -0.1f);
        Vector3 positionC = new Vector3(0, 0, -0.1f);

        Vector3 scaleA = new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 scaleB = new Vector3(0.3f, 0.3f, 0.3f);
        Vector3 scaleC = new Vector3(1, 1, 1);

        Vector3 newposition;
        Vector3 newscale;

        if (position == 0)
        {
            newposition = positionA;
            newscale = scaleA;
        }
        else if (position == 1)
        {
            newposition = positionA;
            newscale = scaleB;
        }
        else
        {
            newposition = positionC;
            newscale = scaleC;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, newposition, Time.deltaTime * 4);
        transform.localScale = Vector3.Lerp(transform.localScale, newscale, Time.deltaTime * 8);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerButton : MonoBehaviour
{
    private float targetLoc;

    public void Update()
    {
        Process();
        Animate();
    }

    private void Animate()
    {
        if (Mathf.Abs(targetLoc - transform.localPosition.y) > 0.1)
        {
            transform.localPosition = new Vector3(0, Mathf.Lerp(transform.localPosition.y, targetLoc, 0.1f));
        }
        else if (targetLoc != transform.localPosition.y)
        {
            transform.localPosition = new Vector3(0, targetLoc, 0);
        }
    }

    private void Process()
    {
        targetLoc = 450 - (PlayerMenuController.playerList.Count) * 120;
    }
}

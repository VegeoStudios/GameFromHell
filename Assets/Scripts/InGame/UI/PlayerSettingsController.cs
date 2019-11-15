using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsController : MonoBehaviour
{
    private bool dragging = false;
    private float targetLoc;
    private float targetScale = 1;

    private float previousPos = 0;

    public void Update()
    {
        Process();
        Animate();
    }

    public void StartDrag()
    {
        dragging = true;
        targetScale = 1.2f;
    }

    public void EndDrag()
    {
        dragging = false;
        targetScale = 1;

        int newpos = Mathf.FloorToInt((-targetLoc + 500) / 120);

        if (newpos < 0) newpos = 0;
        if (newpos > PlayerMenuController.playerList.Count - 1) newpos = PlayerMenuController.playerList.Count - 1;


        PlayerMenuController.playerList.Remove(transform);
        PlayerMenuController.playerList.Insert(newpos, transform);
    }

    public void RemovePlayer()
    {
        PlayerMenuController.playerList.Remove(transform);
        Destroy(transform.gameObject);
        PlayerMenuController.playerMenuController.newPlayerButton.gameObject.SetActive(true);
        if (PlayerMenuController.playerList.Count < 2) PlayerMenuController.playerMenuController.playButton.gameObject.SetActive(false);
    }

    private void Animate()
    {
        if (Mathf.Abs(targetScale - transform.localScale.x) > 1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, targetScale), 0.1f);
        } else if (targetScale != transform.localScale.x)
        {
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        }

        if (Mathf.Abs(targetLoc - transform.localPosition.y) > 1)
        {
            transform.localPosition = new Vector3(0, Mathf.Lerp(transform.localPosition.y, targetLoc, 0.1f));
        } else if (targetLoc != transform.localPosition.y)
        {
            transform.localPosition = new Vector3(0, targetLoc, 0);
        }
    }

    private void Process()
    {
        if (dragging)
        {
            targetLoc = Input.GetTouch(0).position.y - 600;
        } else
        {
            targetLoc = 440 - PlayerMenuController.playerList.IndexOf(transform) * 120;
        }

        previousPos = targetLoc;
    }
}

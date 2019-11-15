using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuController : MonoBehaviour
{
    public bool dragging = false;
    public GameObject playerSettingsPrefab;

    public static List<Transform> playerList = new List<Transform>();
    public static PlayerMenuController playerMenuController;

    public Transform newPlayerButton;
    public Transform playButton;

    public void Start()
    {
        playerMenuController = this;
    }

    public void AddPlayer()
    {
        GameObject ps = Instantiate(playerSettingsPrefab) as GameObject;
        ps.transform.SetParent(transform);
        ps.transform.localPosition = newPlayerButton.localPosition;

        playerList.Add(ps.transform);

        if (playerList.Count == 8)
        {
            newPlayerButton.gameObject.SetActive(false);
        }

        if (playerList.Count > 1) playButton.gameObject.SetActive(true);
    }
}

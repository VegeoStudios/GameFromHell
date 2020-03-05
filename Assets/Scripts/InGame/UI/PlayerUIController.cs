using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    public static PlayerUIController ui;

    public GameObject playerUI;

    private Transform player;

    private void Start()
    {
        ui = this;
    }

    public void CreateUI()
    {
        List<int> queue = BoardController.board.queue;
        foreach (int id in queue)
        {
            Transform ui = (Instantiate(playerUI) as GameObject).transform;
            ui.SetParent(transform, false);

            ui.GetComponent<PlayerUI>().SetPlayer(BoardController.board.GetPlayer(id));
        }
    }

    public void UpdateUI()
    {
        List<PlayerUI> uis = new List<PlayerUI>(transform.GetComponentsInChildren<PlayerUI>());

        foreach (PlayerUI ui in uis)
        {
            ui.UpdateUI();
        }

    }
}

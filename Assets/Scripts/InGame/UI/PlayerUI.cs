using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    private Transform player;

    public void UpdateUI()
    {
        PlayerData pd = player.GetComponent<PlayerController>().data;

        transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().color = pd.color;
        transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = pd.name;
        transform.GetChild(1).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = pd.score.ToString();
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}

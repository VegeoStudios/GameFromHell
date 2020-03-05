using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public Transform player;

    public Transform iColor;
    public Transform iScore;
    public Transform aColor;
    public Transform aScore;
    public Transform playerName;
    public Transform flag;

    public Transform a;
    public Transform i;

    private float target;

    private bool active;

    public void Update()
    {
        Animate();
    }
    public void UpdateUI()
    {
        PlayerData pd = this.player.GetComponent<PlayerController>().GetData();
        aScore.GetComponent<TMPro.TextMeshProUGUI>().text = pd.score.ToString();
        iScore.GetComponent<TMPro.TextMeshProUGUI>().text = pd.score.ToString();

        if (BoardController.board.queue[0] == pd.id)
        {
            target = 0;
            a.gameObject.SetActive(true);
            i.gameObject.SetActive(false);
        } else
        {
            target = -130 - 90 * (BoardController.board.queue.IndexOf(pd.id) - 1);
            a.gameObject.SetActive(false);
            i.gameObject.SetActive(true);
        }

        if (pd.finished) flag.gameObject.SetActive(true);
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;

        PlayerData pd = player.GetComponent<PlayerController>().data;

        aColor.GetComponent<UnityEngine.UI.Image>().color = pd.color;
        iColor.GetComponent<UnityEngine.UI.Image>().color = pd.color;
        playerName.GetComponent<TMPro.TextMeshProUGUI>().text = pd.name;
    }

    public void setActive(bool active)
    {
        this.active = active;
    }

    private void Animate()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.localPosition = Vector2.Lerp(rt.localPosition, new Vector2(rt.localPosition.x, target), 0.5f);
    }
}

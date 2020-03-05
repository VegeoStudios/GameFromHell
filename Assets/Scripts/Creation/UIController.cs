using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public Transform path;
    public Transform editui;
    public Transform type;
    public Transform value;

    public enum UIState
    {
        Hidden,
        Dare,
        Start,
        End
    }

    public UIState state = UIState.Hidden;

    public static UIController ui;

    public void SetState(UIState state)
    {
        this.state = state;
        switch (state)
        {
            case UIState.Hidden:
                editui.gameObject.SetActive(false);
                break;
            case UIState.Dare:
                editui.gameObject.SetActive(true);
                path.gameObject.SetActive(true);
                type.gameObject.SetActive(true);
                value.gameObject.SetActive(true);
                value.GetChild(2).GetComponent<NodeVal>().Set();
                break;
            case UIState.Start:
                editui.gameObject.SetActive(true);
                path.gameObject.SetActive(true);
                type.gameObject.SetActive(false);
                value.gameObject.SetActive(false);
                break;
            case UIState.End:
                editui.gameObject.SetActive(true);
                path.gameObject.SetActive(false);
                type.gameObject.SetActive(true);
                value.gameObject.SetActive(false);
                break;
        }
    }

    public void Start()
    {
        ui = this;
    }
}

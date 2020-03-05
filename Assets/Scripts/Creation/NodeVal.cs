using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVal : MonoBehaviour
{
    public void Set()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = CreationController.cc.selected.GetComponent<CreationNode>().value.ToString();
    }

    public void Increase()
    {
        if (int.Parse(GetComponent<TMPro.TextMeshProUGUI>().text) == 10) return;
        GetComponent<TMPro.TextMeshProUGUI>().text = (int.Parse(GetComponent<TMPro.TextMeshProUGUI>().text) + 1).ToString();
        CreationController.cc.selected.GetComponent<CreationNode>().value++;
    }

    public void Decrease()
    {
        if (int.Parse(GetComponent<TMPro.TextMeshProUGUI>().text) == 0) return;
        GetComponent<TMPro.TextMeshProUGUI>().text = (int.Parse(GetComponent<TMPro.TextMeshProUGUI>().text) - 1).ToString();
        CreationController.cc.selected.GetComponent<CreationNode>().value--;
    }
}

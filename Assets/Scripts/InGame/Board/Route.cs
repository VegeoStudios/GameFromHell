using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    public List<Transform> childNodeList = new List<Transform>();

    public Texture pathTexture;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        FillNodes();

        foreach(Transform child in childNodeList)
        {
            ((NodeController)child.GetComponent(typeof(NodeController))).DrawRoute();
        }
    }

    void FillNodes()
    {
        childNodeList.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            childNodeList.Add(transform.GetChild(i));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{

    public List<Transform> toList;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DrawRoute();
    }

    public void DrawRoute()
    {
        foreach (Transform dest in toList)
        {
            Gizmos.DrawLine(transform.position, dest.position);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDisplay : MonoBehaviour
{
    private int offset = 0;

    private List<Transform> childlist = new List<Transform>();
    private List<LineRenderer> lrlist = new List<LineRenderer>();

    public void OnDrawGizmos()
    {
        List<Transform> tolist = ((NodeController)GetComponent(typeof(NodeController))).toList;

        FillList();

        if (childlist.Count != tolist.Count)
        {
            childlist.Clear();
            foreach(Transform child in childlist)
            {
                DestroyImmediate(child);
            }

            foreach(Transform loc in tolist)
            {
                GameObject newChild = new GameObject("Path");
                newChild.transform.parent = transform;
                newChild.transform.localPosition = Vector3.zero;
                LineRenderer lr = newChild.AddComponent(typeof(LineRenderer)) as LineRenderer;
                lr.SetPositions(new Vector3[] { transform.position, loc.position});
            }
        }
    }

    private void FillList()
    {
        childlist.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.name == "Path")
            {
                childlist.Add(child);
            }
        }
    }
}

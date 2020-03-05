using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeAnimator : MonoBehaviour
{
    private float baseScale;
    public float bigScale = 1.5f;
    private bool over = false;

    public CreationNode.NodeType type;
    void Start()
    {
        baseScale = transform.localScale.x;
    }

    private void Update()
    {
        Animate();
    }

    public void PointerEnter()
    {
        over = true;
    }

    public void PointerExit()
    {
        over = false;
    }

    private void Animate()
    {
        float to = baseScale;
        if (over)
        {
            to = bigScale;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(to, to, to), 0.4f);
    }

    public void Select()
    {
        if (over)
        {
            Transform temp = CreationController.cc.selected;
            CreationController.cc.selected.GetComponent<CreationNode>().SetType(type);
            CreationController.cc.Deselect();
            CreationController.cc.Select(temp);
            if (type == CreationNode.NodeType.End)
            {
                List<Transform> pathstoremove = new List<Transform>();

                foreach (Transform path in PathAnimator.paths)
                {
                    if (path.GetComponent<PathAnimator>().from == temp)
                    {
                        pathstoremove.Add(path);
                    }
                }

                foreach (Transform path in pathstoremove)
                {
                    PathAnimator.paths.Remove(path);
                    temp.GetComponent<CreationNode>().exits.Remove(path.GetComponent<PathAnimator>().destination);
                    Destroy(path.gameObject);
                }
            }
        }

        over = false;
        transform.localScale = new Vector3(baseScale, baseScale, baseScale);
    }
}

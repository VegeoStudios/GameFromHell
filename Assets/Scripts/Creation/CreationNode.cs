using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CreationNode : MonoBehaviour
{
    private int taptimer = 0;
    private int maxtaptime = 10;

    public enum NodeType
    {
        Start,
        Dare,
        End
    }

    public int value = 0;

    public NodeType type;

    private bool dragging = false;

    public bool canDestroy = false;

    public List<Transform> exits = new List<Transform>();

    public Mesh dareMesh;
    public Mesh keyMesh;
    public Material dareMat;
    public Material keyMat;
    public Color dareColor;
    public Color keyColor;

    public void Update()
    {
        Timer();
        if (dragging) Move();
        Animate();
    }

    private void Timer()
    {
        if (taptimer > 0) taptimer--;
    }

    public void SetType(NodeType type)
    {
        this.type = type;
        switch (type)
        {
            case NodeType.Start:
                canDestroy = false;
                GetComponent<MeshRenderer>().material = keyMat;
                GetComponent<MeshFilter>().mesh = keyMesh;
                SetTextColor(keyColor);
                SetText("START");
                break;
            case NodeType.Dare:
                canDestroy = true;
                GetComponent<MeshRenderer>().material = dareMat;
                GetComponent<MeshFilter>().mesh = dareMesh;
                SetTextColor(dareColor);
                SetText("DARE");
                break;
            case NodeType.End:
                canDestroy = true;
                GetComponent<MeshRenderer>().material = keyMat;
                GetComponent<MeshFilter>().mesh = keyMesh;
                SetTextColor(keyColor);
                SetText("END");
                break;
        }
    }

    public void Tapped()
    {
        CreationController.cc.Select(transform);
        if (taptimer == 0)
        {
            taptimer = maxtaptime;
        } else
        {
            taptimer = 0;
            if (type == NodeType.Dare) EditText();
        }
    }

    public void BeginDrag()
    {
        if (CreationController.cc.selected == transform)
        {
            dragging = true;
        }
    }

    public void EndDrag()
    {
        if (dragging)
        {
            dragging = false;
            CreationController.cc.PlaceNode(transform);
        }
    }

    private void Move()
    {
        if (Input.touchCount != 0)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.transform.position.y - CreationController.cc.backgroundPlane.position.y));
            pos.y = 0;

            if (CreationController.cc.snapping)
            {
                pos /= CreationController.cc.gridSize;
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                pos *= CreationController.cc.gridSize;
            }

            if (canDestroy && CreationController.cc.overEraser)
            {
                RectTransform eraser = CreationController.cc.eraser.GetComponent<RectTransform>();
                Vector3 eraserPos = Camera.main.WorldToScreenPoint(eraser.position);
                pos = Camera.main.ScreenToWorldPoint(new Vector3(eraserPos.x, eraserPos.y, Camera.main.transform.position.y));
            }

            transform.position = pos;
        }
    }

    private void Animate()
    {
        float targetScale = 1;
        if (dragging) targetScale = 1.2f;

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, targetScale), 0.2f);
    }

    public void PointerEnter()
    {
        CreationController.cc.addPathNode = transform;
    }

    public void PointerExit()
    {
        CreationController.cc.addPathNode = null;
    }

    public void EditText()
    {
        CreationController.cc.OpenTextEditor(transform);
    }

    public void Remove()
    {
        List<Transform> pathstoremove = new List<Transform>();

        foreach (Transform path in PathAnimator.paths)
        {
            if (path.GetComponent<PathAnimator>().from == transform)
            {
                pathstoremove.Add(path);
            } else if (path.GetComponent<PathAnimator>().destination == transform)
            {
                path.GetComponent<PathAnimator>().from.GetComponent<CreationNode>().exits.Remove(transform);
                pathstoremove.Add(path);
            }
        }

        foreach(Transform path in pathstoremove)
        {
            PathAnimator.paths.Remove(path);
            Destroy(path.gameObject);
        }

        CreationController.cc.Deselect();
        Destroy(gameObject);
    }

    public void SetText(string text)
    {
        transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = text;
    }

    public void SetTextColor(Color color)
    {
        transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().color = color;
    }
}

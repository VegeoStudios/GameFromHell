using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationController : MonoBehaviour
{
    public Transform selected;

    public Shader selectShader;
    public Shader standardShader;

    public GameObject nodePrefab;

    public GameObject pathPrefab;

    public static CreationController cc;

    public bool snapping = true;
    public float gridSize = 5;

    private Transform creatingPath;

    public Transform addPathNode;

    public TouchScreenKeyboard keyboard;

    public bool overEraser = false;

    public Transform eraser;
    public Transform textInput;

    public Transform backgroundPlane;
    public Transform ui;

    public static bool loading = false;
    public static BoardData boardData;

    public Transform boardName;

    public void Start()
    {
        Deselect();
        cc = this;
        if (loading)
        {
            LoadBoard();
        } else
        {
            NewBoard();
        }
        boardName.GetComponent<TMPro.TMP_InputField>().readOnly = loading;
    }

    private void NewBoard()
    {
        boardData = new BoardData();
        GameObject start = Instantiate(nodePrefab) as GameObject;
        start.transform.parent = transform;
        start.transform.position = Vector3.zero;
        start.GetComponent<CreationNode>().SetType(CreationNode.NodeType.Start);
    }

    private void LoadBoard()
    {
        boardName.GetComponent<TMPro.TMP_InputField>().text = boardData.properties.name;
        
        foreach (NodeData node in boardData.nodes)
        {
            GameObject nodeObject = Instantiate(nodePrefab) as GameObject;
            if (node.type == "DARE") nodeObject.GetComponent<CreationNode>().SetType(CreationNode.NodeType.Dare);
            else if (node.type == "START") nodeObject.GetComponent<CreationNode>().SetType(CreationNode.NodeType.Start);
            else if (node.type == "END") nodeObject.GetComponent<CreationNode>().SetType(CreationNode.NodeType.End);
            nodeObject.transform.parent = transform;
            nodeObject.transform.position = new Vector3(node.pos[0], 0, node.pos[1]);
            if (node.type == "DARE") nodeObject.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = node.text;
        }

        foreach (NodeData node in boardData.nodes)
        {
            foreach (int exit in node.exits)
            {
                GameObject path = Instantiate(pathPrefab) as GameObject;
                path.transform.parent = transform.GetChild(node.id);
                path.GetComponent<PathAnimator>().SetNodes(transform.GetChild(node.id), transform.GetChild(exit));
                path.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Idle);
                path.GetComponent<PathAnimator>().Init();
                transform.GetChild(node.id).GetComponent<CreationNode>().exits.Add(transform.GetChild(exit));
            }
        }

        Camera.main.transform.position = new Vector3(boardData.nodes[boardData.properties.startNodeID].pos[0], 30, boardData.nodes[boardData.properties.startNodeID].pos[1]);
    }

    public void BeginDragCreateNode()
    {
        GameObject node = Instantiate(nodePrefab) as GameObject;
        node.transform.parent = transform;
        node.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.transform.position.y - CreationController.cc.backgroundPlane.position.y)); ;
        node.GetComponent<CreationNode>().SetType(CreationNode.NodeType.Dare);

        if (selected != null)
        {
            GameObject path = Instantiate(pathPrefab) as GameObject;
            path.transform.parent = selected;
            path.GetComponent<PathAnimator>().SetNodes(selected, node.transform);
            path.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Idle);
            path.GetComponent<PathAnimator>().Init();
            selected.GetComponent<CreationNode>().exits.Add(node.transform);
        }
        Select(node.transform);
        node.GetComponent<CreationNode>().BeginDrag();
    }

    public void EndDragCreateNode()
    {
        selected.GetComponent<CreationNode>().EndDrag();
    }

    public void TapCreateNode()
    {
        GameObject node = Instantiate(nodePrefab) as GameObject;
        node.transform.parent = transform;
        Vector3 pos = Camera.main.transform.position;
        pos.y = 0;
        node.transform.position = pos;

        node.GetComponent<CreationNode>().SetType(CreationNode.NodeType.Dare);
        PlaceNode(node.transform);

        if (selected != null)
        {
            GameObject path = Instantiate(pathPrefab) as GameObject;
            path.transform.parent = selected;
            path.GetComponent<PathAnimator>().SetNodes(selected, node.transform);
            path.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Idle);
            path.GetComponent<PathAnimator>().Init();
            selected.GetComponent<CreationNode>().exits.Add(node.transform);
        }
    }

    public void BeginDragPath()
    {
        addPathNode = null;
        creatingPath = (Instantiate(pathPrefab) as GameObject).transform;
        creatingPath.parent = selected;
        creatingPath.GetComponent<PathAnimator>().SetNodes(selected, null);
        creatingPath.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.ActivePath);
    }

    public void EndDragPath()
    {
        if (addPathNode == null)
        {
            Destroy(creatingPath.gameObject);
            return;
        }
        if (addPathNode.GetComponent<CreationNode>().type == CreationNode.NodeType.Start || addPathNode == creatingPath.GetComponent<PathAnimator>().from || creatingPath.GetComponent<PathAnimator>().from.GetComponent<CreationNode>().exits.IndexOf(addPathNode) != -1 || addPathNode.GetComponent<CreationNode>().exits.IndexOf(creatingPath.GetComponent<PathAnimator>().from) != -1)
        {
            Destroy(creatingPath.gameObject);
            return;
        }

        creatingPath.GetComponent<PathAnimator>().SetNodes(selected, addPathNode);
        creatingPath.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Idle);
        creatingPath.GetComponent<PathAnimator>().Init();
        selected.GetComponent<CreationNode>().exits.Add(addPathNode);

        creatingPath = null;
    }

    public void Select(Transform toselect)
    {
        
        if (selected != null)
        {
            selected.GetComponent<MeshRenderer>().material.shader = standardShader;
        }
        toselect.GetComponent<MeshRenderer>().material.shader = selectShader;
        selected = toselect;
        if (selected.GetComponent<CreationNode>().type == CreationNode.NodeType.Dare)
        {
            UIController.ui.SetState(UIController.UIState.Dare);
        } else if (selected.GetComponent<CreationNode>().type == CreationNode.NodeType.Start)
        {
            UIController.ui.SetState(UIController.UIState.Start);
        } else if (selected.GetComponent<CreationNode>().type == CreationNode.NodeType.End)
        {
            UIController.ui.SetState(UIController.UIState.End);
        }
    }

    public void Deselect()
    {
        textInput.gameObject.SetActive(false);

        if (selected != null)
        {
            selected.GetComponent<MeshRenderer>().material.shader = standardShader;
        }
        UIController.ui.SetState(UIController.UIState.Hidden);
        selected = null;
    }

    public void PlaceNode(Transform node)
    {
        
        if (overEraser && node.GetComponent<CreationNode>().canDestroy)
        {
            node.GetComponent<CreationNode>().Remove();
        }

        if (node.GetComponent<CreationNode>().type != CreationNode.NodeType.Start) node.GetComponent<CreationNode>().canDestroy = true;
    }

    public void EnterEraser()
    {
        overEraser = true;
    }

    public void ExitEraser()
    {
        overEraser = false;
    }

    public void OpenTextEditor(Transform node)
    {
        textInput.gameObject.SetActive(true);
        textInput.GetComponent<TMPro.TMP_InputField>().text = selected.GetChild(0).GetComponent<TMPro.TextMeshPro>().text;
    }

    public void SetText()
    {
        selected.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = textInput.GetComponent<TMPro.TMP_InputField>().text;
        textInput.gameObject.SetActive(false);
    }
}

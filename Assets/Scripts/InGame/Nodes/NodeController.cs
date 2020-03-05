using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NodeController : MonoBehaviour
{
    public List<Transform> exitTransforms = new List<Transform>();
    public List<Transform> paths = new List<Transform>();

    private bool shown;

    public NodeData data;

    public GameObject pathObject;

    public Transform cardCamPos;
    public Transform nodeCamPos;

    public void Init()
    {
        transform.position = new Vector3(data.pos[0], 0, data.pos[1]);
        
        if (data.type == "DARE")
        {
            transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = data.text;
            transform.GetComponent<DareNode>().Init();
        }
    }

    public void CreatePaths()
    {
        foreach (int exitid in data.exits)
        {
            exitTransforms.Add(BoardController.board.GetNode(exitid));
        }
        foreach (Transform exit in exitTransforms)
        {
            GameObject path = Instantiate(pathObject) as GameObject;
            path.transform.parent = transform;
            paths.Add(path.transform);

            Vector3 start1 = transform.position - new Vector3(0, 0.6f, 0);
            Vector3 end1 = exit.position - new Vector3(0, 0.6f, 0);
            Vector3 start2 = transform.position - new Vector3(0, 0.5f, 0);
            Vector3 end2 = exit.position - new Vector3(0, 0.5f, 0);

            path.transform.GetComponent<PathAnimator>().from = transform;
            path.transform.GetComponent<PathAnimator>().destination = exit;

            path.transform.GetComponent<LineRenderer>().SetPosition(0, start1);
            path.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, start2);
            path.transform.GetComponent<LineRenderer>().SetPosition(1, end1);
            path.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, end2);
            path.transform.GetComponent<PathAnimator>().CreateCollision();
            path.transform.GetComponent<PathAnimator>().destination = exit;
            path.transform.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Idle);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NodeController : MonoBehaviour
{
    public List<Transform> exits = new List<Transform>();
    private List<Transform> paths = new List<Transform>();

    public int pointValue;
    public bool shown;

    public GameObject pathObject;


    void Start()
    {
        CreatePaths();
    }

    private void CreatePaths()
    {
        foreach(Transform exit in exits)
        {
            GameObject path = Instantiate(pathObject) as GameObject;
            path.transform.parent = transform;

            Vector3 start1 = transform.position - new Vector3(0, 0.2f, 0);
            Vector3 end1 = exit.position - new Vector3(0, 0.2f, 0);
            Vector3 start2 = transform.position - new Vector3(0, 0.1f, 0);
            Vector3 end2 = exit.position - new Vector3(0, 0.1f, 0);

            path.transform.GetComponent<LineRenderer>().SetPosition(0, start1);
            path.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, start2);
            path.transform.GetComponent<LineRenderer>().SetPosition(1, end1);
            path.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, end2);
            path.transform.GetComponent<PathAnimator>().CreateCollision();
            path.transform.GetComponent<PathAnimator>().SetState(PathAnimator.PathState.Idle);
        }
    }
}

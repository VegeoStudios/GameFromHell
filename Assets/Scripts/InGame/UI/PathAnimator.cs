using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimator : MonoBehaviour
{
    private float targetwidth;
    private float speed;

    public float[] widths;
    public float[] speeds;
    public Color[] colors;

    public Transform destination;
    public Transform from;

    public static List<Transform> paths;

    public enum PathState
    {
        Idle = 0,
        Selectable = 1,
        Blocked = 2,
        ActivePath = 3
    }

    public PathState state;

    public void SetState(PathState state)
    {
        LineRenderer lr = transform.GetChild(0).GetComponent<LineRenderer>();

        this.state = state;
        switch (this.state)
        {
            case PathState.Idle:
                targetwidth = widths[0];
                speed = speeds[0];
                SetColor(colors[0]);
                break;
            case PathState.Selectable:
                targetwidth = widths[1];
                speed = speeds[1];
                SetColor(colors[1]);
                break;
            case PathState.Blocked:
                targetwidth = widths[2];
                speed = speeds[2];
                SetColor(colors[2]);
                break;
            case PathState.ActivePath:
                targetwidth = widths[3];
                speed = speeds[3];
                SetColor(colors[3]);
                break;
        }
    }

    void Update()
    {
        if (state == PathState.Selectable) DetectTouch();
        Animate();
    }

    private void DetectTouch()
    {
        LineRenderer lr = transform.GetChild(0).GetComponent<LineRenderer>();

        foreach (Touch touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.transform == transform)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    BoardController.board.SelectPath(destination.GetComponent<NodeController>().data.id);
                }
            }
        }
    }

    public void CreateCollision()
    {
        LineRenderer lr = transform.GetChild(0).GetComponent<LineRenderer>();
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();

        capsule.radius = 0.6f;
        capsule.center = Vector3.zero;
        capsule.direction = 2;

        Vector3 start = transform.GetComponent<LineRenderer>().GetPosition(0);
        Vector3 end = transform.GetComponent<LineRenderer>().GetPosition(1);

        capsule.transform.position = start + (end - start) / 2;
        capsule.transform.LookAt(start);
        capsule.height = (end - start).magnitude;
    }

    private void Animate()
    {
        if (destination != null)
        {
            if (new Vector3[] { from.position, destination.position } != GetPositions())
            {
                SetPositions(from.position, destination.position);
            }
        } else if (Input.touchCount > 0)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.transform.position.y - CreationController.cc.backgroundPlane.position.y));
            if (CreationController.cc.addPathNode != null) pos = CreationController.cc.addPathNode.position;
            pos.y = 0;
            SetPositions(from.position, pos);
        }

        LineRenderer lr = transform.GetChild(0).GetComponent<LineRenderer>();
        Vector2 offset = lr.material.GetTextureOffset("_MainTex");

        offset.x -= speed;
        if (offset.x < 0) offset.x += 1;

        lr.material.SetTextureOffset("_MainTex", offset);

        lr.startWidth = Mathf.Lerp(lr.startWidth, targetwidth, 0.5f);
        lr.endWidth = Mathf.Lerp(lr.endWidth, targetwidth, 0.5f);
    }

    private void SetColor(Color color)
    {
        LineRenderer lr = transform.GetChild(0).GetComponent<LineRenderer>();
        lr.startColor = color;
        lr.endColor = color;
    }

    public void SetPositions(Vector3 p0, Vector3 p1)
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        p0.y--;
        p1.y--;
        lr.SetPositions(new Vector3[]{p0, p1});
        p0.y += 0.5f;
        p1.y += 0.5f;
        transform.GetChild(0).GetComponent<LineRenderer>().SetPositions(new Vector3[] { p0, p1 });
    }

    public Vector3[] GetPositions()
    {
        Vector3 p0 = GetComponent<LineRenderer>().GetPosition(0);
        p0.y++;
        Vector3 p1 = GetComponent<LineRenderer>().GetPosition(1);
        p1.y++;
        return new Vector3[] {p0, p1};
    }

    public void SetNodes(Transform node0, Transform node1)
    {
        from = node0;
        destination = node1;
    }

    public void Init()
    {
        if (paths == null) paths = new List<Transform>();
        paths.Add(transform);
    }
}

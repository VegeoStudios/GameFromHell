using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimator : MonoBehaviour
{
    private float targetwidth;
    private float speed;
    private float targetSpeed;

    public float[] widths;
    public float[] speeds;
    public Color[] colors;

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
                targetwidth = 0.5f;
                targetSpeed = 0.001f;
                SetColor(Color.white);
                break;
            case PathState.Selectable:
                targetwidth = 0.8f;
                targetSpeed = 0.01f;
                SetColor(Color.green);
                break;
            case PathState.Blocked:
                targetwidth = 0.3f;
                targetSpeed = 0.005f;
                SetColor(Color.red);
                break;
            case PathState.ActivePath:
                targetwidth = 0.5f;
                targetSpeed = 0.02f;
                SetColor(Color.yellow);
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
        LineRenderer lr = transform.GetChild(0).GetComponent<LineRenderer>();
        Vector2 offset = lr.material.GetTextureOffset("_MainTex");

        speed = Mathf.Lerp(speed, targetSpeed, 0.2f);
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
}

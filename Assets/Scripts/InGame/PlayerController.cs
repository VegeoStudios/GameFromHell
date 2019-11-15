using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData data;

    private Vector3 target;

    private float dist;
    private float truedist;
    private float angle;

    public bool stationary = true;

    private Vector3 velocity = Vector3.zero;

    public void Update()
    {
        Process();
        Animate();
    }

    public void Init(PlayerData pd)
    {
        data = pd;

        transform.GetComponent<MeshRenderer>().material.SetColor("color", pd.color);
        transform.GetComponent<MeshRenderer>().material.color = pd.color;

        transform.position = BoardController.GetNode(data.place).position;
        SetPosition(BoardController.GetNode(data.place));
    }

    public void SetPosition(Transform position)
    {
        data.place = position.GetComponent<NodeController>().data.id;
        angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
        dist = UnityEngine.Random.Range(0, 1.8f);
        truedist = dist;
        
    }

    private void Process()
    {
        if (Camera.main.GetComponent<CameraController>().target == BoardController.GetNode(data.place).GetChild(1))
        {
            truedist = 3;
        } else
        {
            truedist = dist;
        }

        target = BoardController.GetNode(data.place).position + new Vector3(Mathf.Cos(angle) * truedist, 0.3f, Mathf.Sin(angle) * truedist); ;

        
    }

    private void Animate()
    {
        if (Vector3.Distance(target, transform.position) > 0.01)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, 0.35f);
            stationary = false;
        }
        else if (target != transform.position)
        {
            transform.position = target;
            stationary = true;
        }
    }
}

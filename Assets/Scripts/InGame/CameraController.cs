using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 2);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target.eulerAngles, Time.deltaTime * 2);
    }
}

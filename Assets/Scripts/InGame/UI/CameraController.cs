using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public bool locked = false;

    private float rate = 0.2f;

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        
        transform.position = Vector3.Lerp(transform.position, target.position, rate);

        if (Vector3.Distance(transform.eulerAngles, target.eulerAngles) > 0.05)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target.eulerAngles, rate);
        } else if (transform.eulerAngles != target.eulerAngles)
        {
            transform.eulerAngles = target.eulerAngles;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Transform overHead;

    public bool locked = false;

    public float rate;
    private float[] rates = { 0.6f, 0.5f, 0.15f, 0.6f, 0.6f, 0.5f, 0.9f };

    private Vector3 vel0 = Vector3.zero, vel1 = Vector3.zero;

    public void LateUpdate()
    {
        Animate();
    }

    private void Animate()
    {
        
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref vel0, rate);
        transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, target.eulerAngles, ref vel1, rate);
    }

    public float[] GetRates()
    {
        return rates;
    }
}

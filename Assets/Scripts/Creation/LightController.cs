using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private float y;

    private void Start()
    {
        y = transform.position.y;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Camera.main.transform.position.x, y, Camera.main.transform.position.z), 0.5f);
    }
}

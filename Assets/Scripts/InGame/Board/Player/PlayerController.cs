using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName;
    public Color color;
    public Transform location;

    public void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    public void OnDrawGizmos()
    {
        gameObject.GetComponent<Renderer>().sharedMaterial.color = color;
    }

    public void UpdateLocation()
    {

    }
}

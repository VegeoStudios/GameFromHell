using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public void Test()
    {
        Debug.Log("dataPath: " + Application.dataPath);
        Debug.Log("streamingAssetsPath: " + Application.streamingAssetsPath);
        Debug.Log("persistentDataPath: " + Application.persistentDataPath);
    }
}

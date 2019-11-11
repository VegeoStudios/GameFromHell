using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class BoardController : MonoBehaviour
{
    public static BoardController board;

    public static BoardData boardData;
    public GameObject[] nodePrefabs;

    public string filepath;

    void Start()
    {
        boardData = JsonUtility.FromJson<BoardData>(File.ReadAllText(Application.dataPath + filepath));
        print(boardData.properties.name);
        Generate();
    }

    public void Generate()
    {
        board = this;
        foreach(NodeData node in boardData.nodes)
        {
            GameObject nodeObject;
            switch (node.type)
            {
                case "START":
                    nodeObject = Instantiate(nodePrefabs[0]);
                    break;
                case "DARE":
                    nodeObject = Instantiate(nodePrefabs[1]);
                    break;
                case "END":
                    nodeObject = Instantiate(nodePrefabs[2]);
                    break;
                default:
                    nodeObject = Instantiate(nodePrefabs[1]);
                    break;
            }
            nodeObject.transform.parent = transform;
            nodeObject.GetComponent<NodeController>().data = node;
            nodeObject.GetComponent<NodeController>().Init();
        }

        foreach(NodeController node in transform.GetComponentsInChildren<NodeController>())
        {
            node.CreatePaths();
        }
    }

    public Transform GetNode(int nodenum)
    {
        return transform.GetChild(nodenum);
    }
}
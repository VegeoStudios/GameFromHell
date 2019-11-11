using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BoardData
{
    public BoardProperties properties = new BoardProperties();
    public List<NodeData> nodes = new List<NodeData>();
}

[Serializable]
public class BoardProperties
{
    public string name = "";
    public int dice = 0;
    public bool hideNodes = false;
    public int startNodeID = 0;
    public int endNodeID = 0;
}

[Serializable]
public class NodeData
{
    public int id = 0;
    public List<int> pos = new List<int>();
    public string type = "";
    public List<int> exits = new List<int>();
    public string text = "";
    public int value = 0;
}
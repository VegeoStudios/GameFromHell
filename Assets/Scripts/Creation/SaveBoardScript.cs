using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveBoardScript : MonoBehaviour
{
    private BoardData data;

    public Transform board;

    public Transform boardName;

    private string json;

    public void SaveBoard()
    {
        if (boardName.GetComponent<TMPro.TextMeshProUGUI>().text == "")
        {
            return;
        }

        data = new BoardData();

        for (int i = 0; i < board.childCount; i++)
        {
            Transform node = board.GetChild(i);
            NodeData nodedata = new NodeData();
            nodedata.id = i;
            nodedata.pos.Add(node.position.x);
            nodedata.pos.Add(node.position.z);

            string typestring;

            switch (node.GetComponent<CreationNode>().type)
            {
                case CreationNode.NodeType.Start:
                    typestring = "START";
                    data.properties.startNodeID = node.GetSiblingIndex();
                    break;
                case CreationNode.NodeType.Dare:
                    typestring = "DARE";
                    nodedata.value = node.GetComponent<CreationNode>().value;
                    break;
                case CreationNode.NodeType.End:
                    typestring = "END";
                    break;
                default:
                    typestring = "DARE";
                    break;
            }

            nodedata.type = typestring;

            foreach (Transform exit in node.GetComponent<CreationNode>().exits)
            {
                nodedata.exits.Add(exit.GetSiblingIndex());
            }

            nodedata.text = node.GetChild(0).GetComponent<TMPro.TextMeshPro>().text;
            nodedata.value = node.GetComponent<CreationNode>().value;

            data.nodes.Add(nodedata);
        }

        data.properties.name = boardName.GetComponent<TMPro.TextMeshProUGUI>().text;
        data.properties.dice = 6;
        data.properties.hideNodes = false;

        json = JsonUtility.ToJson(data);

        Export();

        CreationController.cc.boardName.GetComponent<TMPro.TMP_InputField>().readOnly = true;
    }

    private void Export()
    {
        string datapath = Application.persistentDataPath;

        string path = datapath + "/BOARDS/CUSTOM/" + data.properties.name + ".json";
        File.WriteAllText(path, json);
        Debug.Log("Exported");

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif


    }
}

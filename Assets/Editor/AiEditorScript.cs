using UnityEngine;
using UnityEditor;
using System.Collections;

public class AiEditorScript : EditorWindow {

    /*
     This script is no longer being used. These were used when initializing the nodes when they were gameObjects.
     Since then, nodes have been changed to lighter weight scriptableObjects instead
     */ 

    [MenuItem("AI Menu / Build All")]
    private static void BuildAll()
    {
        GameObject NodeBuilderObj = GameObject.Find("NodeBuilder");

        NodeBuilderObj.GetComponent<NodeBuilder>().buildNodes();
        NodeBuilderObj.GetComponent<NodeBuilder>().getAllNodeConnections();
        CheckLevelGeometry();
    }

    [MenuItem("AI Menu / Build Nodes")]
    private static void BuildNodes()
    {
        GameObject NodeBuilderObj = GameObject.Find("NodeBuilder");
        NodeBuilderObj.GetComponent<NodeBuilder>().buildNodes();
    }

    [MenuItem("AI Menu / Check Level Geometry")]
    private static void CheckLevelGeometry()
    {
        GameObject[] levelGeometry = GameObject.FindGameObjectsWithTag("LevelGeometry");

        //move colliders up
        for (int i = 0; i < levelGeometry.Length; i++)
        {
            levelGeometry[i].GetComponent<BoxCollider>().size = new Vector3(1f, 0.75f, 1f);
            levelGeometry[i].GetComponent<BoxCollider>().center = new Vector3(0, 0.25f, 0);
        }

        GameObject NodeBuilderObj = GameObject.Find("NodeBuilder");
        NodeBuilderObj.GetComponent<NodeBuilder>().checkAllNodesForLevelGeometry();

        //move back into place
        for (int i = 0; i < levelGeometry.Length; i++)
        {
            levelGeometry[i].GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
            levelGeometry[i].GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
        }
    }

    [MenuItem("AI Menu / Build Node Connections")]
    private static void BuildNodeConnections()
    {
        GameObject NodeBuilderObj = GameObject.Find("NodeBuilder");
        NodeBuilderObj.GetComponent<NodeBuilder>().getAllNodeConnections();
    }

    [MenuItem("AI Menu / Erase Nodes")]
    private static void EraseNodes()
    {
        GameObject NodeBuilderObj = GameObject.Find("NodeBuilder");
        NodeBuilderObj.GetComponent<NodeBuilder>().eraseNodes();
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GraphSpawnerAbility : MonoBehaviour
{
    public static Dictionary<int, GameObject> nodesDict = new Dictionary<int, GameObject>{};
    public static Dictionary<int, List<GameObject>> connectedEdges = new Dictionary<int, List<GameObject>>{};
    private GameObject nodeInstance;
    private GameObject edgeInstance;
    public GameObject nodeObject;
    public GameObject edgeObject;

    private void Start()
    {
        int randint = (int)PhotonNetwork.CurrentRoom.CustomProperties["graph"];
        List<List<List<int>>> randomGraph = GraphCollections.graphCollections[randint];
        List<List<int>> x = randomGraph[0];
        List<List<int>> y = randomGraph[1];
        SpawnGraph(x, y);
    }

    private void SpawnGraph(List<List<int>> x, List<List<int>> y) 
    {
        Dictionary<int, int> nodeLayer = GetNodeLayer(x);
        Dictionary<int, int> totalNodeInLayer = GetTotalNodeInLayer(x);
        float horizontalSpace = 0;
        float verticalSpace = 0;
        float newXPos = 0;
        float newYPos = 0;
        int currentTotalLayer = 1;
        int currentNodeLayer = 0;
        float minScale = FindMinScale(x);
        for (var i = 0; i < y.Count; i++)
        {
            nodeInstance = UnityEngine.Object.Instantiate(nodeObject, new Vector3(0, 0, 0), Quaternion.identity);
            nodeInstance.transform.localScale = new Vector3(minScale, minScale, 1);
            nodeInstance.GetComponent<NodeAbility>().key = i;
            if (i == 0)
            {
                nodeInstance.GetComponent<NodeAbility>().state = 2;
                nodeInstance.GetComponent<Animator>().SetBool("Attacked", true);
            }

            int totalInlayer = totalNodeInLayer[i];
            int layer = nodeLayer[i];
            nodeInstance.GetComponent<NodeAbility>().layer = layer;
            if (currentNodeLayer != layer)
            {   
                horizontalSpace = 0;
                currentTotalLayer = totalInlayer;
                currentNodeLayer = layer;
                verticalSpace += 2 * minScale;
                if (currentTotalLayer >= 10)
                {
                    horizontalSpace = - 6 + (minScale / 2);
                }
                else
                {
                    horizontalSpace = - ((float)(currentTotalLayer + currentTotalLayer - 1) / 2) * minScale + (minScale / 2);
                }
            }
            newXPos = transform.position.x + horizontalSpace;
            newYPos = transform.position.y - verticalSpace;
            nodeInstance.transform.position = new Vector3(newXPos, newYPos , nodeInstance.transform.position.z);
            horizontalSpace += 2 * minScale;

            List<int> parentNodes = y[i];
            foreach (int parentKey in parentNodes)
            {
                GameObject parentObj = nodesDict[parentKey];
                CreateEdge(nodeInstance, parentObj);
                parentObj.GetComponent<NodeAbility>().childNodes.Add(i);
                nodeInstance.GetComponent<NodeAbility>().parentNodes.Add(parentKey);
            }
            nodesDict.Add(i, nodeInstance);
        }
        foreach (KeyValuePair<int, GameObject> entry in nodesDict)
        {

            if ((entry.Value).GetComponent<NodeAbility>().childNodes.Count == 0)
            {
                (entry.Value).GetComponent<Animator>().SetBool("IsSink", true);
                (entry.Value).GetComponent<NodeAbility>().lastLayer = true;
            }
        }
    }

    private Dictionary<int, int> GetNodeLayer(List<List<int>> nodePosition) {
        Dictionary<int, int> dict = new Dictionary<int, int>{};
        for (var i = 0; i < nodePosition.Count; i ++) 
        {
            foreach(int nodeKey in nodePosition[i]) 
            {
                dict.Add(nodeKey, i);
            }
        }
        return dict;
    }

    private Dictionary<int, int> GetTotalNodeInLayer(List<List<int>> nodePosition) {
        Dictionary<int, int> dict = new Dictionary<int, int>{};
        for (var i = 0; i < nodePosition.Count; i ++) 
        {
            foreach (int nodeKey in nodePosition[i]) 
            {
                dict.Add(nodeKey, nodePosition[i].Count);
            }
        }
        return dict;
    }

    private float FindMinScale(List<List<int>> nodePosition)
    {
        float xScale = 1;
        float yScale = 1;
        float minScale = 0;
        int maxNodeInLayer = FindMaxNodeInLayer(nodePosition);

        if (maxNodeInLayer >= 6)
        {
            xScale = CalculateXScale(maxNodeInLayer);
        }
        if (nodePosition.Count >= 4)
        {
            yScale = CalculateYScale(nodePosition.Count);
        }
        minScale = Math.Min(xScale, yScale);

        return minScale;
    }

    private int FindMaxNodeInLayer(List<List<int>> nodePosition)
    {
        int maxNodeInLayer = 0;
        for (var i = 0; i < nodePosition.Count; i ++) 
        {
            int currentTotal = nodePosition[i].Count;
            if(currentTotal > maxNodeInLayer)
            {
                maxNodeInLayer = currentTotal;
            }
        }
        return maxNodeInLayer;
    }

    private float CalculateXScale(int maxNodeInLayer)
    {
        double xScale = 6.38327 / maxNodeInLayer;
        return (float)xScale;
    }

    private float CalculateYScale(int totalLayer) 
    {
        double yScale = 3.88952 / totalLayer;
        return (float)yScale;
    }

    private void CreateEdge(GameObject childNode, GameObject parentNode)
    {
        Vector3 midpoint = CalculateEdgePosition(childNode, parentNode);
        Quaternion rotation = CalculateEdgeRotation(childNode, parentNode);
        Vector3 scale = CalculateEdgeScale(nodeInstance, parentNode);

        edgeInstance = UnityEngine.Object.Instantiate(edgeObject, new Vector3(0, 0, 0), Quaternion.identity);
        edgeInstance.transform.position = midpoint;
        edgeInstance.transform.rotation = rotation;
        edgeInstance.transform.localScale = scale;

        int childKey = childNode.GetComponent<Node>().key;
        int parentKey = parentNode.GetComponent<Node>().key;
        if (connectedEdges.ContainsKey(childKey)) {
            List<GameObject> edgeList = connectedEdges[childKey];
            edgeList.Add(edgeInstance);
            connectedEdges[childKey] = edgeList;
        }
        else {connectedEdges[childKey] = new List<GameObject>{edgeInstance};}
        if (connectedEdges.ContainsKey(parentKey)) {
            List<GameObject> edgeList = connectedEdges[parentKey];
            edgeList.Add(edgeInstance);
            connectedEdges[parentKey] = edgeList;
        }
        else {connectedEdges[parentKey] = new List<GameObject>{edgeInstance};}
    }

    private Vector3 CalculateEdgePosition(GameObject currentObj, GameObject targetObj)
    {
        float midX = (currentObj.transform.position.x + targetObj.transform.position.x) / 2;
        float midY = (currentObj.transform.position.y + targetObj.transform.position.y) / 2;
        return new Vector3(midX, midY, currentObj.transform.position.z);
    }

    private Quaternion CalculateEdgeRotation(GameObject currentObj, GameObject targetObj)
    {
        float firstx = currentObj.transform.position.x;
        float secondx = targetObj.transform.position.x;
        float firsty = currentObj.transform.position.y;
        float secondy = targetObj.transform.position.y;

        double angle = Math.Atan((secondy-firsty) / (secondx-firstx));
        angle = angle * (180/Math.PI);

        return Quaternion.Euler(0, 0, (float)angle);;
    }

    private Vector3 CalculateEdgeScale(GameObject currentObj, GameObject targetObj)
    {
        float firstx = currentObj.transform.position.x;
        float secondx = targetObj.transform.position.x;
        float firsty = currentObj.transform.position.y;
        float secondy = targetObj.transform.position.y;

        double distance = Math.Sqrt(Math.Pow(secondy-firsty, 2) + Math.Pow(secondx-firstx, 2));

        return new Vector3((float)distance, (float)0.05, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GraphSpawnerMulti : MonoBehaviour
{
    [SerializeField]
    private GameObject nodeReference;
    [SerializeField]
    private GameObject edgeReference;
    private GameObject nodeInstance;
    private GameObject edgeInstance;
    public static Dictionary<int, int> nodesDict = new Dictionary<int, int>{};

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            System.Random rand = new System.Random();
            int randint = rand.Next(0, GraphCollections.graphCollections.Count);
            List<List<List<int>>> randomGraph = GraphCollections.graphCollections[5];
            List<List<int>> x = randomGraph[0];
            List<List<int>> y = randomGraph[1];
            SpawnGraph(x, y);
        }
    }

    public void SpawnGraph(List<List<int>> x, List<List<int>> y) 
    {
        PhotonView photonView = GetComponent<PhotonView>();
        Dictionary<int, int> nodeLayer = getNodeLayer(x);
        Dictionary<int, int> totalNodeInLayer = getTotalNodeInLayer(x);
        float horizontalSpace = 0;
        float verticalSpace = 0;
        float newXPos = 0;
        float newYPos = 0;
        int currentTotalLayer = 1;
        int currentNodeLayer = 0;
        float minScale = findMinScale(x);
        for (var i = 0; i < y.Count; i++)
        {
            nodeInstance = PhotonNetwork.Instantiate("Node", new Vector3(0, 0, 0), Quaternion.identity);
            nodeInstance.transform.localScale = new Vector3(minScale, minScale, 1);
            int viewID = nodeInstance.GetComponent<PhotonView>().ViewID;
            photonView.RPC("UpdateNodeKey", RpcTarget.AllBuffered, viewID, i);
            if (i == 0)
            {
                photonView.RPC("UpdateNodeState", RpcTarget.AllBuffered, viewID, 2);
                photonView.RPC("UpdateNodeColorRed", RpcTarget.AllBuffered, viewID);
            }

            int totalInlayer = totalNodeInLayer[i];
            int layer = nodeLayer[i];
            if (currentNodeLayer != layer)
            {   
                horizontalSpace = 0;
                currentTotalLayer = totalInlayer;
                currentNodeLayer = layer;
                verticalSpace += 2 * minScale;
                if (currentTotalLayer >= 10)
                {
                    horizontalSpace = - 8 + (minScale / 2);
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
                int parentID = nodesDict[parentKey];
                GameObject parentObj = PhotonView.Find(parentID).gameObject;
                createEdge(nodeInstance, parentObj);
                photonView.RPC("UpdateNodeChildNodes", RpcTarget.AllBuffered, parentID, i);
                photonView.RPC("UpdateNodeParentNodes", RpcTarget.AllBuffered, viewID, parentKey);
            }
            photonView.RPC("UpdateNodesDict", RpcTarget.AllBuffered, viewID, i);
        }
        foreach (KeyValuePair<int, int> entry in nodesDict)
        {

            if (PhotonView.Find(entry.Value).gameObject.GetComponent<Node>().childNodes.Count == 0)
            {
                photonView.RPC("UpdateNodeLastLayer", RpcTarget.AllBuffered, entry.Value);
            }
        }
    }

    private Dictionary<int, int> getNodeLayer(List<List<int>> nodePosition) {
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

    private Dictionary<int, int> getTotalNodeInLayer(List<List<int>> nodePosition) {
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

    private float findMinScale(List<List<int>> nodePosition)
    {
        float xScale = 1;
        float yScale = 1;
        float minScale = 0;
        int maxNodeInLayer = findMaxNodeInLayer(nodePosition);

        if (maxNodeInLayer >= 10)
        {
            xScale = calculateXScale(maxNodeInLayer);
        }
        if (nodePosition.Count >= 5)
        {
            yScale = calculateYScale(nodePosition.Count);
        }
        minScale = Math.Min(xScale, yScale);

        return minScale;
    }

    private int findMaxNodeInLayer(List<List<int>> nodePosition)
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

    private float calculateXScale(int maxNodeInLayer)
    {
        double xScale = 8.36376 / maxNodeInLayer;
        return (float)xScale;
    }

    private float calculateYScale(int totalLayer) 
    {
        double yScale = 4.34347 / totalLayer;
        return (float)yScale;
    }

    private void createEdge(GameObject childNode, GameObject parentNode)
    {
        Vector3 midpoint = calculateEdgePosition(childNode, parentNode);
        Quaternion rotation = calculateEdgeRotation(childNode, parentNode);
        Vector3 scale = calculateEdgeScale(nodeInstance, parentNode);

        edgeInstance = PhotonNetwork.Instantiate("Edge", new Vector3(0, 0, 0), Quaternion.identity);
        edgeInstance.transform.position = midpoint;
        edgeInstance.transform.rotation = rotation;
        edgeInstance.transform.localScale = scale;
    }

    private Vector3 calculateEdgePosition(GameObject currentObj, GameObject targetObj)
    {
        float midX = (currentObj.transform.position.x + targetObj.transform.position.x) / 2;
        float midY = (currentObj.transform.position.y + targetObj.transform.position.y) / 2;
        return new Vector3(midX, midY, currentObj.transform.position.z);
    }

    private Quaternion calculateEdgeRotation(GameObject currentObj, GameObject targetObj)
    {
        float firstx = currentObj.transform.position.x;
        float secondx = targetObj.transform.position.x;
        float firsty = currentObj.transform.position.y;
        float secondy = targetObj.transform.position.y;

        double angle = Math.Atan((secondy-firsty) / (secondx-firstx));
        angle = angle * (180/Math.PI);

        return Quaternion.Euler(0, 0, (float)angle);;
    }

    private Vector3 calculateEdgeScale(GameObject currentObj, GameObject targetObj)
    {
        float firstx = currentObj.transform.position.x;
        float secondx = targetObj.transform.position.x;
        float firsty = currentObj.transform.position.y;
        float secondy = targetObj.transform.position.y;

        double distance = Math.Sqrt(Math.Pow(secondy-firsty, 2) + Math.Pow(secondx-firstx, 2));

        return new Vector3((float)distance, (float)0.05, 0);
    }

    [PunRPC]
    public void UpdateNodeKey(int viewID, int key)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Node>().key = key;
    }

     [PunRPC]
    public void UpdateNodeState(int viewID, int state)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Node>().state = state;
    }

    [PunRPC]
    public void UpdateNodeColorRed(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.red;
    }

    [PunRPC]
    public void UpdateNodeChildNodes(int parentID, int key)
    {
        GameObject node = PhotonView.Find(parentID).gameObject;
        node.GetComponent<Node>().childNodes.Add(key);
    }
    
    [PunRPC]
    public void UpdateNodeParentNodes(int viewID, int parentKey)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Node>().parentNodes.Add(parentKey);
    }

    [PunRPC]
    public void UpdateNodesDict(int viewID, int key)
    {
        GraphSpawnerMulti.nodesDict.Add(key, viewID);
    }

    [PunRPC]
    public void UpdateNodeLastLayer(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Node>().lastLayer = true;
    }
}

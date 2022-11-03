using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GraphSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject nodeReference;

    [SerializeField]
    private GameObject edgeReference;

    private GameObject edgeInstance;
    private GameObject nodeInstance;
    public static Dictionary<int, GameObject> nodesDict;

    // Start is called before the first frame update
    void Start()
    {
        List<List<int>> x = new List<List<int>>{
          new List<int> {0},
          new List<int> {1, 2},
          new List<int> {3, 4,},
          new List<int> {5, 6, 7, 8, },
        };

        List<List<int>> y = new List<List<int>>{
          new List<int> {},
          new List<int> {0},
          new List<int> {0},
          new List<int> {1},
          new List<int> {2},
          new List<int> {3},
          new List<int> {3},
          new List<int> {4},
          new List<int> {4},
        };
        StartCoroutine(SpawnGraph(x, y));
    }

    IEnumerator SpawnGraph(List<List<int>> x, List<List<int>> y) 
    {
        yield return new WaitForSeconds(0.5f);
        Dictionary<int, int> nodeLayer = GetNodeLayer(x);
        Dictionary<int, int> totalNodeInLayer = GetTotalNodeInLayer(x);
        nodesDict = new Dictionary<int, GameObject>{};
        int lastLayer = x.Count - 1;
        int horizontalSpace = 0;
        float calculatedHorizontalSpace = 0;
        float calculatedVerticalSpace = 0;
        float newXPos = 0;
        float newYPos = 0;
        int currentTotalLayer = 1;
        int currentNodeLayer = 1;
        for (var i = 0; i < y.Count; i++)
        {
            nodeInstance = Instantiate(nodeReference);
            nodeInstance.GetComponent<Node>().setKey(i);
            if (i == 0)
            {
                nodeInstance.GetComponent<Node>().setState(3);
                nodeInstance.GetComponent<Renderer>().material.color = Color.red;
            }

            int totalInlayer = totalNodeInLayer[i];
            int layer = nodeLayer[i];

            if (currentNodeLayer != layer)
            {   
                currentTotalLayer = totalInlayer;
                currentNodeLayer = layer;
                calculatedVerticalSpace += 1;
                calculatedHorizontalSpace = (currentTotalLayer + currentTotalLayer - 1) / 2;
                horizontalSpace = 0;
            }
            newXPos = transform.position.x - calculatedHorizontalSpace + horizontalSpace;
            newYPos = transform.position.y - nodeLayer[i] - calculatedVerticalSpace;
            horizontalSpace += 2;
            nodeInstance.transform.position = new Vector3(newXPos, newYPos , nodeInstance.transform.position.z);

            List<int> parentNodes = y[i];
            nodeInstance.GetComponent<Node>().setParentNodes(parentNodes);
            foreach (int parentKey in parentNodes)
            {
                GameObject parentObj = nodesDict[parentKey];
                parentObj.GetComponent<Node>().childNodes.Add(i);
                float midX = (newXPos + parentObj.transform.position.x) / 2;
                float midY = (newYPos + parentObj.transform.position.y) / 2;
                float distance = CalculateDistance(nodeInstance, parentObj);
                float angle = CalculateAngle(nodeInstance, parentObj);

                Vector3 midpoint = new Vector3(midX, midY, nodeInstance.transform.position.z);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 scale = new Vector3(distance, (float)0.1, 0);

                edgeInstance = Instantiate(edgeReference);
                edgeInstance.transform.position = midpoint;
                edgeInstance.transform.rotation = rotation;
                edgeInstance.transform.localScale = scale;
            }
            nodesDict.Add(i, nodeInstance);
        }
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawner.nodesDict)
        {
            if (entry.Value.GetComponent<Node>().childNodes.Count == 0)
            {
                entry.Value.GetComponent<Node>().lastLayer = true;
            }
        }
    }

    public Dictionary<int, int> GetNodeLayer(List<List<int>> nodePosition) {
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

    public Dictionary<int, int> GetTotalNodeInLayer(List<List<int>> nodePosition) {
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

    public float CalculateDistance(GameObject currentObj, GameObject targetObj)
    {
        float firstx = currentObj.transform.position.x;
        float secondx = targetObj.transform.position.x;
        float firsty = currentObj.transform.position.y;
        float secondy = targetObj.transform.position.y;

        double distance = Math.Sqrt(Math.Pow(secondy-firsty, 2) + Math.Pow(secondx-firstx, 2));

        return (float)distance;
    }

    public float CalculateAngle(GameObject currentObj, GameObject targetObj)
    {
        float firstx = currentObj.transform.position.x;
        float secondx = targetObj.transform.position.x;
        float firsty = currentObj.transform.position.y;
        float secondy = targetObj.transform.position.y;

        double angle = Math.Atan((secondy-firsty) / (secondx-firstx));
        angle = angle * (180/Math.PI);

        return (float)angle;
    }
}

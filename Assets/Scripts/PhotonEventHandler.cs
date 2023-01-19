using System;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class PhotonEventHandler : MonoBehaviour, IOnEventCallback
{
    public GameObject edgeObject;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 0)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animBool = (string)  contents[1];
            int state = (int) contents[2];
            int currentPlayer = (int) contents[3];
            GameObject node = GraphSpawnerMulti.nodesDict[key];

            node.GetComponent<Animator>().SetBool(animBool, true);
            node.GetComponent<Node>().state = state;
            GameController.currentPlayer = currentPlayer;
        }

        else if (photonEvent.Code == 1)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            string tag = (string) contents[0];
            string text = (string)  contents[1];

            GameObject textObject = GameObject.FindWithTag(tag);
            textObject.GetComponent<Text>().text = text;
            GameController.gameOver = true;
            GameController.matchStart = false;
        }

        else if (photonEvent.Code == 2)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animBool = (string)  contents[1];
            int state = (int) contents[2];
            int currentPlayer = (int) contents[3];
            GameObject node = GraphSpawnerAbility.nodesDict[key];

            node.GetComponent<Animator>().SetBool(animBool, true);
            node.GetComponent<NodeAbility>().state = state;
            GameControllerAbility.currentPlayer = currentPlayer;
        }

        else if (photonEvent.Code == 3)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            string tag = (string) contents[0];
            string text = (string)  contents[1];

            GameObject textObject = GameObject.FindWithTag(tag);
            textObject.GetComponent<Text>().text = text;
            GameControllerAbility.gameOver = true;
            GameControllerAbility.matchStart = false;
        }

        else if (photonEvent.Code == 4)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animTag = (string)  contents[1];
            bool animBool = (bool) contents[2];
            int state = (int) contents[3];
            GameObject node = GraphSpawnerAbility.nodesDict[key];

            node.GetComponent<Animator>().SetBool(animTag, animBool);
            node.GetComponent<NodeAbility>().state = state;
        }

        else if (photonEvent.Code == 5)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int firstKey = (int) contents[0];
            int secondKey = (int) contents[1];
            GameObject firstNodeObj = GraphSpawnerAbility.nodesDict[firstKey];
            GameObject secondNodeObj = GraphSpawnerAbility.nodesDict[secondKey];
            firstNodeObj.GetComponent<NodeAbility>().childNodes.Add(secondKey);
            secondNodeObj.GetComponent<NodeAbility>().parentNodes.Add(firstKey);
            CreateEdge(firstNodeObj, secondNodeObj);
        }
    }

    private void CreateEdge(GameObject childNode, GameObject parentNode)
    {
        Vector3 midpoint = CalculateEdgePosition(childNode, parentNode);
        Quaternion rotation = CalculateEdgeRotation(childNode, parentNode);
        Vector3 scale = CalculateEdgeScale(childNode, parentNode);

        GameObject edgeInstance = UnityEngine.Object.Instantiate(edgeObject, new Vector3(0, 0, 0), Quaternion.identity);
        edgeInstance.transform.position = midpoint;
        edgeInstance.transform.rotation = rotation;
        edgeInstance.transform.localScale = scale;
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
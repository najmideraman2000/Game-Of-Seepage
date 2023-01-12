using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class NodeAbility : MonoBehaviour
{
    public int key;
    // state
    // 0: untouched
    // 1: defended
    // 2: attacked
    public int state;
    public List<int> parentNodes;
    public List<int> childNodes;
    public bool lastLayer;

    public void OnMouseDown() {
        Debug.Log("HEY");
        PhotonView photonView = GetComponent<PhotonView>();
        int viewID = photonView.ViewID;
        if (!GameControllerAbility.gameOver && GameControllerAbility.player == 0 && GameControllerAbility.currentPlayer == 0) // if defender
        {
            if (GameControllerAbility.abilityChoosed)
            {
                // freeze node
                photonView.RPC("UpdateNodeColorBlue", RpcTarget.All, viewID);
                photonView.RPC("UpdateNodeState", RpcTarget.All, viewID, 3);
                GameControllerAbility.abilityChoosed = false;
            }
            else
            {
                if (state == 0 && state != 3)
                {   
                    photonView.RPC("UpdateNodeColorGreen", RpcTarget.All, viewID);
                    photonView.RPC("UpdateNodeState", RpcTarget.All, viewID, 1);
                    photonView.RPC("UpdateCurrentPlayer", RpcTarget.All, 1);
                    if (attackerLose())
                    {
                        photonView.RPC("UpdateText", RpcTarget.All, "DEFENDER WIN");
                        photonView.RPC("UpdateGameOver", RpcTarget.All);
                    }
                    else {
                        photonView.RPC("UpdateText", RpcTarget.All, "ATTACKER'S TURN");
                    }
                }
            }
        }
        else if (!GameControllerAbility.gameOver && GameControllerAbility.player == 1 && GameControllerAbility.currentPlayer == 1) // if attacker
        {
            if (GameControllerAbility.abilityChoosed)
            {
                Debug.Log("TW1");
                if (GameControllerAbility.firstNodeChoosed)
                {
                    // create edge
                    Debug.Log("LOL");
                    int firstNodeID = GraphSpawnerAbility.nodesDict[GameControllerAbility.firstKey];
                    int secondNodeID = GraphSpawnerAbility.nodesDict[key];
                    GameObject firstNodeObj = PhotonView.Find(firstNodeID).gameObject;
                    GameObject secondNodeObj = PhotonView.Find(secondNodeID).gameObject;
                    createEdge(firstNodeObj, secondNodeObj);
                    photonView.RPC("UpdateNodeChildNodes", RpcTarget.AllBuffered, firstNodeID, key);
                    photonView.RPC("UpdateNodeParentNodes", RpcTarget.AllBuffered, secondNodeID, GameControllerAbility.firstKey);
                    GameControllerAbility.abilityChoosed = false;
                    GameControllerAbility.firstNodeChoosed = false;

                }
                else
                {
                    Debug.Log("LMAO");
                    GameControllerAbility.firstNodeChoosed = true;
                    GameControllerAbility.firstKey = key;
                }
            }
            else
            {
                Debug.Log("HMMMM");
                if (state == 0)
                {
                    foreach (int parentKey in parentNodes)
                    {
                        if (PhotonView.Find(GraphSpawnerAbility.nodesDict[parentKey]).gameObject.GetComponent<NodeAbility>().state == 2)
                        {
                            photonView.RPC("UpdateNodeColorRed", RpcTarget.All, viewID);
                            photonView.RPC("UpdateNodeState", RpcTarget.All, viewID, 2);
                            photonView.RPC("UpdateCurrentPlayer", RpcTarget.All, 0);
                            if (lastLayer) {
                                photonView.RPC("UpdateText", RpcTarget.All, "ATTACKER WIN");
                                photonView.RPC("UpdateGameOver", RpcTarget.All);
                                break;
                            }
                            photonView.RPC("UpdateText", RpcTarget.All, "DEFENDER'S TURN");
                            checkNodeFrozen();
                            break;
                        }
                    }
                    
                }
            }
        }
    }

    public bool attackerLose()
    {
        foreach(KeyValuePair<int, int> entry in GraphSpawnerAbility.nodesDict)
        {
            if (PhotonView.Find(entry.Value).gameObject.GetComponent<NodeAbility>().state == 0)
            {
                foreach(int parentKey in PhotonView.Find(entry.Value).gameObject.GetComponent<NodeAbility>().parentNodes)
                {
                    if (PhotonView.Find(GraphSpawnerAbility.nodesDict[parentKey]).gameObject.GetComponent<NodeAbility>().state == 2)
                    {
                        if(hasPathToWin(entry.Value))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public bool hasPathToWin(int viewID)
    {
        if (PhotonView.Find(viewID).gameObject.GetComponent<NodeAbility>().lastLayer)
        {
            return true;
        }
        foreach(int childKey in PhotonView.Find(viewID).gameObject.GetComponent<NodeAbility>().childNodes)
        {
            if (PhotonView.Find(GraphSpawnerAbility.nodesDict[childKey]).gameObject.GetComponent<NodeAbility>().state == 0)
            {
                if (hasPathToWin(GraphSpawnerAbility.nodesDict[childKey]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void createEdge(GameObject childNode, GameObject parentNode)
    {
        Vector3 midpoint = calculateEdgePosition(childNode, parentNode);
        Quaternion rotation = calculateEdgeRotation(childNode, parentNode);
        Vector3 scale = calculateEdgeScale(childNode, parentNode);

        GameObject edgeInstance = PhotonNetwork.Instantiate("Edge", new Vector3(0, 0, 0), Quaternion.identity);
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

    public void checkNodeFrozen()
    {
        foreach(KeyValuePair<int, int> entry in GraphSpawnerAbility.nodesDict)
        {
            if (PhotonView.Find(entry.Value).gameObject.GetComponent<NodeAbility>().state == 3)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("UpdateNodeState", RpcTarget.All, entry.Value, 0);
                photonView.RPC("UpdateNodeColorWhite", RpcTarget.All, entry.Value);
            }
        }
    }

    [PunRPC]
    public void UpdateNodeColorGreen(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.green;
    }

    [PunRPC]
    public void UpdateNodeState(int viewID, int state)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<NodeAbility>().state = state;
    }

    [PunRPC]
    public void UpdateCurrentPlayer(int currentPlayer)
    {
        GameControllerAbility.currentPlayer = currentPlayer;
    }

    [PunRPC]
    public void UpdateText(string text)
    {
        GameObject textObject = GameObject.FindWithTag("Text");
        textObject.GetComponent<Text>().text = text;
    }

    [PunRPC]
    public void UpdateGameOver()
    {
        GameControllerAbility.gameOver = true;
    }

    [PunRPC]
    public void UpdateNodeColorRed(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.red;
    }

    [PunRPC]
    public void UpdateNodeColorBlue(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.blue;
    }

    [PunRPC]
    public void UpdateNodeColorWhite(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.white;
    }

    [PunRPC]
    public void UpdateNodeChildNodes(int parentID, int key)
    {
        GameObject node = PhotonView.Find(parentID).gameObject;
        node.GetComponent<NodeAbility>().childNodes.Add(key);
    }
    
    [PunRPC]
    public void UpdateNodeParentNodes(int viewID, int parentKey)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<NodeAbility>().parentNodes.Add(parentKey);
    }
}

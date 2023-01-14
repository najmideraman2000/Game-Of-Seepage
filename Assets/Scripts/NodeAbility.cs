using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NodeAbility : MonoBehaviour
{
    public int key;
    // state
    // 0: untouched
    // 1: defended
    // 2: attacked
    // 4: frozen
    public int state;
    public List<int> parentNodes;
    public List<int> childNodes;
    public bool lastLayer;

    private void OnMouseDown() {
        PhotonView photonView = GetComponent<PhotonView>();
        int viewID = photonView.ViewID;
        if (!GameControllerAbility.gameOver && GameControllerAbility.player == 0 && GameControllerAbility.currentPlayer == 0) // if defender
        {
            if (GameControllerAbility.abilityChoosed && !GameControllerAbility.abilityDone)
            {
                photonView.RPC("UpdateNodeColorBlue", RpcTarget.All, viewID);
                photonView.RPC("UpdateNodeState", RpcTarget.All, viewID, 3);
                GameControllerAbility.abilityChoosed = false;
                GameControllerAbility.abilityDone = true;
            }
            else
            {
                if (state == 0 && state != 3)
                {   
                    photonView.RPC("UpdateNodeColorGreen", RpcTarget.All, viewID);
                    photonView.RPC("UpdateNodeState", RpcTarget.All, viewID, 1);
                    photonView.RPC("UpdateCurrentPlayer", RpcTarget.All, 1);
                    if (AttackerLose())
                    {
                        photonView.RPC("UpdateText", RpcTarget.All, "Text", "DEFENDER WIN");
                        photonView.RPC("UpdateGameOver", RpcTarget.All, true);
                        photonView.RPC("UpdateMatchStart", RpcTarget.All, false);
                        GameControllerAbility.winGame = true;
                    }
                }
            }
        }
        else if (!GameControllerAbility.gameOver && GameControllerAbility.player == 1 && GameControllerAbility.currentPlayer == 1) // if attacker
        {
            if (GameControllerAbility.abilityChoosed && !GameControllerAbility.abilityDone)
            {
                if (GameControllerAbility.firstNodeChoosed)
                {
                    int firstNodeID = GraphSpawnerAbility.nodesDict[GameControllerAbility.firstKey];
                    int secondNodeID = GraphSpawnerAbility.nodesDict[key];
                    GameObject firstNodeObj = PhotonView.Find(firstNodeID).gameObject;
                    GameObject secondNodeObj = PhotonView.Find(secondNodeID).gameObject;
                    CreateEdge(firstNodeObj, secondNodeObj);
                    photonView.RPC("UpdateNodeChildNodes", RpcTarget.AllBuffered, firstNodeID, key);
                    photonView.RPC("UpdateNodeParentNodes", RpcTarget.AllBuffered, secondNodeID, GameControllerAbility.firstKey);
                    GameControllerAbility.abilityChoosed = false;
                    GameControllerAbility.firstNodeChoosed = false;
                    GameControllerAbility.abilityDone = true;

                }
                else
                {
                    GameControllerAbility.firstNodeChoosed = true;
                    GameControllerAbility.firstKey = key;
                }
            }
            else
            {
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
                                photonView.RPC("UpdateText", RpcTarget.All, "Text", "ATTACKER WIN");
                                photonView.RPC("UpdateGameOver", RpcTarget.All, true);
                                photonView.RPC("UpdateMatchStart", RpcTarget.All, false);
                                GameControllerAbility.winGame = true;
                            }
                            CheckNodeFrozen();
                            break;
                        }
                    }
                    
                }
            }
        }
    }

    private bool AttackerLose()
    {
        foreach(KeyValuePair<int, int> entry in GraphSpawnerAbility.nodesDict)
        {
            if (PhotonView.Find(entry.Value).gameObject.GetComponent<NodeAbility>().state == 0)
            {
                foreach(int parentKey in PhotonView.Find(entry.Value).gameObject.GetComponent<NodeAbility>().parentNodes)
                {
                    if (PhotonView.Find(GraphSpawnerAbility.nodesDict[parentKey]).gameObject.GetComponent<NodeAbility>().state == 2)
                    {
                        if(HasPathToWin(entry.Value))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private bool HasPathToWin(int viewID)
    {
        if (PhotonView.Find(viewID).gameObject.GetComponent<NodeAbility>().lastLayer)
        {
            return true;
        }
        foreach(int childKey in PhotonView.Find(viewID).gameObject.GetComponent<NodeAbility>().childNodes)
        {
            if (PhotonView.Find(GraphSpawnerAbility.nodesDict[childKey]).gameObject.GetComponent<NodeAbility>().state == 0)
            {
                if (HasPathToWin(GraphSpawnerAbility.nodesDict[childKey]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void CreateEdge(GameObject childNode, GameObject parentNode)
    {
        Vector3 midpoint = CalculateEdgePosition(childNode, parentNode);
        Quaternion rotation = CalculateEdgeRotation(childNode, parentNode);
        Vector3 scale = CalculateEdgeScale(childNode, parentNode);

        GameObject edgeInstance = PhotonNetwork.Instantiate("Edge", new Vector3(0, 0, 0), Quaternion.identity);
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

    private void CheckNodeFrozen()
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
    private void UpdateNodeColorGreen(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.green;
    }

    [PunRPC]
    private void UpdateNodeColorRed(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.red;
    }

    [PunRPC]
    private void UpdateNodeColorBlue(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.blue;
    }

    [PunRPC]
    private void UpdateNodeColorWhite(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.white;
    }

    [PunRPC]
    private void UpdateNodeState(int viewID, int state)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<NodeAbility>().state = state;
    }

    [PunRPC]
    private void UpdateNodeChildNodes(int parentID, int key)
    {
        GameObject node = PhotonView.Find(parentID).gameObject;
        node.GetComponent<NodeAbility>().childNodes.Add(key);
    }
    
    [PunRPC]
    private void UpdateNodeParentNodes(int viewID, int parentKey)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<NodeAbility>().parentNodes.Add(parentKey);
    }

    [PunRPC]
    private void UpdateCurrentPlayer(int currentPlayer)
    {
        GameControllerAbility.currentPlayer = currentPlayer;
    }

    [PunRPC]
    private void UpdateText(string tag, string text)
    {
        Debug.Log(tag);
        GameObject textObject = GameObject.FindWithTag(tag);
        textObject.GetComponent<Text>().text = text;
    }

    [PunRPC]
    private void UpdateGameOver(bool state)
    {
        GameControllerAbility.gameOver = state;
    }

    [PunRPC]
    private void UpdateMatchStart(bool state)
    {
        GameControllerAbility.matchStart = state;
    }
}

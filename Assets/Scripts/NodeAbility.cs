using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NodeAbility : MonoBehaviour
{
    public int key;
    // state
    // 0: untouched
    // 1: defended
    // 2: attacked
    // 3: frozen
    public int state;
    public List<int> parentNodes;
    public List<int> childNodes;
    public bool lastLayer;
    public GameObject edgeObject;
    private RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    private object[] contents;

    private void OnMouseDown() {
        if (!GameControllerAbility.gameOver && GameControllerAbility.player == 0 && GameControllerAbility.currentPlayer == 0) // if defender
        {
            if (GameControllerAbility.abilityChoosed && !GameControllerAbility.abilityDone)
            {
                contents = new object[]{key, "Frozen", true, 1};
                PhotonNetwork.RaiseEvent(4, contents, raiseEventOptions, SendOptions.SendReliable);
                GameControllerAbility.abilityChoosed = false;
                GameControllerAbility.abilityDone = true;
            }
            else
            {
                if (state == 0 && state != 3)
                {   
                    contents = new object[]{key, "Defended", 1, 1};
                    PhotonNetwork.RaiseEvent(2, contents, raiseEventOptions, SendOptions.SendReliable);
                    if (AttackerLose())
                    {
                        contents = new object[]{"Text", "DEFENDER WIN"};
                        PhotonNetwork.RaiseEvent(3, contents, raiseEventOptions, SendOptions.SendReliable);
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
                    GameObject firstNodeObj = GraphSpawnerAbility.nodesDict[GameControllerAbility.firstKey];
                    GameObject secondNodeObj = GraphSpawnerAbility.nodesDict[key];
                    CreateEdge(firstNodeObj, secondNodeObj);
                    contents = new object[]{GameControllerAbility.firstKey, key};
                    PhotonNetwork.RaiseEvent(5, contents, raiseEventOptions, SendOptions.SendReliable);
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
                        if (GraphSpawnerAbility.nodesDict[parentKey].GetComponent<NodeAbility>().state == 2)
                        {
                            contents = new object[]{key, "Attacked", 2, 0};
                            PhotonNetwork.RaiseEvent(2, contents, raiseEventOptions, SendOptions.SendReliable);
                            if (lastLayer) {
                                contents = new object[]{"Text", "ATTACKER WIN"};
                                PhotonNetwork.RaiseEvent(3, contents, raiseEventOptions, SendOptions.SendReliable);
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
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawnerAbility.nodesDict)
        {
            if ((entry.Value).GetComponent<NodeAbility>().state == 0)
            {
                foreach(int parentKey in (entry.Value).gameObject.GetComponent<NodeAbility>().parentNodes)
                {
                    if (GraphSpawnerAbility.nodesDict[parentKey].GetComponent<NodeAbility>().state == 2)
                    {
                        if(HasPathToWin(entry.Key)) return false;
                    }
                }
            }
        }
        return true;
    }

    private bool HasPathToWin(int key)
    {
        if (GraphSpawnerAbility.nodesDict[key].GetComponent<NodeAbility>().lastLayer) return true;
        foreach(int childKey in GraphSpawnerAbility.nodesDict[key].GetComponent<NodeAbility>().childNodes)
        {
            if (GraphSpawnerAbility.nodesDict[childKey].GetComponent<NodeAbility>().state == 0)
            {
                if (HasPathToWin(childKey)) return true;
            }
        }
        return false;
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

    private void CheckNodeFrozen()
    {
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawnerAbility.nodesDict)
        {
            if ((entry.Value).GetComponent<NodeAbility>().state == 3)
            {
                contents = new object[]{key, "Frozen", false, 0};
                PhotonNetwork.RaiseEvent(4, contents, raiseEventOptions, SendOptions.SendReliable);
            }
        }
    }
}

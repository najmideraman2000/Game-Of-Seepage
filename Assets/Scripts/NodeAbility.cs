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
        PhotonView photonView = GetComponent<PhotonView>();
        int viewID = photonView.ViewID;
        if (!GameControllerAbility.gameOver && GameControllerAbility.player == 0 && GameControllerAbility.currentPlayer == 0) // if defender
        {
            if (state == 0)
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
        else if (!GameControllerAbility.gameOver && GameControllerAbility.player == 1 && GameControllerAbility.currentPlayer == 1) // if attacker
        {
            if (state == 0)
            {
                foreach (int parentKey in parentNodes)
                {
                    if (PhotonView.Find(GraphSpawnerAbility.nodesDict[parentKey]).gameObject.GetComponent<Node>().state == 2)
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
                        break;
                    }
                }
                
            }
        }
    }

    public bool attackerLose()
    {
        foreach(KeyValuePair<int, int> entry in GraphSpawnerAbility.nodesDict)
        {
            if (PhotonView.Find(entry.Value).gameObject.GetComponent<Node>().state == 0)
            {
                foreach(int parentKey in PhotonView.Find(entry.Value).gameObject.GetComponent<Node>().parentNodes)
                {
                    if (PhotonView.Find(GraphSpawnerAbility.nodesDict[parentKey]).gameObject.GetComponent<Node>().state == 2)
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
        if (PhotonView.Find(viewID).gameObject.GetComponent<Node>().lastLayer)
        {
            return true;
        }
        foreach(int childKey in PhotonView.Find(viewID).gameObject.GetComponent<Node>().childNodes)
        {
            if (PhotonView.Find(GraphSpawnerAbility.nodesDict[childKey]).gameObject.GetComponent<Node>().state == 0)
            {
                if (hasPathToWin(GraphSpawnerAbility.nodesDict[childKey]))
                {
                    return true;
                }
            }
        }
        return false;
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
        node.GetComponent<Node>().state = state;
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
}

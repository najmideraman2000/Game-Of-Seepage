using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class Node : MonoBehaviour
{
    public int key;
    // state
    // 0: untouched
    // 1: defended
    // 2: attacked
    private int state = 0;
    // player
    // 1: defender
    // 2: attacker
    public List<int> parentNodes;
    public bool lastLayer;
    public List<int> childNodes;
    public GameObject gameOverText;


    public void OnMouseDown() {
        GameObject textObject = GameObject.FindWithTag("Text");
        PhotonView photonView = GetComponent<PhotonView>();
        int viewID = photonView.ViewID;
        if (GameController.player == 0 && GameController.currentPlayer == 0) // if defender
        {
            if (state == 0)
            {   
                photonView.RPC("UpdateNodeColorGreen", RpcTarget.All, viewID);
                photonView.RPC("UpdateNodeState", RpcTarget.All, viewID, 1);
                photonView.RPC("UpdateCurrentPlayer", RpcTarget.All, 1);
                if (attackerLose())
                {
                    Debug.Log("DEFENDER WIN");
                    // textObject.GetComponent<Text>().text = "DEFENDER WIN";
                }
                else {
                    Debug.Log("ATTACKER'S TURN");
                    // textObject.GetComponent<Text>().text = "ATTACKER'S TURN";
                }
            }
        }
        else if (GameController.player == 1 && GameController.currentPlayer == 1) // if attacker
        {
            if (state == 0)
            {
                foreach (int parentKey in parentNodes)
                {
                    if (GraphSpawner.nodesDict[parentKey].GetComponent<Node>().getState() == 3)
                    {
                        GetComponent<Renderer>().material.color = Color.red;
                        state = 3;
                        GameController.player = 1;
                        if (lastLayer) {
                            textObject.GetComponent<Text>().text = "ATTACKER WIN";
                            break;
                        }
                        textObject.GetComponent<Text>().text = "DEFENDER'S TURN";
                        break;
                    }
                }
                
            }
        }
    }

    public bool attackerLose()
    {
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawnerMulti.nodesDict)
        {
            if (entry.Value.GetComponent<Node>().getState() == 0)
            {
                foreach(int parentKey in entry.Value.GetComponent<Node>().parentNodes)
                {
                    if (GraphSpawnerMulti.nodesDict[parentKey].GetComponent<Node>().getState() == 3)
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


    
    public void setKey(int k)
    {
        key = k;
    }
    
    public void setParentNodes(List<int> pN)
    {
        parentNodes = pN;
    }

    public int getState()
    {
        return state;
    }

    public void setState(int s)
    {
        state = s;
    }

    public bool hasPathToWin(GameObject node)
    {
        if (node.GetComponent<Node>().lastLayer)
        {
            return true;
        }
        foreach(int childKey in node.GetComponent<Node>().childNodes)
        {
            if (GraphSpawnerMulti.nodesDict[childKey].GetComponent<Node>().state == 0)
            {
                if (hasPathToWin(GraphSpawnerMulti.nodesDict[childKey]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    [PunRPC]
    public void UpdateNodeState(int viewID, int state)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Node>().setState(state);
    }

    [PunRPC]
    public void UpdateNodeColorRed(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.red;
    }

    [PunRPC]
    public void UpdateNodeColorGreen(int viewID)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Renderer>().material.color = Color.green;
    }

    [PunRPC]
    public void UpdateCurrentPlayer(int currentPlayer)
    {
        GameController.currentPlayer = currentPlayer;
    }

    [PunRPC]
    public void UpdateText(string text)
    {
        GameObject textObject = GameObject.FindWithTag("Text");
        textObject.GetComponent<Text>().text = text;
    }
}

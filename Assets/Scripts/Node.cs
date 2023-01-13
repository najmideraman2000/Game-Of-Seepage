using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Node : MonoBehaviour
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

    private void OnMouseDown() {
        PhotonView photonView = GetComponent<PhotonView>();
        int viewID = photonView.ViewID;
        if (!GameController.gameOver && GameController.player == 0 && GameController.currentPlayer == 0) // if defender
        {
            if (state == 0)
            {   
                photonView.RPC("UpdateNodeColorGreen", RpcTarget.All, viewID);
                photonView.RPC("UpdateNodeState", RpcTarget.All, viewID, 1);
                photonView.RPC("UpdateCurrentPlayer", RpcTarget.All, 1);
                if (AttackerLose())
                {
                    photonView.RPC("UpdateText", RpcTarget.All, "Text", "DEFENDER WIN");
                    photonView.RPC("UpdateGameOver", RpcTarget.All, true);
                    photonView.RPC("UpdateMatchStart", RpcTarget.All, false);
                    if (GameController.player == 0) photonView.RPC("UpdateText", RpcTarget.All, "ResultText", "YOU WIN");
                    else if (GameController.player == 1) photonView.RPC("UpdateText", RpcTarget.All, "ResultText", "YOU LOSE");
                }
            }
        }
        else if (!GameController.gameOver && GameController.player == 1 && GameController.currentPlayer == 1) // if attacker
        {
            if (state == 0)
            {
                foreach (int parentKey in parentNodes)
                {
                    if (PhotonView.Find(GraphSpawnerMulti.nodesDict[parentKey]).gameObject.GetComponent<Node>().state == 2)
                    {
                        photonView.RPC("UpdateNodeColorRed", RpcTarget.All, viewID);
                        photonView.RPC("UpdateNodeState", RpcTarget.All, viewID, 2);
                        photonView.RPC("UpdateCurrentPlayer", RpcTarget.All, 0);
                        if (lastLayer) {
                            photonView.RPC("UpdateText", RpcTarget.All, "Text", "ATTACKER WIN");
                            photonView.RPC("UpdateGameOver", RpcTarget.All, true);
                            photonView.RPC("UpdateMatchStart", RpcTarget.All, false);
                            if (GameController.player == 0) photonView.RPC("UpdateText", RpcTarget.All, "ResultText", "YOU LOSE");
                            else if (GameController.player == 1) photonView.RPC("UpdateText", RpcTarget.All, "ResultText", "YOU WIN");
                        }
                        break;
                    }
                }
                
            }
        }
    }

    private bool AttackerLose()
    {
        foreach(KeyValuePair<int, int> entry in GraphSpawnerMulti.nodesDict)
        {
            if (PhotonView.Find(entry.Value).gameObject.GetComponent<Node>().state == 0)
            {
                foreach(int parentKey in PhotonView.Find(entry.Value).gameObject.GetComponent<Node>().parentNodes)
                {
                    if (PhotonView.Find(GraphSpawnerMulti.nodesDict[parentKey]).gameObject.GetComponent<Node>().state == 2)
                    {
                        if(HasPathToWin(entry.Value)) return false;
                    }
                }
            }
        }
        return true;
    }

    private bool HasPathToWin(int viewID)
    {
        if (PhotonView.Find(viewID).gameObject.GetComponent<Node>().lastLayer) return true;
        foreach(int childKey in PhotonView.Find(viewID).gameObject.GetComponent<Node>().childNodes)
        {
            if (PhotonView.Find(GraphSpawnerMulti.nodesDict[childKey]).gameObject.GetComponent<Node>().state == 0)
            {
                if (HasPathToWin(GraphSpawnerMulti.nodesDict[childKey]))
                {
                    return true;
                }
            }
        }
        return false;
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
    private void UpdateNodeState(int viewID, int state)
    {
        GameObject node = PhotonView.Find(viewID).gameObject;
        node.GetComponent<Node>().state = state;
    }

    [PunRPC]
    private void UpdateCurrentPlayer(int currentPlayer)
    {
        GameController.currentPlayer = currentPlayer;
    }

    [PunRPC]
    private void UpdateText(string tag, string text)
    {
        GameObject textObject = GameObject.FindWithTag(tag);
        textObject.GetComponent<Text>().text = text;
    }

    [PunRPC]
    private void UpdateGameOver(bool state)
    {
        GameController.gameOver = state;
    }

    [PunRPC]
    private void UpdateMatchStart(bool state)
    {
        GameController.matchStart = state;
    }
}

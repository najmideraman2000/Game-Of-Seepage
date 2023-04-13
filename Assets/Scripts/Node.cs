using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

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
    private RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    private object[] contents;
    private bool hovered = false;

    private void OnMouseDown()
    {
        if (GameController.settingOpened) return;
        if (GameController.gameOver) return;

        if (GameController.currentPlayer != GameController.player)
        {
            contents = new object[]{0};
            PhotonNetwork.RaiseEvent(6, contents, raiseEventOptions, SendOptions.SendReliable);
            return;
        }

        if (state == 0)
        {
            if (GameController.player == 0)
            {
                state = 1;
                contents = new object[]{key, "Defended", 1, 1};
                PhotonNetwork.RaiseEvent(0, contents, raiseEventOptions, SendOptions.SendReliable);
                if (AttackerLose())
                {
                    contents = new object[]{"Text", "DEFENDER WIN"};
                    PhotonNetwork.RaiseEvent(1, contents, raiseEventOptions, SendOptions.SendReliable);
                    GameController.winGame = true;
                }
            }
            else if (GameController.player == 1)
            {
                foreach (int parentKey in parentNodes)
                {
                    if (GraphSpawner.nodesDict[parentKey].GetComponent<Node>().state == 2)
                    {
                        contents = new object[]{key, "Attacked", 2, 0};
                        PhotonNetwork.RaiseEvent(0, contents, raiseEventOptions, SendOptions.SendReliable);
                        if (lastLayer) 
                        {
                            contents = new object[]{"Text", "ATTACKER WIN"};
                            PhotonNetwork.RaiseEvent(1, contents, raiseEventOptions, SendOptions.SendReliable);
                            GameController.winGame = true;
                        }
                        return;
                    }
                }
                contents = new object[]{1};
                PhotonNetwork.RaiseEvent(6, contents, raiseEventOptions, SendOptions.SendReliable);
            }
        }
        else if (state == 1)
        {
            contents = new object[]{2};
            PhotonNetwork.RaiseEvent(6, contents, raiseEventOptions, SendOptions.SendReliable);
        }
        else if (state == 2)
        {
            contents = new object[]{3};
            PhotonNetwork.RaiseEvent(6, contents, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    private void OnMouseEnter()
    {
        List<GameObject> edges = GraphSpawner.connectedEdges[key];
        foreach (GameObject edge in edges) {
            edge.GetComponent<Renderer>().material.color = Color.yellow;
            edge.GetComponent<Renderer>().sortingLayerName = "Layer1.5";
        }

        if (GameController.settingOpened) return;
        if (!GameController.gameOver && GameController.player == 0 && GameController.currentPlayer == 0 && state == 0)
        {
            float scale = transform.localScale.x;
            transform.localScale = new Vector3(1.2f * scale, 1.2f * scale, 1);
            hovered = true;
        }
        else if (!GameController.gameOver && GameController.player == 1 && GameController.currentPlayer == 1 && state == 0)
        {
            foreach (int parentKey in parentNodes)
            {
                if (GraphSpawner.nodesDict[parentKey].GetComponent<Node>().state == 2)
                {
                    float scale = transform.localScale.x;
                    transform.localScale = new Vector3(1.2f * scale, 1.2f * scale, 1);
                    hovered = true;
                    break;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        List<GameObject> edges = GraphSpawner.connectedEdges[key];
        foreach (GameObject edge in edges) {
            Color color = new Color32((byte)(0xFF), (byte)(0xFF), (byte)(0xFF), (byte)(0xFF));
            edge.GetComponent<Renderer>().material.color = color;
            edge.GetComponent<Renderer>().sortingLayerName = "Layer1";
        }
        
        if (GameController.settingOpened) return;
        if (hovered)
        {
            float scale = transform.localScale.x;
            transform.localScale = new Vector3(scale / 1.2f,scale / 1.2f, 1);
            hovered = false;
        }
    }

    private bool AttackerLose()
    {
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawner.nodesDict)
        {
            if ((entry.Value).GetComponent<Node>().state == 0)
            {
                foreach(int parentKey in (entry.Value).GetComponent<Node>().parentNodes)
                {
                    if (GraphSpawner.nodesDict[parentKey].GetComponent<Node>().state == 2)
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
        if (GraphSpawner.nodesDict[key].GetComponent<Node>().lastLayer) return true;
        foreach(int childKey in GraphSpawner.nodesDict[key].GetComponent<Node>().childNodes)
        {
            if (GraphSpawner.nodesDict[childKey].GetComponent<Node>().state == 0)
            {
                if (HasPathToWin(childKey)) return true;
            }
        }
        return false;
    }
}

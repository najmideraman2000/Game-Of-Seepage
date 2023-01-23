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

    private void OnMouseDown() {
        if (!GameController.gameOver && GameController.player == 0 && GameController.currentPlayer == 0 && state == 0) // if defender
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
        else if (!GameController.gameOver && GameController.player == 1 && GameController.currentPlayer == 1 && state == 0) // if attacker
        {
            foreach (int parentKey in parentNodes)
            {
                if (GraphSpawnerMulti.nodesDict[parentKey].GetComponent<Node>().state == 2)
                {
                    contents = new object[]{key, "Attacked", 2, 0};
                    PhotonNetwork.RaiseEvent(0, contents, raiseEventOptions, SendOptions.SendReliable);
                    if (lastLayer) 
                    {
                        contents = new object[]{"Text", "ATTACKER WIN"};
                        PhotonNetwork.RaiseEvent(1, contents, raiseEventOptions, SendOptions.SendReliable);
                        GameController.winGame = true;
                    }
                    break;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
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
                if (GraphSpawnerMulti.nodesDict[parentKey].GetComponent<Node>().state == 2)
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
        if (hovered)
        {
            float scale = transform.localScale.x;
            transform.localScale = new Vector3(1.2f / scale, 1.2f / scale, 1);
            hovered = false;
        }
    }

    private bool AttackerLose()
    {
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawnerMulti.nodesDict)
        {
            if ((entry.Value).GetComponent<Node>().state == 0)
            {
                foreach(int parentKey in (entry.Value).GetComponent<Node>().parentNodes)
                {
                    if (GraphSpawnerMulti.nodesDict[parentKey].GetComponent<Node>().state == 2)
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
        if (GraphSpawnerMulti.nodesDict[key].GetComponent<Node>().lastLayer) return true;
        foreach(int childKey in GraphSpawnerMulti.nodesDict[key].GetComponent<Node>().childNodes)
        {
            if (GraphSpawnerMulti.nodesDict[childKey].GetComponent<Node>().state == 0)
            {
                if (HasPathToWin(childKey)) return true;
            }
        }
        return false;
    }
}

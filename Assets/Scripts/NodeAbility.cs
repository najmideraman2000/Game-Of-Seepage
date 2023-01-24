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
    private bool hovered = false;

    private void OnMouseDown() {
        if (GameControllerAbility.settingOpened) return;
        if (!GameControllerAbility.gameOver && GameControllerAbility.player == 0 && GameControllerAbility.currentPlayer == 0 && state == 0) // if defender
        {
            if (GameControllerAbility.abilityChoosed && !GameControllerAbility.abilityDone)
            {
                contents = new object[]{key, "Frozen", true, 3};
                PhotonNetwork.RaiseEvent(4, contents, raiseEventOptions, SendOptions.SendReliable);
                GameControllerAbility.abilityChoosed = false;
                GameControllerAbility.abilityDone = true;
            }
            else
            {
                state = 1;
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
        else if (!GameControllerAbility.gameOver && GameControllerAbility.player == 1 && GameControllerAbility.currentPlayer == 1 && state == 0) // if attacker
        {
            if (GameControllerAbility.abilityChoosed && !GameControllerAbility.abilityDone)
            {
                if (GameControllerAbility.firstNodeChoosed)
                {
                    contents = new object[]{GameControllerAbility.firstKey, key};
                    PhotonNetwork.RaiseEvent(5, contents, raiseEventOptions, SendOptions.SendReliable);
                    GameControllerAbility.abilityChoosed = false;
                    GameControllerAbility.firstNodeChoosed = false;
                    GameControllerAbility.abilityDone = true;
                    GraphSpawnerAbility.nodesDict[GameControllerAbility.firstKey].GetComponent<Animator>().SetBool("ChosenFirstNode", false);
                }
                else
                {
                    GameControllerAbility.firstNodeChoosed = true;
                    GameControllerAbility.firstKey = key;
                    GetComponent<Animator>().SetBool("ChosenFirstNode", true);
                }
            }
            else
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

    private void OnMouseEnter()
    {
        if (GameControllerAbility.settingOpened) return;
        if (!GameControllerAbility.gameOver && GameControllerAbility.player == 0 && GameControllerAbility.currentPlayer == 0 && state == 0) // if defender
        {
            float scale = transform.localScale.x;
            transform.localScale = new Vector3(1.2f * scale, 1.2f * scale, 1);
            hovered = true;
        }
        else if (!GameControllerAbility.gameOver && GameControllerAbility.player == 1 && GameControllerAbility.currentPlayer == 1 && state == 0) // if attacker
        {
            if (GameControllerAbility.abilityChoosed && !GameControllerAbility.abilityDone)
            {
                float scale = transform.localScale.x;
                transform.localScale = new Vector3(1.2f * scale, 1.2f * scale, 1);
                hovered = true;
            }
            else
            {
                foreach (int parentKey in parentNodes)
                {
                    if (GraphSpawnerAbility.nodesDict[parentKey].GetComponent<NodeAbility>().state == 2)
                    {
                        float scale = transform.localScale.x;
                        transform.localScale = new Vector3(1.2f * scale, 1.2f * scale, 1);
                        hovered = true;
                        break;
                    }
                }
                    
            }
        }
    }

    private void OnMouseExit()
    {
        if (GameControllerAbility.settingOpened) return;
        if (hovered)
        {
            float scale = transform.localScale.x;
            transform.localScale = new Vector3(scale / 1.2f, scale / 1.2f, 1);
            hovered = false;
        }
    }

    private bool AttackerLose()
    {
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawnerAbility.nodesDict)
        {
            if ((entry.Value).GetComponent<NodeAbility>().state == 0)
            {
                foreach(int parentKey in (entry.Value).GetComponent<NodeAbility>().parentNodes)
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

    private void CheckNodeFrozen()
    {
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawnerAbility.nodesDict)
        {
            if ((entry.Value).GetComponent<NodeAbility>().state == 3)
            {
                contents = new object[]{entry.Key, "Frozen", false, 0};
                PhotonNetwork.RaiseEvent(4, contents, raiseEventOptions, SendOptions.SendReliable);
            }
        }
    }
}

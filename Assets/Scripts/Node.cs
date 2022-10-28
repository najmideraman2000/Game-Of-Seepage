using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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


    public void OnMouseDown() {
        GameObject textObject = GameObject.FindWithTag("Text");
        if (GameController.player == 1)
        {
            if (state == 0)
            {   
                GetComponent<Renderer>().material.color = Color.green;
                state = 1;
                GameController.player = 2;
                if (attackerLose())
                {
                    textObject.GetComponent<Text>().text = "DEFENDER'S WIN";
                }
                else {
                    textObject.GetComponent<Text>().text = "ATTACKER'S TURN";
                }
            }
        }
        if (GameController.player == 2)
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
                            textObject.GetComponent<Text>().text = "ATTACKER's WIN";
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
        foreach(KeyValuePair<int, GameObject> entry in GraphSpawner.nodesDict)
        {
            if (entry.Value.GetComponent<Node>().getState() == 0)
            {
                foreach(int parentKey in entry.Value.GetComponent<Node>().parentNodes)
                {
                    if (GraphSpawner.nodesDict[parentKey].GetComponent<Node>().getState() == 3)
                    {
                        Debug.Log(entry.Value.GetComponent<Node>().key);
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
            if (GraphSpawner.nodesDict[childKey].GetComponent<Node>().state == 0)
            {
                if (hasPathToWin(GraphSpawner.nodesDict[childKey]))
                {
                    return true;
                }
            }
        }
        return false;
    }
}

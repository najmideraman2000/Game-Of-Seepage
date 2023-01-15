using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class PhotonEventHandler : MonoBehaviour, IOnEventCallback
{
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 0)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animBool = (string)  contents[1];
            int state = (int) contents[2];
            int currentPlayer = (int) contents[3];
            GameObject node = GraphSpawnerMulti.nodesDict[key];

            node.GetComponent<Animator>().SetBool(animBool, true);
            node.GetComponent<Node>().state = state;
            GameController.currentPlayer = currentPlayer;
        }

        else if (photonEvent.Code == 1)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            string tag = (string) contents[0];
            string text = (string)  contents[1];

            GameObject textObject = GameObject.FindWithTag(tag);
            textObject.GetComponent<Text>().text = text;
            GameController.gameOver = true;
            GameController.matchStart = false;
        }

        else if (photonEvent.Code == 2)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animBool = (string)  contents[1];
            int state = (int) contents[2];
            int currentPlayer = (int) contents[3];
            GameObject node = GraphSpawnerAbility.nodesDict[key];

            node.GetComponent<Animator>().SetBool(animBool, true);
            node.GetComponent<NodeAbility>().state = state;
            GameControllerAbility.currentPlayer = currentPlayer;
        }

        else if (photonEvent.Code == 3)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            string tag = (string) contents[0];
            string text = (string)  contents[1];

            GameObject textObject = GameObject.FindWithTag(tag);
            textObject.GetComponent<Text>().text = text;
            GameControllerAbility.gameOver = true;
            GameControllerAbility.matchStart = false;
        }

        else if (photonEvent.Code == 4)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animTag = (string)  contents[1];
            bool animBool = (bool) contents[2];
            int state = (int) contents[3];
            GameObject node = GraphSpawnerAbility.nodesDict[key];

            node.GetComponent<Animator>().SetBool(animTag, animBool);
            node.GetComponent<NodeAbility>().state = state;
        }

        else if (photonEvent.Code == 5)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int firstKey = (int) contents[0];
            int secondKey = (int) contents[1];
            GraphSpawnerAbility.nodesDict[firstKey].GetComponent<NodeAbility>().childNodes.Add(secondKey);
            GraphSpawnerAbility.nodesDict[secondKey].GetComponent<NodeAbility>().parentNodes.Add(firstKey);
        }
    }
}
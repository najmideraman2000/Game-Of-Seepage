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
    }
}
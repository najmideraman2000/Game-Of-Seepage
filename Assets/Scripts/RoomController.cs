using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{

    public Text player1Name;
    public Text player2Name;
    public Text roomKeyName;
    void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        int numOfPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        if (numOfPlayer == 1)
        {
            photonView.RPC("UpdateRoomKey", RpcTarget.AllBuffered, CreateAndJoinRooms.roomKey);
            photonView.RPC("UpdatePlayer1Name", RpcTarget.AllBuffered, CreateAndJoinRooms.playerName);
        }
        else {
            photonView.RPC("UpdatePlayer2Name", RpcTarget.AllBuffered, CreateAndJoinRooms.playerName);
        }
    }

    [PunRPC]
    public void UpdatePlayer1Name(string text)
    {
        player1Name.GetComponent<Text>().text = text;
    }
    [PunRPC]
    public void UpdatePlayer2Name(string text)
    {
        player2Name.GetComponent<Text>().text = text;
    }
    [PunRPC]
    public void UpdateRoomKey(string text)
    {
        roomKeyName.GetComponent<Text>().text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviourPunCallbacks
{

    public Text player1Name;
    public Text player2Name;
    public Text roomKeyName;
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonView photonView = GetComponent<PhotonView>();
        int numOfPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        if (numOfPlayer == 1)
        {
            // photonView.RPC("UpdateRoomKey", RpcTarget.AllBuffered, CreateAndJoinRooms.roomKey);
            photonView.RPC("UpdatePlayer1Name", RpcTarget.AllBuffered, JoinTest.playerName);
        }
        else {
            photonView.RPC("UpdatePlayer2Name", RpcTarget.AllBuffered, JoinTest.playerName);
        }
    }

    public void playMultiPlayer()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        int randomNumber = 0;
        if (randomNumber == 0)
        {
            GameController.player = 0;
            photonView.RPC("UpdatePlayer2Player", RpcTarget.Others, 1);
        }
        else
        {
            GameController.player = 1;
            photonView.RPC("UpdatePlayer2Player", RpcTarget.Others, 0);
        }
        PhotonNetwork.LoadLevel("GameplayMultiplayer");
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        // TODO load scene connecting to lobby
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("LobbyTest");
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

    [PunRPC]
    public void UpdatePlayer2Player(int player)
    {
        GameController.player = player;
    }
}

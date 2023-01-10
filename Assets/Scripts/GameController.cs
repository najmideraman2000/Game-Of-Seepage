using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviourPunCallbacks
{
    public static int player;
    public static int currentPlayer = 0;
    public static bool gameOver = false;

    public void leaveGame()
    {
        Debug.Log("lmao");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        resetGame();
        // TODO load scene connecting to lobby
        // PhotonNetwork.LoadLevel("LobbyTest");
        // SceneManager.LoadScene("LobbyTest");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void resetGame()
    {
        GraphSpawnerMulti.nodesDict = new Dictionary<int, int>{};
        currentPlayer = 0;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("LobbyTest");
    }
}

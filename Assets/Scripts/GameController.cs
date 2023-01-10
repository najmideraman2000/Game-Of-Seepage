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
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        resetGame();
        PhotonNetwork.ConnectUsingSettings();
    }

    public void resetGame()
    {
        GraphSpawnerMulti.nodesDict = new Dictionary<int, int>{};
        currentPlayer = 0;
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("FindGame");
    }
}

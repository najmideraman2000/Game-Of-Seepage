using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        PhotonNetwork.LoadLevel("GameLobby");
    }

    public void resetGame()
    {
        GraphSpawnerMulti.nodesDict = new Dictionary<int, int>{};
        currentPlayer = 0;
    }
}

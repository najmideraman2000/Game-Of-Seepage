using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerAbility : MonoBehaviourPunCallbacks
{
    public static int player;
    public static int currentPlayer = 0;
    public static bool gameOver = false;

    public static bool abilityChoosed = false;
    public static bool firstNodeChoosed = false;
    public static int firstKey;
    public static bool matchStart = false;
    public static double gameTime = 100;
    private double defenderTimeRemain = 100;
    private double attackerTimeRemain = 100;
    static double startTime;
    public Text defenderTimeText;
    public Text attackerTimeText;

    void Start()
    {
        defenderTimeRemain = gameTime;
        attackerTimeRemain = gameTime;
        string minute = ((int) (gameTime / 60)).ToString();
        string second = ((int) (gameTime % 60)).ToString();
        string timeText = minute + " : " + second;
        defenderTimeText.text = timeText;
        attackerTimeText.text = timeText;
        if (PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time;
            // PhotonNetwork.CurrentRoom.CustomProperties.Add("startTime", startTime);
            // startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["startTime"].ToString());
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("UpdateStartTime", RpcTarget.All, startTime);
            photonView.RPC("UpdateMatchStart", RpcTarget.All, true);
            matchStart = true;
        }
        // else
        // {
        //     startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["startTime"].ToString());
        //     matchStart = true;
        // }
    }

    void Update()
    {
        if (!matchStart) return;
        if (currentPlayer == 0)
        {
            // continue defender timer
            defenderTimeRemain = 200 - attackerTimeRemain - (PhotonNetwork.Time - startTime);
            updateTimeDefenderUI();
            if (defenderTimeRemain <= 0)
            {
                // gameOver
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("UpdateGameOver", RpcTarget.All);
                photonView.RPC("UpdateMatchStart", RpcTarget.All, false);
            }
        }
        else if (currentPlayer == 1)
        {
            // continue attacker timer
            attackerTimeRemain = 200 - defenderTimeRemain - (PhotonNetwork.Time - startTime);
            updateTimeAttackerUI();
            if (attackerTimeRemain <= 0)
            {
                // gameOver
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("UpdateGameOver", RpcTarget.All);
                photonView.RPC("UpdateMatchStart", RpcTarget.All, false);
            }
        }
    }

    public void updateTimeDefenderUI()
    {
        // Debug.Log(defenderTimeRemain);
        string minute = ((int) (defenderTimeRemain / 60)).ToString("00");
        string second = ((int) (defenderTimeRemain % 60)).ToString("00");
        string timeText = minute + " : " + second;
        defenderTimeText.text = timeText;
    }

    public void updateTimeAttackerUI()
    {
        // Debug.Log(attackerTimeRemain);
        string minute = ((int) (attackerTimeRemain / 60)).ToString("00");
        string second = ((int) (attackerTimeRemain % 60)).ToString("00");
        string timeText = minute + " : " + second;
        attackerTimeText.text = timeText;
    }

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

    public void chooseAbility()
    {
        abilityChoosed = true;
    }

    [PunRPC]
    public void UpdateGameOver()
    {
        GameControllerAbility.gameOver = true;
    }

    [PunRPC]
    public void UpdateMatchStart(bool state)
    {
        GameControllerAbility.matchStart = state;
    }

    [PunRPC]
    public void UpdateStartTime(double startTime)
    {
        GameControllerAbility.startTime = startTime;
    }
}

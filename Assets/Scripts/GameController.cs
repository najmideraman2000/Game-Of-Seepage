using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class GameController : MonoBehaviourPunCallbacks
{
    public static int player;
    public static int currentPlayer = 0;
    public static bool gameOver = false;
    public static bool matchStart = false;
    public static double gameTime = 10;
    private static double startTime;
    private double defenderTimeRemain;
    private double attackerTimeRemain;
    public Text turnText;
    public Text roleText;
    public Text defenderTimeText;
    public Text attackerTimeText;
    public Text resultText;
    public GameObject canvasGameOver;

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
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("UpdateStartTime", RpcTarget.All, PhotonNetwork.Time);
            photonView.RPC("UpdateMatchStart", RpcTarget.All, true);
        }
    }

    void Update()
    {
        if (gameOver)
        {
            canvasGameOver.SetActive(true);
            gameOver = false;
            return;
        }
        if (!matchStart) return;
        UpdateTurnRoleText();
        if (currentPlayer == 0)
        {
            defenderTimeRemain = (2 * gameTime) - attackerTimeRemain - (PhotonNetwork.Time - startTime);
            UpdateTimeDefenderUI();
            if (defenderTimeRemain <= 0)
            {
                turnText.text = "ATTACKER WIN";
                if (player == 0)
                {
                    resultText.text = "YOU LOSE";
                }
                else
                {
                    resultText.text = "YOU WIN";
                }
                gameOver = true;
                matchStart = false;
            }
        }
        else if (currentPlayer == 1)
        {
            attackerTimeRemain = (2 * gameTime) - defenderTimeRemain - (PhotonNetwork.Time - startTime);
            UpdateTimeAttackerUI();
            if (attackerTimeRemain <= 0)
            {
                turnText.text = "DEFENDER WIN";
                if (player == 0)
                {
                    resultText.text = "YOU WIN";
                }
                else
                {
                    resultText.text = "YOU LOSE";
                }
                gameOver = true;
                matchStart = false;
            }
        }
    }

    public void UpdateTurnRoleText()
    {
        if (player == 0)
        {
            if (currentPlayer == 0)
            {
                turnText.text = "YOUR TURN";
                roleText.text = "(DEFENDER)";
            }
            else if (currentPlayer == 1)
            {
                turnText.text = "OPPONENT'S TURN";
                roleText.text = "(ATTACKER)";
            }
        }
        if (player == 1)
        {
            if (currentPlayer == 0)
            {
                turnText.text = "OPPONENT'S TURN";
                roleText.text = "(DEFENDER)";
            }
            else if (currentPlayer == 1)
            {
                turnText.text ="YOUR TURN";
                roleText.text = "(ATTACKER)";
            }
        }
    }

    public void UpdateTimeDefenderUI()
    {
        string minute = ((int) (defenderTimeRemain / 60)).ToString("00");
        string second = ((int) (defenderTimeRemain % 60)).ToString("00");
        string timeText = minute + " : " + second;
        defenderTimeText.text = timeText;
    }

    public void UpdateTimeAttackerUI()
    {
        string minute = ((int) (attackerTimeRemain / 60)).ToString("00");
        string second = ((int) (attackerTimeRemain % 60)).ToString("00");
        string timeText = minute + " : " + second;
        attackerTimeText.text = timeText;
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        ResetGame();
        SceneManager.LoadScene("ConnectServer");
    }

    public void ResetGame()
    {
        GraphSpawnerMulti.nodesDict = new Dictionary<int, int>{};
        currentPlayer = 0;
    }

    public void BackToMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    [PunRPC]
    public void UpdateStartTime(double startTime)
    {
        GameController.startTime = startTime;
    }

    [PunRPC]
    public void UpdateMatchStart(bool state)
    {
        GameController.matchStart = state;
    }
}

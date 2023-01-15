using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameControllerAbility : MonoBehaviourPunCallbacks
{
    public static int player;
    public static int currentPlayer = 0;
    public static bool abilityChoosed = false;
    public static bool firstNodeChoosed = false;
    public static bool abilityDone = false;
    public static int firstKey;
    public static bool gameOver = false;
    public static bool matchStart = false;
    public static double gameTime = 180;
    private static double startTime;
    public static bool winGame = false;
    private double defenderTimeRemain;
    private double attackerTimeRemain;
    public GameObject abilityButton;
    public Sprite defenderAbilityImage;
    public Sprite attackerAbilityImage;
    public Text turnText;
    public Text roleText;
    public Text defenderTimeText;
    public Text attackerTimeText;
    public Text resultText;
    public GameObject canvasSetting;
    public GameObject canvasGameOver;

    private void Start()
    {
        if (player == 0) abilityButton.GetComponent<Image>().sprite = defenderAbilityImage;
        else if (player == 1) abilityButton.GetComponent<Image>().sprite = attackerAbilityImage;
        defenderTimeRemain = gameTime;
        attackerTimeRemain = gameTime;
        string minute = ((int) (gameTime / 60)).ToString("00");
        string second = ((int) (gameTime % 60)).ToString("00");
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

    private void Update()
    {
        if (gameOver)
        {
            if (winGame) resultText.text = "YOU WIN";
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
                if (player == 1) winGame = true;
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
                if (player == 0) winGame = true;
                gameOver = true;
                matchStart = false;
            }
        }
    }

    private void UpdateTurnRoleText()
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

    private void UpdateTimeDefenderUI()
    {
        string minute = ((int) (defenderTimeRemain / 60)).ToString("00");
        string second = ((int) (defenderTimeRemain % 60)).ToString("00");
        string timeText = minute + " : " + second;
        defenderTimeText.text = timeText;
    }

    private void UpdateTimeAttackerUI()
    {
        string minute = ((int) (attackerTimeRemain / 60)).ToString("00");
        string second = ((int) (attackerTimeRemain % 60)).ToString("00");
        string timeText = minute + " : " + second;
        attackerTimeText.text = timeText;
    }

    public void UseAbility()
    {
        abilityChoosed = true;
        abilityButton.GetComponent<Button>().interactable = false;
    }

    public void OpenSetting()
    {
        canvasSetting.SetActive(true);
    }

    public void CloseSetting()
    {
        canvasSetting.SetActive(false);
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

    private void ResetGame()
    {
        GraphSpawnerAbility.nodesDict = new Dictionary<int, GameObject>{};
        currentPlayer = 0;
        abilityChoosed = false;
        firstNodeChoosed = false;
        abilityDone = false;
        gameOver = false;
        matchStart = false;
        winGame = false;
    }

    public void BackToMenu()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("MainMenu");
    }

    [PunRPC]
    private void UpdateStartTime(double startTime)
    {
        GameControllerAbility.startTime = startTime;
    }

    [PunRPC]
    private void UpdateMatchStart(bool state)
    {
        GameControllerAbility.matchStart = state;
    }
}

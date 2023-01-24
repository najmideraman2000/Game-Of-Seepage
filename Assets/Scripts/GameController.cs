using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPunCallbacks
{
    public static int player;
    public static int currentPlayer = 0;
    public static bool gameOver = false;
    public static bool matchStart = false;
    public static bool settingOpened = false;
    public static double gameTime = 180;
    private static double startTime;
    public static bool winGame = false;
    private double defenderTimeRemain;
    private double attackerTimeRemain;
    public Text turnText;
    public Text roleText;
    public Text defenderTimeText;
    public Text attackerTimeText;
    public Text resultText;
    public GameObject canvasSetting;
    public GameObject canvasGameOver;
    public Slider musicSlider;
    public Slider effectSlider;
    public AudioSource musicSource;
    public AudioSource effectSource;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume")) PlayerPrefs.SetFloat("musicVolume", 0.5f);
        if (!PlayerPrefs.HasKey("effectVolume")) PlayerPrefs.SetFloat("effectVolume", 0.5f);
        LoadSetting();
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

    public void CloseGameOver()
    {
        canvasGameOver.SetActive(false);
    }

    public void OpenSetting()
    {
        canvasSetting.SetActive(true);
        settingOpened = true;
    }

    public void CloseSetting()
    {
        canvasSetting.SetActive(false);
        settingOpened = false;
        SaveSetting();
    }

    public void ChangeVolume()
    {
        musicSource.volume = musicSlider.value;
        effectSource.volume = effectSlider.value;
    }

    private void LoadSetting()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        effectSlider.value = PlayerPrefs.GetFloat("effectVolume");
    }

    private void SaveSetting()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("effectVolume", effectSlider.value);
    }

    public void LeaveGame()
    {
        SaveSetting();
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        ResetGame();
        SceneManager.LoadScene("ConnectServer");
    }

    private void ResetGame()
    {
        GraphSpawnerMulti.nodesDict = new Dictionary<int, GameObject>{};
        currentPlayer = 0;
        gameOver = false;
        matchStart = false;
        winGame = false;
        settingOpened = false;
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
        GameController.startTime = startTime;
    }

    [PunRPC]
    private void UpdateMatchStart(bool state)
    {
        GameController.matchStart = state;
    }
}

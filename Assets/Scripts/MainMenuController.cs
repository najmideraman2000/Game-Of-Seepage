using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void playSingleplayerGame() 
    {
        SceneManager.LoadScene("GamePlay");
        
    }
    public void playMultiplayerGame()
    {
        SceneManager.LoadScene("LoadingLobby");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.LoadScene("ConnectServer");
    }
}

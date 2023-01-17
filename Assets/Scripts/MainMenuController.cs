using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMenuController : MonoBehaviour
{
    public GameObject canvasSetting;
    public Slider volumeSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadSetting();
        }
        else LoadSetting();
    }

    public void PlayGame()
    {
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.LoadScene("ConnectServer");
    }

    public void OpenSetting()
    {
        canvasSetting.SetActive(true);
    }

    public void CloseSetting()
    {
        canvasSetting.SetActive(false);
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveSetting();
    }

    private void LoadSetting()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void SaveSetting()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}

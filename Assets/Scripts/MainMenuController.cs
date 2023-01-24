using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMenuController : MonoBehaviour
{
    public GameObject canvasSetting;
    public Slider musicSlider;
    public AudioSource musicSource;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume")) PlayerPrefs.SetFloat("musicVolume", 0.5f);
        LoadSetting();
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
        SaveSetting();
    }

    public void ChangeVolume()
    {
        musicSource.volume = musicSlider.value;
    }

    private void LoadSetting()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void SaveSetting()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }
}

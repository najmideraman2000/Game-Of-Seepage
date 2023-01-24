using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        for (int i = 0; i < GraphCollections.graphCollections.Count; i++)
        {
            List<List<List<int>>> graph = GraphCollections.graphCollections[i];
            bool correctGraph = CheckGraph(graph);
            if (!correctGraph)
            {
                Debug.Log("Graph " + i.ToString() + " is wrong");
            }
        }
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

    private bool CheckGraph(List<List<List<int>>> graph)
    {
        List<List<int>> arrangement = graph[0];
        List<List<int>> parents = graph[1];
        List<int> flatList = arrangement.SelectMany(x => x).ToList();

        // check arrangement list
        if (arrangement[0].Count != 1) return false;
        for (int i = 0; i < flatList.Count; i++)
        {
            if (i != flatList[i]) return false;
        }
        // check parents list
        if (parents[0].Count != 0) return false;
        Dictionary<int, int> layerDict = new Dictionary<int, int>{};
        for (int i = 0; i < arrangement.Count; i ++)
        {
            for (int j = 0; j < arrangement[i].Count; j++)
            {
                layerDict.Add(arrangement[i][j], i);
            }
        }
        for (int key = 1; key < flatList.Count; key++)
        {
            int parentsLayer = layerDict[key] - 1;
            List<int> nodesInLayer = arrangement[parentsLayer];
            for (int i = 0; i < parents[key].Count; i++)
            {
                int parentKey = parents[key][i];
                if (!nodesInLayer.Contains(parentKey)) return false;
            }
        }
        return true;
    }
}

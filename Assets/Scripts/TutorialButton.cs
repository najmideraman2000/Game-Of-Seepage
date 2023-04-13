using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class TutorialButton : MonoBehaviour
{
    private int tutoPage = 0;
    public GameObject canvasMenu;
    public GameObject canvasTuto;
    public GameObject[] tutoPagesStandard;
    public GameObject[] tutoPagesAbility;
    public GameObject howToBtn;
    public GameObject standardTutoBtn;
    public GameObject abilityTutoBtn;
    public GameObject prevBtn;
    public GameObject nextBtn;

    public void OpenTuto()
    {
        canvasMenu.SetActive(false);
        howToBtn.SetActive(false);
        canvasTuto.SetActive(true);
        standardTutoBtn.GetComponent<Button>().interactable = false;
        abilityTutoBtn.GetComponent<Button>().interactable = true;
        prevBtn.GetComponent<Button>().interactable = false;
        nextBtn.GetComponent<Button>().interactable = true;
        tutoPagesStandard[0].SetActive(true);

        foreach (GameObject obj in GraphSpawner.allObjects) {
            obj.SetActive(false);
        }
        foreach (GameObject obj in GraphSpawnerEdgeStep.allObjects) {
            obj.SetActive(false);
        }
    }

    public void CloseTuto()
    {
        canvasTuto.SetActive(false);
        foreach (GameObject tutoPageObj in tutoPagesStandard)
        {
            tutoPageObj.SetActive(false);
        }
        foreach (GameObject tutoPageObj in tutoPagesAbility)
        {
            tutoPageObj.SetActive(false);
        }
        tutoPage = 0;
        canvasMenu.SetActive(true);
        howToBtn.SetActive(true);

        foreach (GameObject obj in GraphSpawner.allObjects) {
            obj.SetActive(true);
        }
        foreach (GameObject obj in GraphSpawnerEdgeStep.allObjects) {
            obj.SetActive(true);
        }
    }

    public void ChangeTutoMode()
    {
        if (abilityTutoBtn.GetComponent<Button>().interactable)
        {
            foreach (GameObject tutoPageObj in tutoPagesStandard)
            {
                tutoPageObj.SetActive(false);
            }
            standardTutoBtn.GetComponent<Button>().interactable = true;
            abilityTutoBtn.GetComponent<Button>().interactable = false;
            tutoPagesAbility[0].SetActive(true);
        }
        else if (standardTutoBtn.GetComponent<Button>().interactable)
        {
            foreach (GameObject tutoPageObj in tutoPagesAbility)
            {
                tutoPageObj.SetActive(false);
            }
            abilityTutoBtn.GetComponent<Button>().interactable = true;
            standardTutoBtn.GetComponent<Button>().interactable = false;
            tutoPagesStandard[0].SetActive(true);
        }
        tutoPage = 0;
        prevBtn.GetComponent<Button>().interactable = false;
        nextBtn.GetComponent<Button>().interactable = true;
    }

    public void ChangeTutoPrev()
    {
        if (!standardTutoBtn.GetComponent<Button>().interactable)
        {
            if (tutoPage == 0) return;
            if (tutoPage == 4)
            {
                tutoPagesStandard[4].SetActive(false);
                tutoPagesStandard[3].SetActive(true);
                nextBtn.GetComponent<Button>().interactable = true;
            }
            else if (tutoPage == 3)
            {
                tutoPagesStandard[3].SetActive(false);
                tutoPagesStandard[2].SetActive(true);
            }
            else if (tutoPage == 2)
            {
                tutoPagesStandard[2].SetActive(false);
                tutoPagesStandard[1].SetActive(true);
            }
            else if (tutoPage == 1)
            {
                tutoPagesStandard[1].SetActive(false);
                tutoPagesStandard[0].SetActive(true);
                prevBtn.GetComponent<Button>().interactable = false;
            }
            tutoPage -= 1;
        }
        else if (!abilityTutoBtn.GetComponent<Button>().interactable)
        {
            if (tutoPage == 0) return;
            if (tutoPage == 2)
            {
                tutoPagesAbility[2].SetActive(false);
                tutoPagesAbility[1].SetActive(true);
                nextBtn.GetComponent<Button>().interactable = true;
            }
            else if (tutoPage == 1)
            {
                tutoPagesAbility[1].SetActive(false);
                tutoPagesAbility[0].SetActive(true);
                prevBtn.GetComponent<Button>().interactable = false;
            }
            tutoPage -= 1;
        }
    }

    public void ChangeTutoNext()
    {
        if (!standardTutoBtn.GetComponent<Button>().interactable)
        {
            if (tutoPage == 4) return;
            if (tutoPage == 0)
            {
                tutoPagesStandard[0].SetActive(false);
                tutoPagesStandard[1].SetActive(true);
                prevBtn.GetComponent<Button>().interactable = true;
            }
            else if (tutoPage == 1)
            {
                tutoPagesStandard[1].SetActive(false);
                tutoPagesStandard[2].SetActive(true);
            }
            else if (tutoPage == 2)
            {
                tutoPagesStandard[2].SetActive(false);
                tutoPagesStandard[3].SetActive(true);
            }
            else if (tutoPage == 3)
            {
                tutoPagesStandard[3].SetActive(false);
                tutoPagesStandard[4].SetActive(true);
                nextBtn.GetComponent<Button>().interactable = false;
            }
            tutoPage += 1;
        }
        else if (!abilityTutoBtn.GetComponent<Button>().interactable)
        {
            if (tutoPage == 2) return;
            if (tutoPage == 0)
            {
                tutoPagesAbility[0].SetActive(false);
                tutoPagesAbility[1].SetActive(true);
                prevBtn.GetComponent<Button>().interactable = true;
            }
            else if (tutoPage == 1)
            {
                tutoPagesAbility[1].SetActive(false);
                tutoPagesAbility[2].SetActive(true);
                nextBtn.GetComponent<Button>().interactable = false;
            }
            tutoPage += 1;
        }
    }
}

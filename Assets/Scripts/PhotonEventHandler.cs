using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class PhotonEventHandler : MonoBehaviour, IOnEventCallback
{
    public GameObject edgeObject;
    public GameObject gameController;
    public AudioClip defendEffect;
    public AudioClip attackEffect;
    public AudioClip iceEffect;
    public AudioClip addEdgeEffect;
    public GameObject canvasGameOver;
    public Text resultText;
    public Image feedbackBox;
    public Text feedbackText;

    private void Start()
    {
        feedbackBox.canvasRenderer.SetAlpha(0);
        feedbackText.canvasRenderer.SetAlpha(0);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 0)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animBool = (string)  contents[1];
            int state = (int) contents[2];
            int currentPlayer = (int) contents[3];
            GameObject node = GraphSpawner.nodesDict[key];
            AudioSource audSrc = gameController.GetComponent<GameController>().effectSource;

            if (animBool == "Defended") audSrc.PlayOneShot(defendEffect, audSrc.volume);
            else if (animBool == "Attacked") audSrc.PlayOneShot(attackEffect, audSrc.volume);

            node.GetComponent<Animator>().SetBool(animBool, true);
            node.GetComponent<Node>().state = state;
            GameController.currentPlayer = currentPlayer;
        }

        else if (photonEvent.Code == 1)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            string tag = (string) contents[0];
            string text = (string)  contents[1];

            GameObject textObject = GameObject.FindWithTag(tag);
            textObject.GetComponent<Text>().text = text;
            GameController.gameOver = true;
            GameController.matchStart = false;
            StartCoroutine(GameOver());
        }

        else if (photonEvent.Code == 2)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animBool = (string)  contents[1];
            int state = (int) contents[2];
            int currentPlayer = (int) contents[3];
            GameObject node = GraphSpawnerEdgeStep.nodesDict[key];
            AudioSource audSrc = gameController.GetComponent<GameControllerEdgeStep>().effectSource;

            if (animBool == "Defended") audSrc.PlayOneShot(defendEffect, audSrc.volume);
            else if (animBool == "Attacked") audSrc.PlayOneShot(attackEffect, audSrc.volume);

            node.GetComponent<Animator>().SetBool(animBool, true);
            node.GetComponent<NodeEdgeStep>().state = state;
            GameControllerEdgeStep.currentPlayer = currentPlayer;
        }

        else if (photonEvent.Code == 3)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            string tag = (string) contents[0];
            string text = (string)  contents[1];

            GameObject textObject = GameObject.FindWithTag(tag);
            textObject.GetComponent<Text>().text = text;
            GameControllerEdgeStep.gameOver = true;
            GameControllerEdgeStep.matchStart = false;
            StartCoroutine(GameOver());
        }

        else if (photonEvent.Code == 4)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int key = (int) contents[0];
            string animTag = (string)  contents[1];
            bool animBool = (bool) contents[2];
            int state = (int) contents[3];
            GameObject node = GraphSpawnerEdgeStep.nodesDict[key];
            AudioSource audSrc = gameController.GetComponent<GameControllerEdgeStep>().effectSource;
            audSrc.PlayOneShot(iceEffect, audSrc.volume);

            node.GetComponent<Animator>().SetBool(animTag, animBool);
            node.GetComponent<NodeEdgeStep>().state = state;
        }

        else if (photonEvent.Code == 5)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int firstKey = (int) contents[0];
            int secondKey = (int) contents[1];
            AudioSource audSrc = gameController.GetComponent<GameControllerEdgeStep>().effectSource;
            audSrc.PlayOneShot(addEdgeEffect, audSrc.volume);

            GameObject firstNodeObj = GraphSpawnerEdgeStep.nodesDict[firstKey];
            GameObject secondNodeObj = GraphSpawnerEdgeStep.nodesDict[secondKey];
            firstNodeObj.GetComponent<NodeEdgeStep>().childNodes.Add(secondKey);
            secondNodeObj.GetComponent<NodeEdgeStep>().parentNodes.Add(firstKey);
            CreateEdge(firstNodeObj, secondNodeObj);
        }

        else if (photonEvent.Code == 6)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int textInt = (int) contents[0];

            if (textInt == 0)
            {
                if (GameController.player != GameController.currentPlayer)
                {
                    StartCoroutine(ShowFeedbackText(textInt));
                }
            }
            else
            {
                if (GameController.player == GameController.currentPlayer)
                {
                    StartCoroutine(ShowFeedbackText(textInt));
                }
            }
        }

        else if (photonEvent.Code == 7)
        {
            object[] contents = (object[]) photonEvent.CustomData;
            int textInt = (int) contents[0];

            if (textInt == 0)
            {
                if (GameControllerEdgeStep.player != GameControllerEdgeStep.currentPlayer)
                {
                    StartCoroutine(ShowFeedbackText(textInt));
                }
            }
            else
            {
                if (GameControllerEdgeStep.player == GameControllerEdgeStep.currentPlayer)
                {
                    StartCoroutine(ShowFeedbackText(textInt));
                }
            }
        }
    }

    private void CreateEdge(GameObject childNode, GameObject parentNode)
    {
        Vector3 midpoint = CalculateEdgePosition(childNode, parentNode);
        Quaternion rotation = CalculateEdgeRotation(childNode, parentNode);
        Vector3 scale = CalculateEdgeScale(childNode, parentNode);

        GameObject edgeInstance = UnityEngine.Object.Instantiate(edgeObject, new Vector3(0, 0, 0), Quaternion.identity);
        edgeInstance.GetComponent<Renderer>().material.color = Color.green;
        edgeInstance.transform.position = midpoint;
        edgeInstance.transform.rotation = rotation;
        edgeInstance.transform.localScale = scale;
    }

    private Vector3 CalculateEdgePosition(GameObject currentObj, GameObject targetObj)
    {
        float midX = (currentObj.transform.position.x + targetObj.transform.position.x) / 2;
        float midY = (currentObj.transform.position.y + targetObj.transform.position.y) / 2;
        return new Vector3(midX, midY, currentObj.transform.position.z);
    }

    private Quaternion CalculateEdgeRotation(GameObject currentObj, GameObject targetObj)
    {
        float firstx = currentObj.transform.position.x;
        float secondx = targetObj.transform.position.x;
        float firsty = currentObj.transform.position.y;
        float secondy = targetObj.transform.position.y;

        double angle = Math.Atan((secondy-firsty) / (secondx-firstx));
        angle = angle * (180/Math.PI);

        return Quaternion.Euler(0, 0, (float)angle);;
    }

    private Vector3 CalculateEdgeScale(GameObject currentObj, GameObject targetObj)
    {
        float firstx = currentObj.transform.position.x;
        float secondx = targetObj.transform.position.x;
        float firsty = currentObj.transform.position.y;
        float secondy = targetObj.transform.position.y;

        double distance = Math.Sqrt(Math.Pow(secondy-firsty, 2) + Math.Pow(secondx-firstx, 2));

        return new Vector3((float)distance, (float)0.05, 0);
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.0f);
        if (GameController.winGame) resultText.text = "YOU WIN";
        canvasGameOver.SetActive(true);
    }

    public IEnumerator ShowFeedbackText(int textInt)
    {
        if (textInt == 0) feedbackText.text = "It is not your turn yet";
        else if (textInt == 1) feedbackText.text = "The vertex doesn't have attacked vertex as the parent";
        else if (textInt == 2) feedbackText.text = "The vertex has been defended";
        else if (textInt == 3) feedbackText.text = "The vertex has been attacked";
        else if (textInt == 4) feedbackText.text = "The vertex has been frozen for a round";
        
        feedbackBox.CrossFadeAlpha(1.0f, 1.0f, false);
        feedbackText.CrossFadeAlpha(1.0f, 1.0f, false);
        yield return new WaitForSeconds(1.75f);
        feedbackBox.CrossFadeAlpha(0.0f, 1.0f, false);
        feedbackText.CrossFadeAlpha(0.0f, 1.0f, false);
    }
}
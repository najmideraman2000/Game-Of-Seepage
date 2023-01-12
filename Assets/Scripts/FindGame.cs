using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FindGame : MonoBehaviourPunCallbacks
{
    public Text selectedRole;
    public Text selectedTime;
    public Text selectedMode;
    public GameObject choiceCanvas;
    public GameObject findCanvas;

    public void joinRoom()
    {
        choiceCanvas.SetActive(false);
        findCanvas.SetActive(true);
        Hashtable expectedCustomRoomProperties = new Hashtable{};
        if (selectedMode.text == "Normal")
        {
            if (selectedRole.text == "Defender") 
            {
                GameController.player = 0;
                expectedCustomRoomProperties.Add("role", "Attacker");
            }
            else if (selectedRole.text == "Attacker") 
            {
                GameController.player = 1;
                expectedCustomRoomProperties.Add("role", "Defender");
            }
        }
        else if (selectedMode.text == "Ability")
        {
            if (selectedRole.text == "Defender") 
            {
                GameControllerAbility.player = 0;
                expectedCustomRoomProperties.Add("role", "Attacker");
            }
            else if (selectedRole.text == "Attacker") 
            {
                GameControllerAbility.player = 1;
                expectedCustomRoomProperties.Add("role", "Defender");
            }
        }
        expectedCustomRoomProperties.Add("time", selectedTime.text);
        expectedCustomRoomProperties.Add("mode", selectedMode.text);

        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        makeRoom();
    }

    public void makeRoom()
    {
        RoomOptions roomOptions =  new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2,
        };

        Hashtable roomCustomProperties =  new Hashtable{};
        roomCustomProperties.Add("role", selectedRole.text);
        roomCustomProperties.Add("time", selectedTime.text);
        roomCustomProperties.Add("mode", selectedMode.text);
        roomOptions.CustomRoomProperties = roomCustomProperties;

        string[] customPropsForLobby = {"role", "time", "mode"};
        roomOptions.CustomRoomPropertiesForLobby = customPropsForLobby;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["mode"].ToString() == "Normal")
            {
                PhotonNetwork.LoadLevel("GameNormalMulti");
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties["mode"].ToString() == "Ability")
            {
                PhotonNetwork.LoadLevel("GameAbilityMulti");
            }
        }
    }

    // public override void OnJoinedRoom()
    // {
    //     PhotonNetwork.LoadLevel("GameAbilityMulti");
    // }
}

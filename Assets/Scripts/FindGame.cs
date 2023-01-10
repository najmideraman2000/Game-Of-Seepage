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
        Hashtable expectedCustomRoomProperties = new Hashtable {
            {"role", selectedRole.text},
            {"time", selectedTime.text},
            {"mode", selectedMode.text}
        };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
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
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2  && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameNormalMulti");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RandomLobby : MonoBehaviourPunCallbacks
{
    public Text selectedRole;
    public Text selectedTime;
    public Text selectedMode;

    public void joinRoom()
    {
        Hashtable expectedCustomRoomProperties = new Hashtable {
            {"role", selectedRole.text},
            {"time", selectedTime.text},
            {"mode", selectedMode.text}
        };
        Debug.Log(selectedMode.text);
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("Join Random Failed with error code {0} and error message {1}", returnCode, message);
        // here usually you create a new room
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
        Debug.Log("lol");

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Debug.Log(PhotonNetwork.CurrentRoom.CustomRoomProperties["role"]);
        Debug.Log("lol");
        // PhotonNetwork.AutomaticallySyncScene = true;
        // joined a room successfully, JoinRandomRoom leads here on success
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2  && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameplayMultiplayer");
        }
    }

}

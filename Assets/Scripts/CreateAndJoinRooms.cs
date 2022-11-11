using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using System.Linq;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField joinInput;
    public InputField playerNameInput;
    public static string playerName;
    public static string roomKey;

    public void createRoom()
    {
        int length = 6;
        System.Random random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        roomKey = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
        Debug.Log(roomKey);
        playerName = playerNameInput.text;
        PhotonNetwork.CreateRoom(roomKey);
    }

    public void JoinRoom()
    {
        playerName = playerNameInput.text;
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameRoom");
    }
}

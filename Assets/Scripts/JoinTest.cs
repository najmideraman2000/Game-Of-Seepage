using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using System.Linq;
using Photon.Realtime;

public class JoinTest : MonoBehaviourPunCallbacks
{
    public InputField joinInput;
    public InputField playerNameInput;
    public InputField gameNameInput;
    public static string playerName;
    public static string roomKey;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void createRoom()
    {
        int length = 6;
        System.Random random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        roomKey = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
        Debug.Log(roomKey);
        playerName = playerNameInput.text;
        PhotonNetwork.CreateRoom(gameNameInput.text);
    }

    public void JoinRoom(string input)
    {
        playerName = playerNameInput.text;
        PhotonNetwork.JoinRoom(input);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameRoom");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }
}

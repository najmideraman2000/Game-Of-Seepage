using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    JoinTest manager;

    private void Start()
    {
        manager = FindObjectOfType<JoinTest>();
    }
    public void SetRoomName(string roomname)
    {
        roomName.text = roomname;
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
}

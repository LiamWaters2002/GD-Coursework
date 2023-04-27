using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lobby : MonoBehaviourPunCallbacks
{
    public TMP_Text txtPlayerName;
    public TMP_Text txtRoomName;

    private void Awake()
    {

        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        // Create a new room: Room name = playerName.
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 6;
        PhotonNetwork.CreateRoom(txtRoomName.text, options);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void ConnectToRoom()
    {
        PhotonNetwork.NickName = txtPlayerName.text;

        PhotonNetwork.JoinRoom(txtRoomName.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = txtPlayerName.text;
        // Load the game scene when the player successfully joins the room
        if (PhotonNetwork.NickName != "")
        {
            PhotonNetwork.LoadLevel("Room");
        }
    }
}

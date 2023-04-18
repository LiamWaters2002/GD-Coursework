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
        options.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(txtRoomName.text, options);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ConnectToRoom()
    {
        PhotonNetwork.JoinRoom(txtRoomName.text);
    }

    public override void OnJoinedRoom()
    {
        // Load the game scene when the player successfully joins the room
        PhotonNetwork.NickName = txtPlayerName.text;
        PhotonNetwork.LoadLevel("Room");
    }
}

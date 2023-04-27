using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerLabel : MonoBehaviourPun
{
    public Text label;

    [SerializeField]
    private string playerName;

    public void Start()
    {
        if (photonView.IsMine)
        {
            playerName = PhotonNetwork.NickName;
            photonView.RPC("SetPlayerName", RpcTarget.AllBuffered, playerName);
        }
    }

    public void Update()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            string nicknameText = player.NickName + "\n";
            Debug.Log(nicknameText);
        }
    }

    [PunRPC]
    public void SetPlayerName(string newName)
    {
        playerName = newName;
        if (label != null)
        {
            label.text = playerName;
        }
    }

    //private void Update()
    //{
    //    if (photonView.IsMine)
    //    {
    //        if (label != null && label.text != playerName)
    //        {
    //            photonView.RPC("SetPlayerName", RpcTarget.AllBuffered, playerName);
    //        }
    //    }
    //}
}

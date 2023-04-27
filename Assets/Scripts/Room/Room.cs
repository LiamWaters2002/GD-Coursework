using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Room : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text countDownText;
    [SerializeField] private Button startButton;
    [SerializeField] private Text playerCountText;
    [SerializeField] private Text buttonText;

    private int maxPlayers = 6;
    private int playersCount = 0;
    private bool isGameReady = false;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount; 
            playerCountText.text = playerCount.ToString() + "/6";
        }


            foreach (Player player in PhotonNetwork.PlayerList)
            {
                string nicknameText = player.NickName + "\n";
                Debug.Log(nicknameText);
            }
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playersCount++;
        if (playersCount == maxPlayers)
        {
            isGameReady = true;
            StartCoroutine(StartCountdown());
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playersCount--;
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        { 
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel("Prison");
            //SceneManager.UnloadSceneAsync("Rooom");
            //photonView.RPC("SetLoadingText", RpcTarget.All);
        }
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        SceneManager.LoadScene("Lobby");
    }

    [PunRPC]
    private void SetLoadingText()
    {
        buttonText.text = "loading";
    }

    private IEnumerator StartCountdown()
    {
        int countdown = 5;
        while (countdown > 0)
        {
            countDownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        countDownText.text = "";
    }
}

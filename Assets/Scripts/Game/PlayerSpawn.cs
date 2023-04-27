using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    public GameObject[] prisonerSpawnPoints;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public Vector3 cameraOffset;

    public Text score;

    private int localPlayerIndex;

    private void Start()
    {
        cameraOffset = new Vector3(0, 0.3f, 0.1f);
        Debug.Log("playerspawn");

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RPCSpawnPlayers", RpcTarget.MasterClient);
                photonView.RPC("RPCFindPhotonView", RpcTarget.AllBuffered);
            }
        }
        else
        {
            Debug.Log("singleplayer");
            GameObject player = Instantiate(playerPrefab, prisonerSpawnPoints[0].transform.position, Quaternion.identity);
            GameObject camera = Instantiate(cameraPrefab, player.transform.position + cameraOffset, Quaternion.identity);
            camera.transform.SetParent(player.transform);
        }

    }
    //Spawn based on their index.


    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {

            int intScore = int.Parse(score.text);
            ScoreController scoreController = ScoreController.Instance;
            scoreController.SetHighScore(intScore);
            scoreController.GetHighScore();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Lobby");
        }
    }


    [PunRPC]
    void RPCSpawnPlayers()
    {
        localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            Vector3 spawnPosition = prisonerSpawnPoints[i].transform.position;
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);

            PhotonView photonView = player.GetComponent<PhotonView>();
            photonView.TransferOwnership(i + 1);
        }
    }

    [PunRPC]
    void RPCFindPhotonView()
    {
        // Get all GameObjects with "Player" tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Loop through each GameObject
        foreach (GameObject player in players)
        {
            if (player.GetPhotonView().IsMine)
            {
                GameObject camera = Instantiate(cameraPrefab, player.transform.position + cameraOffset, Quaternion.identity);
                camera.transform.SetParent(player.transform);
                camera.transform.position = camera.transform.position + cameraOffset;
                player.transform.Find("Canvas").GetComponent<Canvas>().enabled = false;

            }
        }
    }
}
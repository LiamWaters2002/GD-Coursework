using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    public GameObject[] prisonerSpawnPoints;
    public GameObject[] monsterSpawnPoint;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;
    public GameObject cameraPrefab;
    public Vector3 cameraOffset;

    private int monsterIndex;
    private int localPlayerIndex;

    private void Start()
    {
        Vector3 cameraOffset = new Vector3(0, 0.7f, 0.5f);

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                monsterIndex = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
                photonView.RPC("RPCSetMonsterIndex", RpcTarget.MasterClient, monsterIndex);
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Lobby");
        }
    }

    [PunRPC]
    void RPCSetMonsterIndex(int index)
    {
        monsterIndex = index;
        Debug.Log("This runs twice");
    }

    [PunRPC]
    void RPCSpawnPlayers()
    {
        localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (i == monsterIndex)
            {
                Vector3 spawnPosition = monsterSpawnPoint[0].transform.position; //May add more positions...
                GameObject monster = PhotonNetwork.Instantiate(monsterPrefab.name, spawnPosition, Quaternion.identity);

                PhotonView photonView = monster.GetComponent<PhotonView>();
                photonView.TransferOwnership(i + 1);
            }
            else
            {
                Vector3 spawnPosition = prisonerSpawnPoints[i].transform.position;
                GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);

                PhotonView photonView = player.GetComponent<PhotonView>();
                photonView.TransferOwnership(i + 1);
            }
        }
    }

    [PunRPC]
    void RPCFindPhotonView()
    {
        // Get all GameObjects with "Player" tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject monster = GameObject.FindGameObjectWithTag("Monster");

        if (monster.GetPhotonView().IsMine)
        {
            GameObject camera = Instantiate(cameraPrefab, monster.transform.position + cameraOffset, Quaternion.identity);
            camera.transform.SetParent(monster.transform);
        }
        // Loop through each GameObject
        foreach (GameObject player in players)
        {
            if (player.GetPhotonView().IsMine)
            {
                GameObject camera = Instantiate(cameraPrefab, player.transform.position + cameraOffset, Quaternion.identity);
                camera.transform.SetParent(player.transform);
            }
        }
    }
}
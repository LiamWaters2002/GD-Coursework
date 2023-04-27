using UnityEngine;
using System.Collections;
using Photon.Pun;

public class DoorController : MonoBehaviourPun
{

    public bool isOpen = false;
    private Ray ray;
    private RaycastHit hit;
    private float distance = 5.0f;
    public bool isDoorMoving = false;

    public AudioSource audioSource;
    public GameObject door;


    private void Update()
    {
        if (Input.GetKeyDown("e") && !isDoorMoving)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, distance))
            {
                Debug.Log(hit.collider.gameObject.name);
                Debug.Log(hit.collider.gameObject == door);
                if (hit.collider.gameObject.name.Equals(this.name) || hit.collider.gameObject == door)
                {
                    if (!isOpen)
                    {
                        audioSource.Stop();
                        door.transform.Rotate(0.0f, 90.0f, 0.0f);
                        isOpen = true;
                        photonView.RPC("SyncDoorState", RpcTarget.AllBuffered, isOpen);
                        Debug.Log("rotated");
                        audioSource.time = 0f;
                        StartCoroutine(DelayTime(3f));
                    }
                    else
                    {
                        audioSource.Stop();
                        door.transform.Rotate(0.0f, -90.0f, 0.0f);
                        isOpen = false;
                        photonView.RPC("SyncDoorState", RpcTarget.AllBuffered, isOpen);
                        Debug.Log("rotated");
                        audioSource.time = 5f;
                        audioSource.Play();
                    }
                }
            }
        }
    }

    IEnumerator DelayTime(float seconds)
    {
        audioSource.Play();
        yield return new WaitForSeconds(seconds);
        audioSource.Stop();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            stream.SendNext(isOpen);
        }
        else
        {
            isOpen = (bool)stream.ReceiveNext();
        }
    }

    public void toggleDoor()
    {
        if (isOpen)
        {
            door.transform.Rotate(0.0f, 90.0f, 0.0f);

        }
        else
        {
            door.transform.Rotate(0.0f, -90.0f, 0.0f);
        }
    }

    [PunRPC]
    void SyncDoorState(bool state)
    {
        isOpen = state;
        toggleDoor();
    }
}
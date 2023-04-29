using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public float fullHealth = 100f; // Maximum health of the player
    public float currentHealth; // Current health of the player
    private float healRate = 1.0f;
    private bool isHealing;

    private void Start()
    {
        currentHealth = fullHealth;
    }

    private void Update() 
    {
        if (currentHealth < fullHealth && !isHealing)
        {
            isHealing = true;
            StartHealing();
        }
    }


    public void TakeDamage()
    {
        if (photonView.IsMine)
        {
            currentHealth = currentHealth - 5;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        photonView.RPC("UpdateHealth", RpcTarget.Others, currentHealth);

    }

    public void Heal(float amount)
    {
        if (photonView.IsMine)
        {
            currentHealth = Mathf.Min(currentHealth + amount, fullHealth);//Keep adding until fullHealth becomes the smallest value.
        }

    }

    private void Die()
    {
        //PhotonNetwork.LeaveRoom(this);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void StartHealing()
    {
        StartCoroutine(Heal());
    }

    private IEnumerator Heal()
    {
        // Heal the player while their current health is less or equal to the maximum health
        while (currentHealth < fullHealth)
        {
            currentHealth = currentHealth + healRate;

            if (currentHealth > fullHealth)
            {
                currentHealth = fullHealth;
            }

            yield return new WaitForSeconds(1f);
        }
        isHealing = false;
    }


    [PunRPC]
    private void UpdateHealth(float health)
    {
        currentHealth = health;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HealthBar : MonoBehaviourPunCallbacks
{
    public float fullHealth = 100f; 
    public PlayerHealth playerHealth;

    public Image healthBarImage;
    public Image myHealthBar;
    public bool isUIHealthBar;

    private void Start()
    {
        if (photonView.IsMine)
        {        
            healthBarImage = GetComponent<Image>();
            myHealthBar = GameObject.Find("Canvas").transform.GetChild(0).Find("Health").GetComponent<Image>();

        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            //healthBarImage = GetComponent<Image>();
            //myHealthBar = GameObject.Find("Canvas").transform.GetChild(0).Find("Health").GetComponent<Image>();
            float currentHealth = playerHealth.GetCurrentHealth();
            healthBarImage.fillAmount = currentHealth / fullHealth;
            myHealthBar.fillAmount = currentHealth / fullHealth;
        }
        else // Update the health bar for other players
        {
            if (playerHealth != null)
            {
                float healthPercent = playerHealth.GetCurrentHealth() / playerHealth.fullHealth;
                healthBarImage.fillAmount = healthPercent;
            }
            else
            {
               // Destroy(gameObject); //destroy player
            }
        }

    }

    [PunRPC]
    private void UpdateFillAmount(float fillAmount)
    {
        // Update the fill amount on all other clients
        healthBarImage.fillAmount = fillAmount;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send the fill amount to other clients
            stream.SendNext(healthBarImage.fillAmount);
        }
        else
        {
            // Receive the fill amount and update it on the image
            float fillAmount = (float)stream.ReceiveNext();
            healthBarImage.fillAmount = fillAmount;
        }
    }
}

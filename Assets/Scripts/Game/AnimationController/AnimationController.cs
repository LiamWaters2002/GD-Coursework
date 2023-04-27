using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{ 
    public float speed = 6.0f;

    private Animator animator;
    private bool isWalking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Player is moving, isWalking will become true and play the "Walk" animation
            isWalking = true;
            animator.SetBool("isWalking", true);
        }
        else
        {
            // The player is not moving, so set the isWalking varialbe to false and play the "Idle" animation
            isWalking = false;
            animator.SetBool("isWalking", false);
        }
    }
}

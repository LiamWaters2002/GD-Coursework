using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using UnityEngine.TextCore.Text;
using System.Data;

public class FirstPersonController : MonoBehaviourPunCallbacks, IPunObservable
{

    [SerializeField] private float mouseSensitivity = 2f;

    public float speed = 5;
    public float rotationSpeed = 5;
    public float jumpSpeed = 5;

    public KeyCode flashlightKey = KeyCode.F;

    public Light flashlightLight;
    private float currentAngle = 0f;
    public float maxAngle = 45f; 
    public Transform playerTransform; 

    private bool flashlightOn = false;

    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private Camera playerCamera;
    private float cameraPitch = 0f;

    // The target position and velocity received over the network
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private float lerpSpeed;

    private bool isAttacking;




    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
        playerCamera = GetComponentInChildren<Camera>();
        flashlightLight= GetComponentInChildren<Light>();
        originalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            Vector3 movementDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                movementDirection += transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movementDirection -= transform.right;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movementDirection -= transform.forward;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movementDirection += transform.right;
            }

            // Normalize the movement direction and apply speed
            movementDirection.Normalize();
            float magnitude = movementDirection.magnitude * speed;

            // Apply gravity
            ySpeed += Physics.gravity.y * Time.deltaTime;

            // Handle jumping
            if (characterController.isGrounded)
            {
                characterController.stepOffset = originalStepOffset;
                ySpeed = -0.5f;

                if (Input.GetButtonDown("Jump"))
                {
                    ySpeed = jumpSpeed;
                }
            }
            else
            {
                characterController.stepOffset = 0;
            }

            // Apply the movement and gravity to the character controller
            Vector3 velocity = movementDirection * magnitude;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);

            // Handle mouse look
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);
            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
            flashlightLight.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);



            if (Input.GetKeyDown(flashlightKey))
            {
                flashlightOn = !flashlightOn; // Toggle the flashlight on and off
                flashlightLight.enabled = flashlightOn; // Activate or deactivate the flashlight Light component
            }

            if (flashlightOn)
            {
                // Move the flashlight up and down based on mouse movement
                float mouseMovement = Input.GetAxis("Mouse Y");
                currentAngle += mouseMovement;
            }

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("UpdatePlayerPosition", RpcTarget.OthersBuffered, transform.position, transform.rotation);
            }


            if (Input.GetMouseButtonDown(0) )
            {
                Attack();
            }

        }
        else
        {
            if (!photonView.IsMine && playerCamera)
            {
                Destroy(playerCamera);
            }
       // Apply received position and velocity
           transform.position = Vector3.Lerp(transform.position, networkPosition, 0.1f);
           transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, 0.1f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send player data over the network
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Receive player data from the network
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void UpdatePlayerPosition(Vector3 position, Quaternion rotation)
    {
        networkPosition = position;
        networkRotation = rotation;
    }

    private void Attack()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider.gameObject.CompareTag("Zombie"))
        {
            Destroy(hit.collider.gameObject);
            ScoreController scoreController = ScoreController.Instance;
            scoreController.incrementScore();

        }
    }
}
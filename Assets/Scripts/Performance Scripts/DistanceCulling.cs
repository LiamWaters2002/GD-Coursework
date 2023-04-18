//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DistanceCulling : MonoBehaviour
//{
//    public Transform playerTransform; // Reference to the player's transform
//    public float cullDistance = 0.5f; // Distance threshold for culling

//    private bool isVisible = true; // Whether the object is currently visible
//    private Renderer objectRenderer; // Reference to the object's renderer component

//    void Start()
//    {
//        objectRenderer = GetComponent<Renderer>();
//    }

//    void Update()
//    {
//        if (objectRenderer == null || playerTransform == null) return;

//        // Check if the object is within the culling distance
//        if (Vector3.Distance(transform.position, playerTransform.position) < cullDistance)
//        {
//            // The object is within the culling distance, so enable it
//            if (!isVisible)
//            {
//                objectRenderer.enabled = true;
//                isVisible = true;
//            }
//        }
//        else
//        {
//            // The object is outside the culling distance, so disable it
//            if (isVisible)
//            {
//                objectRenderer.enabled = false;
//                isVisible = false;
//            }
//        }
//    }
//}

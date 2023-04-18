//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering;

//public class FrustumCulling : MonoBehaviour
//{
//    private Camera cam;
//    private Renderer rend;

//    void Start()
//    {
//        cam = Camera.main;
//        rend = GetComponent<Renderer>();
//    }

//    void Update()
//    {
//        if (cam == null || rend == null) return;

//        // Check if the renderer is inside the camera's view frustum
//        if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), rend.bounds))
//        {
//            // The renderer is inside the view frustum, so enable it
//            rend.enabled = true;
//        }
//        else
//        {
//            // The renderer is outside the view frustum, so disable it
//            rend.enabled = false;
//        }
//    }
//}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraZoomController : MonoBehaviour
// {
//     private Camera cam;
//     private float targetZoom;
//     private float zoomFactor = 3f;
//     private float zoomLerpSpeed = 10;

//     void Start()
//     {
//         cam = Camera.main;
//         targetZoom = cam.orthographicSize;
//     }

//     void Update()
//     {
//         float scrollData;
//         scrollData = Input.GetAxis("Mouse ScrollWheel");
//         targetZoom -= scrollData * zoomFactor;
//         targetZoom = Mathf.Clamp(targetZoom, 2.0f, 12f);
//         cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
//         Debug.Log(Input.mousePosition);
//         Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         transform.position = Vector3.Lerp(transform.position,  newPosition, Time.deltaTime);
//     }
// }
// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class CameraZoomController : MonoBehaviour
// // {

// // private float zoom = 10;
// // Vector3 newPosition;

// // void Update()
// // {
// //     if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom > 9)
// //     {
// //         zoom -= 1;
// //         Camera.main.orthographicSize = zoom;
// //         newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
// //         transform.position = Vector3.Lerp(transform.position, newPosition, 0.1F);
// //     }

// //     if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom < 300)
// //     {
// //         zoom += 1;
// //         Camera.main.orthographicSize = zoom;
// //     }

// // }

// // }
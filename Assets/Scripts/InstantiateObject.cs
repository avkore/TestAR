using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhacedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]

public class InstantiateObject : MonoBehaviour
{
   [SerializeField] private GameObject prefab;
   private ARRaycastManager _arRaycastManager;
   private ARPlaneManager _arPlaneManager;
   private List<ARRaycastHit> hits = new List<ARRaycastHit>();

   private void Awake()
   {
      _arRaycastManager = GetComponent<ARRaycastManager>();
      _arPlaneManager = GetComponent<ARPlaneManager>();
   }

   private void OnEnable()
   {
      EnhacedTouch.TouchSimulation.Enable();
      EnhacedTouch.EnhancedTouchSupport.Enable();
      EnhacedTouch.Touch.onFingerDown += FingerDown;
   }
   
   private void OnDisable()
   {
      EnhacedTouch.TouchSimulation.Disable();
      EnhacedTouch.EnhancedTouchSupport.Disable();
      EnhacedTouch.Touch.onFingerDown -= FingerDown;
   }

   private void FingerDown(EnhacedTouch.Finger finger)
   {
      if (finger.index != 0) return;
      if (_arRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
      {
         foreach (ARRaycastHit hit in hits)
         {
            Pose pose = hit.pose;
            GameObject obj = Instantiate(prefab, pose.position, pose.rotation);

            if (_arPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
            {
               Vector3 position = obj.transform.position;
               Vector3 cameraPosition = Camera.main.transform.position;
               Vector3 direction = cameraPosition - position;
               Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
               Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, obj.transform.up.normalized);
               Quaternion targetRotation = Quaternion.Euler(scaledEuler);
               obj.transform.rotation *= targetRotation;
            }
         }
      }
   }
}

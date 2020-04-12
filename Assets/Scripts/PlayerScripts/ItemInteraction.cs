using System;
using UnityEngine;

namespace PlayerScripts
{
    public class ItemInteraction : MonoBehaviour
    {
        [Tooltip("If checked, debug gizmos such as boxcast will be drawn.")]
        public bool drawDebugGizmos;
        public float interactionDistance;

        private GameObject _takenGameObject;
        private Transform _mainCameraTransform;
        private Camera _mainCamera;
        private Quaternion _oldRotation;
        private Vector3 _oldScale;
        private bool _isObjTaken;
        private bool _mouseClickCaptured;

        public RaycastHit debug_hitInfo;
        public bool debug_hitDetected;

        private void Start()
        {
            _isObjTaken = false;
            _mainCamera = Camera.main;
            if (interactionDistance < 0.0f)
                interactionDistance = 3;
            if (_mainCamera != null)
                _mainCameraTransform = _mainCamera.transform;
        }

        private void OnDrawGizmos()
        {
            if (!drawDebugGizmos) return;
            if (!_isObjTaken) return;
            
            var camPos = _mainCameraTransform.position;
            var dir = _mainCameraTransform.forward;
            if (debug_hitDetected)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(camPos, dir * debug_hitInfo.distance);
                Gizmos.DrawWireCube(camPos + dir * debug_hitInfo.distance, _oldScale);
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(camPos + dir * debug_hitInfo.distance, debug_hitInfo.collider.transform.position - (camPos + dir * debug_hitInfo.distance));
                Gizmos.DrawSphere(debug_hitInfo.collider.transform.position, 0.05f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(camPos, dir * interactionDistance);
                Gizmos.DrawWireCube(camPos + dir * interactionDistance, _oldScale);
            }
        }
        
        private void FixedUpdate()
        {
            if (_isObjTaken)
            {
                if (Physics.BoxCast(_mainCameraTransform.position, _oldScale * 0.5f,
                    _mainCameraTransform.forward, out var hitInfo, _mainCameraTransform.rotation,
                    interactionDistance))
                {
                    //TODO: instead of boxcast cast ray that ends with boxcast on point - box size (idk how to math it out)
                    //TODO: split code to methods
                    debug_hitInfo = hitInfo;
                    debug_hitDetected = true;
                    if (_mouseClickCaptured)
                    {
                        _isObjTaken = false;
                        _takenGameObject.transform.position = _mainCameraTransform.position + _mainCameraTransform.forward * hitInfo.distance;
                        _takenGameObject.transform.localScale = _oldScale;
                        _takenGameObject.transform.rotation = _oldRotation;
                        _takenGameObject.transform.SetParent(null, true);
                        _takenGameObject = null;
                    }
                }
                else 
                    debug_hitDetected = false;
            }
            else if (_mouseClickCaptured)
            {
                if (Physics.Raycast(_mainCameraTransform.position, _mainCameraTransform.forward, out var hitInfo, interactionDistance))
                {
                        var debugClickHit = false;
                        
                        if (hitInfo.transform.gameObject.CompareTag("Takable"))
                        {
                            debugClickHit = true;
                            _isObjTaken = true;
                            _oldScale = hitInfo.transform.localScale;
                            _oldRotation = hitInfo.transform.rotation;
                            _takenGameObject = hitInfo.transform.gameObject;
                            hitInfo.transform.position = new Vector3(0.0f, -0.25f, 0.42f);
                            hitInfo.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            hitInfo.transform.rotation = new Quaternion();
                            hitInfo.transform.SetParent(_mainCamera.transform, false);
                        }

                        if (drawDebugGizmos)
                            Debug.DrawRay(_mainCameraTransform.position, _mainCameraTransform.forward * interactionDistance,
                                debugClickHit ? Color.green : Color.magenta, 1.5f);
                }
            }
            _mouseClickCaptured = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseClickCaptured = true;
            }
        }
    }
}

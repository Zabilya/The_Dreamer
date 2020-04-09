using System;
using UnityEngine;

namespace PlayerScripts
{
    public class ItemInteraction : MonoBehaviour
    {
        public float interactionDistance;

        private GameObject _takenGameObject;
        private GameObject _player;
        private Transform _mainCameraTransform;
        private Camera _mainCamera;
        private Quaternion _oldRotation;
        private Vector3 _oldScale;
        private bool _isObjTaken;
        private bool _ableToMoveObjects;

        private void Start()
        {
            _player = GameObject.Find("Player");
            _isObjTaken = false;
            _mainCamera = Camera.main;
            if (interactionDistance < 0.0f)
                interactionDistance = 3;
            if (_mainCamera != null)
                _mainCameraTransform = _mainCamera.transform;
        }

        private void FixedUpdate()
        {
            if (_ableToMoveObjects)
            {
                if (_isObjTaken)
                {
                    var playerTransform = _player.transform.position;
                    
                    _isObjTaken = false;
                    _takenGameObject.transform.position = _mainCameraTransform.position + _mainCameraTransform.forward * interactionDistance;
                    _takenGameObject.transform.localScale = _oldScale;
                    _takenGameObject.transform.rotation = _oldRotation;
                    _takenGameObject.transform.SetParent(null, true);
                    _takenGameObject = null;
                    _player.transform.position = playerTransform;
                }
                else
                {
                    var orig = _mainCameraTransform.position;

                    // Debug.DrawRay(orig, camTrans.forward, Color.magenta, 1.0f);
                    
                    if (Physics.Raycast(orig, _mainCameraTransform.forward, out var hitInfo, interactionDistance))
                    {
                        // Debug.DrawRay(orig, end, Color.red);
                        if (hitInfo.transform.gameObject.CompareTag("Takable"))
                        {
                            _isObjTaken = true;
                            _oldScale = hitInfo.transform.localScale;
                            _oldRotation = hitInfo.transform.rotation;
                            _takenGameObject = hitInfo.transform.gameObject;
                            hitInfo.transform.position = new Vector3(0.0f, -0.25f, 0.42f);
                            hitInfo.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            hitInfo.transform.rotation = new Quaternion();
                            hitInfo.transform.SetParent(_mainCamera.transform, false);
                        }
                    }
                }

                _ableToMoveObjects = false;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ableToMoveObjects = true;
                /*
                if (_isObjTaken)
                {
                    var playerTransform = _player.transform.position;
                    
                    _isObjTaken = false;
                    _takenGameObject.transform.position = _mainCameraTransform.position + _mainCameraTransform.forward * interactionDistance;
                    _takenGameObject.transform.localScale = _oldScale;
                    _takenGameObject.transform.rotation = _oldRotation;
                    _takenGameObject.transform.SetParent(null, true);
                    _takenGameObject = null;
                    _player.transform.position = playerTransform;
                }
                else
                {
                    var orig = _mainCameraTransform.position;

                    // Debug.DrawRay(orig, camTrans.forward, Color.magenta, 1.0f);
                    
                    if (Physics.Raycast(orig, _mainCameraTransform.forward, out var hitInfo, interactionDistance))
                    {
                        // Debug.DrawRay(orig, end, Color.red);
                        if (hitInfo.transform.gameObject.CompareTag("Takable"))
                        {
                            _isObjTaken = true;
                            _oldScale = hitInfo.transform.localScale;
                            _oldRotation = hitInfo.transform.rotation;
                            _takenGameObject = hitInfo.transform.gameObject;
                            hitInfo.transform.position = new Vector3(0.0f, -0.25f, 0.42f);
                            hitInfo.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            hitInfo.transform.rotation = new Quaternion();
                            hitInfo.transform.SetParent(_mainCamera.transform, false);
                        }
                    }
                }
                */
            }
        }
    }
}

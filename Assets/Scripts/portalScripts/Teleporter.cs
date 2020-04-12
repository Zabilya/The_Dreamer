using System;
using System.Collections;
using UnityEngine;

namespace portalScripts
{
    public class Teleporter : MonoBehaviour
    {
        private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        public Teleporter otherTeleporter;
        public GameObject player;
        public bool isTeleported;
        private Camera mainCamera;
        private CameraClearFlags _clearFlags;
        private int _cullingMask;
        private bool _isTp;
        private bool _isFreezed;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("Player");
            isTeleported = false;
            mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            // if (_isFreezed)
            // {
            //     UnFreezeCamera();
            //     _isFreezed = false;
            //     _isTp = false;
            // }
            
            if (isTeleported)
            {
                player.GetComponent<PlayerController>().enabled = true;
                isTeleported = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            float zPos = transform.InverseTransformPoint(other.transform.position).z;
            // if (zPos < 0.08f && !_isFreezed)
            // { 
            //     FreezeCamera();
            //     _isFreezed = true;
            // }
            if (zPos < 0.0f && zPos > -0.4f)
            {
                player.GetComponent<PlayerController>().enabled = false;
                Teleport(player.GetComponent<Transform>());
                isTeleported = true;
                _isTp = true;
            }
        }

        // private void OnTriggerExit(Collider other)
        // {
        //     var position = GetComponent<Transform>().position;
        //     float zPos = transform.InverseTransformPoint(other.transform.position).z;
        // }

        private void FreezeCamera()
        {
            _clearFlags = mainCamera.clearFlags;
            mainCamera.clearFlags = CameraClearFlags.Nothing;
            _cullingMask = mainCamera.cullingMask;
            mainCamera.cullingMask = 0;
        }

        private void UnFreezeCamera()
        {
            mainCamera.clearFlags = _clearFlags;
            mainCamera.cullingMask = _cullingMask;
        }

        private void Teleport(Transform objTrans)
        {
            // Position
            // Vector3 localPos = transform.worldToLocalMatrix.MultiplyPoint3x4(objTrans.position);
            // localPos = new Vector3(-localPos.x, localPos.y, -localPos.z);
            // objTrans.position = otherTeleporter.transform.localToWorldMatrix.MultiplyPoint3x4(localPos);
            
            
            // // Rotation
            // Quaternion difference = transform.rotation * Quaternion.Inverse(otherTeleporter.transform.rotation * 
            //                                                                Quaternion.Euler(0, 180, 0)); 
            // objTrans.rotation = difference * objTrans.rotation;
            
            // Update position of object.
            Vector3 relativePos = transform.InverseTransformPoint(objTrans.position);
            relativePos = halfTurn * relativePos;
            objTrans.position = otherTeleporter.transform.TransformPoint(relativePos);

            // Update rotation of object.
            Quaternion relativeRot = Quaternion.Inverse(transform.rotation) * objTrans.rotation;
            relativeRot = halfTurn * relativeRot;
            objTrans.rotation = otherTeleporter.transform.rotation * relativeRot;
        }
    }
}

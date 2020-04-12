using System;
using PlayerScripts;
using UnityEngine;

namespace portalScripts
{
    public class Teleporter : MonoBehaviour
    {
        private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        public Teleporter otherTeleporter;
        public GameObject player;
        public bool isTeleported;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("Player");
            isTeleported = false;
        }

        private void FixedUpdate()
        {
            if (isTeleported)
            {
                player.GetComponent<PlayerController>().enabled = true;
                isTeleported = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            float zPos = transform.worldToLocalMatrix.MultiplyPoint3x4(other.transform.position).z;

            if (zPos < 0)
            {
                player.GetComponent<PlayerController>().enabled = false;
                Teleport(player.GetComponent<Transform>());
                isTeleported = true;
            }
        }

        private void Teleport(Transform objTrans)
        {
            // Position
            Vector3 localPos = transform.worldToLocalMatrix.MultiplyPoint3x4(objTrans.position);
            localPos = new Vector3(-localPos.x, localPos.y, -localPos.z);
            objTrans.position = otherTeleporter.transform.localToWorldMatrix.MultiplyPoint3x4(localPos);
            
            
            // // Rotation
            // Quaternion difference = transform.rotation * Quaternion.Inverse(otherTeleporter.transform.rotation * 
            //                                                                Quaternion.Euler(0, 180, 0)); 
            // objTrans.rotation = difference * objTrans.rotation;
            
            // Update position of object.
            // Vector3 relativePos = transform.InverseTransformPoint(objTrans.position);
            // relativePos = halfTurn * relativePos;
            // objTrans.position = otherTeleporter.transform.TransformPoint(relativePos);

            // Update rotation of object.
            Quaternion relativeRot = Quaternion.Inverse(transform.rotation) * objTrans.rotation;
            relativeRot = halfTurn * relativeRot;
            objTrans.rotation = otherTeleporter.transform.rotation * relativeRot;
        }
    }
}

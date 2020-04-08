using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteraction : MonoBehaviour
{
    public float interactionDistance;
    void Start()
    {
        if (interactionDistance < 0.0f)
            interactionDistance = 3;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                var camTrans = Camera.main.transform;
                var camPos = camTrans.position;
                var tmp = camTrans.forward * 3;
                var orig = camPos;
                var end = orig + tmp;

                if (Physics.Raycast(orig, end, (end - orig).magnitude))
                {
                    Debug.DrawRay(orig, end, Color.red);
                }
                // var ray = new Ray(camPos, Vector3.forward);
                // var hit = new RaycastHit();
                
            }
        }
    }
}

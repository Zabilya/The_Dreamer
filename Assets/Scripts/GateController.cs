using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public GateManager gateManager;
    public Vector3 scaleValue;
    public bool collidesPlayer;
    public int gateIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collidesPlayer = true;
            if (gateIndex == 0 && !gateManager.Gates[1].collidesPlayer)
                gateManager.UpdateGatesScaleValue(other.transform.localScale);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collidesPlayer = false;
            other.transform.localScale = scaleValue;
            
            //TODO: reset gates after leaving tunnel or smth else.
        }
    }
}

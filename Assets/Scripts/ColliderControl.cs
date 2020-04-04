using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("GameObject entered " + this.name);
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("GameObject left " + this.name);
    }
}

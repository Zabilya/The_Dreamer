using System;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    [Tooltip("By this percent player's original size should be resized.")]
    [SerializeField]
    private float _scaleFactor;
    [Tooltip("Determines how smooth should scaling be applied. More gates means more smoothness. Minimum gates count: 2")]
    [SerializeField]
    private int gatesCount;
    public List<GateController> Gates { get; private set; }

    private Transform _holderTransform;
    private Vector3 _ceilingScale;
    private Vector3 _wallScale;
    private float _tunnelHeight;
    private float _tunnelWidth;

    private void Start()
    {
        if (gatesCount < 2)
            gatesCount = 2;
        if (_scaleFactor <= 0.0f)
            _scaleFactor = 0.5f;
        
        _holderTransform = this.transform;
        _ceilingScale = _holderTransform.Find("Ceiling").localScale;
        _wallScale = _holderTransform.Find("LeftWall").localScale;
        _tunnelHeight = _wallScale.y;
        _tunnelWidth = _ceilingScale.x - _wallScale.x * 2;
        Gates = new List<GateController>();

        CreateGates();
    }

    //TODO: Method could be used as collider recreation after tunnel's parameters changing.
    private void CreateGates()
    {
        if (gatesCount > 0)
            Gates.Clear();
        for (var i = 0; i < gatesCount; i++)
        {
            var oldObj = _holderTransform.Find("Collider" + i);
            var obj = new GameObject("Collider" + i);
            var newCollider = obj.AddComponent<BoxCollider>();
            var objScript = obj.AddComponent<GateController>(); 
            var colliderLength = 1.0f / gatesCount;
            
            if (oldObj)
                GameObject.Destroy(oldObj);
            objScript.gateIndex = i;
            objScript.gateManager = this;
            newCollider.size = new Vector3(_tunnelWidth, _tunnelHeight,colliderLength);
            newCollider.isTrigger = true;
            obj.transform.position = new Vector3(0, 0, colliderLength * (i - 0.5f * (gatesCount - 1))); //i * colliderLength - (colliderLength * colliderCount * 0.5f) + colliderLength * 0.5f);
            obj.transform.SetParent(_holderTransform, false);
            Gates.Add(objScript); 
        }
    }

    public void UpdateGatesScaleValue(Vector3 startSize)
    {
        var x = startSize.x * _scaleFactor / (gatesCount - 1);
        var y = startSize.y * _scaleFactor / (gatesCount - 1);
        var z = startSize.z * _scaleFactor / (gatesCount - 1);
        
        var resizeStep = new Vector3(x, y, z);
        Debug.Log(resizeStep.ToString()); //TODO: excuse me what the fuck

        Gates[0].scaleValue = startSize;
        for (var i = 1; i < Gates.Count; i++)
        {
            Gates[i].scaleValue.x = startSize.x - x * i;
            Gates[i].scaleValue.y = startSize.y - y * i;
            Gates[i].scaleValue.z = startSize.z - z * i;
        }
    }
}
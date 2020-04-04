using System;
using UnityEngine;

public class ColliderCreator : MonoBehaviour
{
    [SerializeField]
    private int colliderCount;
    [SerializeField]
    private int sizeChangePerStep;

    private Transform _holderTransform;
    private Vector3 _ceilingScale;
    private Vector3 _wallScale;
    private float _tunnelHeight;
    private float _tunnelWidth;
    
    private void Start()
    {
        if (colliderCount == 0)
            colliderCount = 10;
        if (sizeChangePerStep == 0)
            sizeChangePerStep = 2;
        
        _holderTransform = this.transform;
        _ceilingScale = _holderTransform.Find("Ceiling").localScale;
        _wallScale = _holderTransform.Find("LeftWall").localScale;
        _tunnelHeight = _wallScale.y;
        _tunnelWidth = _ceilingScale.x - _wallScale.x * 2;

        CreateNewColliders();
    }

    private void CreateNewColliders()
    {
        for (var i = 0; i < colliderCount; i++)
        {
            var oldObj = _holderTransform.Find("Collider" + i);
            var obj = new GameObject("Collider" + i);
            var newCollider = obj.AddComponent<BoxCollider>();
            var colliderLength = 1.0f / colliderCount;
            
            newCollider.size = new Vector3(_tunnelWidth, _tunnelHeight,colliderLength);
            newCollider.isTrigger = true;
            if (oldObj)
                GameObject.Destroy(oldObj);
            obj.transform.position = new Vector3(0, 0, colliderLength * (i - 0.5f * (colliderCount - 1))); //i * colliderLength - (colliderLength * colliderCount * 0.5f) + colliderLength * 0.5f);
            obj.AddComponent<ColliderControl>();
            obj.transform.SetParent(_holderTransform, false);
        }
    }
}
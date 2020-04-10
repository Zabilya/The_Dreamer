using System.Collections.Generic;
using UnityEngine;

namespace portalScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class NewPortal : MonoBehaviour
    {
        [SerializeField] 
        private NewPortal otherPortal;

        private bool isPlaced = true;

        // private List<PortalableObject> portalObjects = new List<PortalableObject>();

        private Material material;

        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponentInChildren<Renderer>();
            material = renderer.material;
        }

        private void Update()
        {
            // for (int i = 0; i < portalObjects.Count; ++i)
            // {
            //     Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);
            //
            //     if (objPos.z > 0.0f)
            //     {
            //         portalObjects[i].Warp();
            //     }
            // }
        }

        public NewPortal GetOtherPortal()
        {
            return otherPortal;
        }

        public void SetTexture(RenderTexture tex)
        {
            material.mainTexture = tex;
        }

        public bool IsRendererVisible()
        {
            // return renderer.isVisible;
            return renderer.IsVisibleFrom(Camera.main);
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     var obj = other.GetComponent<PortalableObject>();
        //     if (obj != null)
        //     {
        //         portalObjects.Add(obj);
        //         obj.SetIsInPortal(this, otherPortal, wallCollider);
        //     }
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     var obj = other.GetComponent<PortalableObject>();
        //
        //     if(portalObjects.Contains(obj))
        //     {
        //         portalObjects.Remove(obj);
        //         obj.ExitPortal(wallCollider);
        //     }
        // }

        public bool IsPlaced()
        {
            return isPlaced;
        }
    }
}
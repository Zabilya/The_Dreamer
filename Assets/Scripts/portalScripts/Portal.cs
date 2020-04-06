using UnityEngine;

namespace portalScripts
{
    public class Portal : MonoBehaviour
    {
        public Portal otherPortal;
        public Camera portalView;
    
        // Start is called before the first frame update
        void Start()
        {
            otherPortal.portalView.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            GetComponent<MeshRenderer>().sharedMaterial.mainTexture = otherPortal.portalView.targetTexture;
        }

        // Update is called once per frame
        void Update()
        {
            // Position
            Vector3 playerPosition =
                otherPortal.transform.worldToLocalMatrix.MultiplyPoint3x4(Camera.main.transform.position);
            portalView.transform.localPosition = -playerPosition;
            
            // Rotation
            Quaternion difference = transform.rotation * 
                                    Quaternion.Inverse(otherPortal.transform.rotation * Quaternion.Euler(0, 180, 0));
            portalView.transform.rotation = difference * Camera.main.transform.rotation;
            
            // Clipping
            portalView.nearClipPlane = playerPosition.magnitude;
        }
    }
}

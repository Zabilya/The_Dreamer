using UnityEngine;

namespace portalScripts
{
    public class RecursivePortalCamera : MonoBehaviour
    {
        [SerializeField] 
        private NewPortal[] portals = new NewPortal[2];
        
        public Camera portalCamera;
        public Camera mainCamera;
        public Camera otherPortalCamera;

        private RenderTexture tempTexture1;
        private RenderTexture tempTexture2;

        private const int iterations = 3;

        private void Awake()
        {
            mainCamera = Camera.main;
            portalCamera = GetComponent<Camera>();

            tempTexture1 = new RenderTexture(Screen.width, Screen.height, 24);
            tempTexture2 = new RenderTexture(Screen.width, Screen.height, 24);
        }

        private void Start()
        {
            portals[0].SetTexture(tempTexture1);
            portals[1].SetTexture(tempTexture2);
        }

        private void OnPreRender()
        {
            if (!portals[0].IsPlaced() || !portals[1].IsPlaced())
            {
                return;
            }
            if (portals[0].IsRendererVisible(Camera.main))
            {
                portalCamera.targetTexture = tempTexture1;
                for (int i = iterations - 1; i >= 0; --i)
                {
                    RenderCamera(Camera.main, portals[0], portals[1], i);
                }
            }
            else
            {
                if (otherPortalCamera != null)
                {
                    if (portals[0].IsRendererVisible(otherPortalCamera))
                    {
                        portalCamera.targetTexture = tempTexture1;
                        for (int i = iterations - 1; i >= 0; --i)
                        {
                            RenderCamera(otherPortalCamera, portals[0], portals[1], i);
                        }
                    }
                }
            }

            if (portals[1].IsRendererVisible(Camera.main))
            {
                portalCamera.targetTexture = tempTexture2;
                for (int i = iterations - 1; i >= 0; --i)
                {
                    RenderCamera(Camera.main, portals[1], portals[0], i);
                }
            }
            else
            {
                if (otherPortalCamera != null)
                {
                    if (portals[1].IsRendererVisible(otherPortalCamera))
                    {
                        portalCamera.targetTexture = tempTexture2;
                        for (int i = iterations - 1; i >= 0; --i)
                        {
                            RenderCamera(otherPortalCamera, portals[1], portals[0], i);
                        }
                    }
                }
            }
        }

        private void RenderCamera(Camera cam, NewPortal inPortal, NewPortal outPortal, int iterationID)
        {
            Transform inTransform = inPortal.transform;
            Transform outTransform = outPortal.transform;
            Transform mainCameraTransform = cam.transform;
            Transform portalCameraTransform = portalCamera.transform;
            
            portalCameraTransform.position = mainCameraTransform.position;
            portalCameraTransform.rotation = mainCameraTransform.rotation;

            for (int i = 0; i <= iterationID; ++i)
            {
                // Position the camera behind the other portal.
                Vector3 relativePos = inTransform.InverseTransformPoint(portalCameraTransform.position);
                relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
                portalCameraTransform.position = outTransform.TransformPoint(relativePos);

                // Position
                // Vector3 playerPosition =
                //     outTransform.worldToLocalMatrix.MultiplyPoint3x4(Camera.main.transform.position);
                // playerPosition = new Vector3(-playerPosition.x, playerPosition.y, -playerPosition.z);
                // cameraTransform.localPosition = playerPosition;
                

                // Rotate the camera to look through the other portal.
                Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * portalCameraTransform.rotation;
                relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
                portalCameraTransform.rotation = outTransform.rotation * relativeRot;

                // Clipping
                // portalCamera.nearClipPlane = playerPosition.magnitude;
            }
            
            // Set the camera's oblique view frustum.
            Plane p = new Plane(outTransform.forward, outTransform.position);
            Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
            Vector4 clipPlaneCameraSpace =
                Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlane;
            
            var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
            portalCamera.projectionMatrix = newMatrix;

            // Render the camera to its render target.
            portalCamera.Render();
        }
    }
}
using UnityEngine;

namespace PlayerScripts
{
    public class IsInCameraView : MonoBehaviour
    {
        public bool isInView;
        private Renderer _objRenderer;
    
        // Start is called before the first frame update
        void Start()
        {
            _objRenderer = this.gameObject.GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_objRenderer.IsVisibleFrom(Camera.main))
            {
                isInView = true;
            }
        }
    }
}

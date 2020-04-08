using UnityEditor.PackageManager;
using UnityEngine;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

namespace PlayerScripts
{
    public class CrosshairController : MonoBehaviour
    {
        public GameObject crosshairObject;
        public Image crosshairImage;
        public Color defaultColor = Color.white;
        public Color activeColor = Color.green;
        public float spreadPerSecond = 2.0f;
        public float maxSpread = 0.6f;
        public float minSpread = 0.2f;

        private ItemInteraction _interactionScript;

        private void Start ()
        {
            Cursor.visible = false;
            if (crosshairObject == null)
                crosshairObject = GameObject.Find("CrosshairObject");
            if (crosshairImage == null)
                crosshairObject.GetComponent<Image>();
            _interactionScript = this.GetComponent<ItemInteraction>();
        }

        private void OnGUI()
        {
            if (Camera.main == null) return;
            
            var camTrans = Camera.main.transform;
            var camPos = camTrans.position;
            var tmpVec = crosshairObject.transform.localScale;

            if (Physics.Raycast(camPos, camTrans.forward, out var hitInfo, _interactionScript.interactionDistance))
            {
                if (hitInfo.transform.gameObject.CompareTag("Takable"))
                {
                    if (tmpVec.x < maxSpread)
                    {
                        tmpVec.x += spreadPerSecond * Time.deltaTime;
                        tmpVec.y += spreadPerSecond * Time.deltaTime;
                        tmpVec.z += spreadPerSecond * Time.deltaTime;
                        if (crosshairImage.color != activeColor)
                            crosshairImage.color = activeColor;
                    }
                }
            }
            else if (tmpVec.x > minSpread)
            {
                tmpVec.x -= spreadPerSecond * Time.deltaTime;
                tmpVec.y -= spreadPerSecond * Time.deltaTime;
                tmpVec.z -= spreadPerSecond * Time.deltaTime;
                if (crosshairImage.color != defaultColor)
                    crosshairImage.color = defaultColor;
            }
            if (crosshairObject.transform.localScale != tmpVec)
                crosshairObject.transform.localScale = tmpVec;
        }

        private void Fire() { }
    }
}

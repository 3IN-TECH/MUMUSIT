using UnityEngine;

namespace metaverse_template
{

    /// <summary>
    /// Managed the control of the camera using the mouse
    /// </summary>
    public class NotVRCameraControl : MonoBehaviour
    {
        const float _mouseSensibilityAndroid = 90;
        const float _mouseSensibilityPC = 140;
        const float maxVelocity = 10;
        public float mouseSensibility
        {
            get
            {
#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS
                return _mouseSensibilityPC;
#else
            return _mouseSensibilityAndroid;
#endif
            }
        }
        Transform playerBody;
        float xRotation = 0;

        [SerializeField] MobileJoystickController rightJoystick;

        private void Start()
        {
            playerBody = PlayerLimbFinder.Player().transform;
#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS
            rightJoystick.gameObject.SetActive(false);
#endif
        }

        void Update()
        {
#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS
            float mouseX = 0;
            float mouseY = 0;
            if (Input.GetMouseButton(1))
            {
                mouseX = Input.GetAxis("Mouse X") % maxVelocity * mouseSensibility * Time.deltaTime;
                mouseY = Input.GetAxis("Mouse Y") % maxVelocity * mouseSensibility * Time.deltaTime;
            }
#else
        float mouseX = rightJoystick.pointPosition.x * mouseSensibility * Time.deltaTime;
        float mouseY = rightJoystick.pointPosition.y * mouseSensibility * Time.deltaTime;
#endif
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);

            // Rotational movement by touch
            /*Vector3 desde = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 hacia = new Vector3(rightJoystick.pointPosition.x, 0.0f, rightJoystick.pointPosition.y);
            float angulo = Vector3.SignedAngle(desde, hacia, Vector3.up);
            transform.eulerAngles = Vector3.up * Mathf.LerpAngle(transform.eulerAngles.y, angulo, smoothTime);*/
        }
#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS
        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
        }
#endif
    }
}
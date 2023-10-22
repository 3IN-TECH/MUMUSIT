using UnityEngine;

namespace metaverse_template
{

    /// <summary>
    /// Managed the movement with de wasd keyboard
    /// </summary>
    public class MovementPlayer : MonoBehaviour
    {
        [Tooltip("Can you moved at start?")]
        [SerializeField] bool initialCanMove = true;

        [Tooltip("GameObject where is the Canvas, parent of the MobileJoystickController")]
        [SerializeField] GameObject canvasJoystick;

        [Tooltip("Joystick that will control the movement of the player move")]
        [SerializeField] MobileJoystickController joystickToMove;

        NetworkingAnimator networkingAnimator;

        bool _canMove = true;
        public bool canMove
        {
            get
            {
                return _canMove;
            }

            set
            {
                _canMove = value;
                joystickToMove.gameObject.SetActive(value);
            }
        }

        Transform playerTransform;
        const float speed = 2.5f;
        const float backMovModifier = 0.6f;

        void Start()
        {
            playerTransform = PlayerLimbFinder.Player().transform;
            canMove = initialCanMove;

            // Deshabilitar el joystick al entrar en la sala si eres PC, habilitarlo para android
#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS
            canvasJoystick.SetActive(false);
#else
        canvasJoystick.SetActive(true);
#endif
        }

        void Update()
        {
            if (canMove)
            {
                Vector3 direction = Vector3.zero;
#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS
                direction.x = Input.GetAxis("Horizontal");
                direction.y = Input.GetAxis("Vertical");
#else
             
            //Movimiento 
            direction.x = joystickToMove.pointPosition.x;
            direction.y = joystickToMove.pointPosition.y;
#endif

                if (direction.Equals(Vector3.zero))
                {
                    if (networkingAnimator)
                    {
                        networkingAnimator.SetAnimation(0);
                    }
                }
                else if (direction.y < 0)
                {
                    //Backward
                    direction = direction.y * this.transform.forward + direction.x * this.transform.right;

                    if (networkingAnimator)
                    {
                        networkingAnimator.SetAnimation(2);
                    }

                    direction.y = 0;
                    playerTransform.transform.position += direction.normalized * speed * backMovModifier * Time.deltaTime;
                }
                else
                {
                    //Forward
                    direction = direction.y * this.transform.forward + direction.x * this.transform.right;

                    if (networkingAnimator)
                    {
                        networkingAnimator.SetAnimation(1);
                    }

                    direction.y = 0;
                    playerTransform.transform.position += direction.normalized * speed * Time.deltaTime;
                }

            }

#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
#endif
        }

        public void SetNetworkingAnimator(NetworkingAnimator newAnimator)
        {
            networkingAnimator = newAnimator;
        }
    }
}

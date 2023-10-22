using UnityEngine;

namespace metaverse_template
{

    public class PlayerLimbFinder : MonoBehaviour
    {
        [Tooltip("GameObject of the player")]
        [SerializeField] GameObject player;
        static GameObject _playerstatic;

        [Tooltip("Transform where is the TrackedPoseDriver of the head or the NotVRCameraControl")]
        [SerializeField] GameObject head;
        static GameObject _head;

        [Tooltip("Transform where is the XRController of the left hand")]
        [SerializeField] GameObject lefthand;
        static GameObject _lefthand;

        [Tooltip("Transform where is the XRController of the right hand")]
        [SerializeField] GameObject righthand;
        static GameObject _righthand;


        void Awake()
        {
            _playerstatic = player;
            _head = head;
            _lefthand = lefthand;
            _righthand = righthand;
        }

        public static GameObject Player()
        {
            return _playerstatic;
        }

        public static GameObject Head()
        {
            return _head;
        }

        public static GameObject LeftHand()
        {
            return _lefthand;
        }

        public static GameObject RightHand()
        {
            return _righthand;
        }

    }
}
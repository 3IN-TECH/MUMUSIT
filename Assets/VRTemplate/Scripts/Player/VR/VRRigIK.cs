using UnityEngine;

namespace metaverse_template
{

    public class VRMap
    {
        public Transform vrTarget;
        public Transform rigTarget;
        public Vector3 trackingPositionOffset;
        public Vector3 trackingRotationOffset;

        public void Map()
        {
            rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }

    }

    /// <summary>
    /// Makes the IK system to follow the vr tracking
    /// </summary>
    public class VRRigIK : MonoBehaviour
    {
        public VRMap head;
        public VRMap leftHand;
        public VRMap rightHand;

        Transform avatarTransform;
        Transform headConstraint;

        Vector3 headBodyOffset;

        public void SetAvatarIK(Transform _avatarTransform, Transform _headConstraints, Transform lHandConstraints, Transform rHandConstraints)
        {
            avatarTransform = _avatarTransform;
            headConstraint = _headConstraints;
            headBodyOffset = this.transform.position - headConstraint.position;
            this.enabled = true;
            head = new VRMap();
            leftHand = new VRMap();
            rightHand = new VRMap();
            head.vrTarget = Camera.main.transform;
            head.rigTarget = _headConstraints;
            leftHand.vrTarget = PlayerLimbFinder.LeftHand().transform;
            leftHand.rigTarget = lHandConstraints;
            leftHand.trackingRotationOffset = new Vector3(0, 90, 90);
            rightHand.vrTarget = PlayerLimbFinder.RightHand().transform;
            rightHand.rigTarget = rHandConstraints;
            rightHand.trackingRotationOffset = new Vector3(0, -90, -90);
        }

        void LateUpdate()
        {
            if (!headConstraint)
            {
                this.enabled = false;
                return;
            }
            avatarTransform.position = headConstraint.position + headBodyOffset;
            avatarTransform.forward = Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized;

            head.Map();
            leftHand.Map();
            rightHand.Map();

        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace metaverse_template
{

    /// <summary>
    /// Contain the logic to select a avatar and a mirror system to see your selected avatar
    /// </summary>
    public class AvatarCanvas : MonoBehaviour
    {
        [Tooltip("Transform when place the mirror avatar GameObject")]
        [SerializeField] Transform mirrorAvatarParent;

        [Tooltip("List of the Avatar prefabs that you can select")]
        [SerializeField] List<GameObject> avatarPrefabs;

        [Tooltip("List of the Buttons of the canvas. Used to disabled while the changing of avatar is hapening")]
        [SerializeField] List<Button> buttonsInCanvas;

        [Tooltip("List of the Buttons of the canvas. Used to changes the avatar order like the avatarPrefabs list")]
        [SerializeField] List<Button> buttonsToChangeAvatar;

        /// <summary>
        /// GameObject of the player
        /// </summary>
        GameObject localController;

        /// <summary>
        /// Aux value to have the avatar selected index of the avatarPrefabs
        /// </summary>
        int avatarIndex = -1;

        /// <summary>
        /// Avatar that you have equiped
        /// </summary>
        GameObject avatar;

        /// <summary>
        /// Avatar that copy your movements
        /// </summary>
        GameObject mirrorAvatar;


        //Variables used to cupy the tracking of the avatar to the mirror avatar
        Transform headAvatarPlayer;
        Transform headAvatarMirror;
        Transform leftHandAvatarPlayer;
        Transform leftHandAvatarMirror;
        Transform rightHandAvatarPlayer;
        Transform rightHandAvatarMirror;

        [Header("Offset for mirror")]
        [Tooltip("Variables used to invert the axes to get the feeling that you are looking in the mirror. By default it inverts the player on the Z-axis")]
        [SerializeField] Vector3 offsetMirror = Vector3.zero;
        [SerializeField] Vector3 offsetRotationMirror = Vector3.up * 180;
        [SerializeField] Vector3 offsetRotationLeftHandMirror = Vector3.one * 180;
        [SerializeField] Vector3 offsetRotationRightHandMirror = Vector3.one * 180;

        private void Start()
        {
            localController = PlayerLimbFinder.Player();
            ChangeAvatar(0);
            LoadButtons();
        }

        //Update the tracking of the mirror avatar
        void Update()
        {
            if (mirrorAvatar != null)
            {
                headAvatarMirror.localEulerAngles = new Vector3(headAvatarPlayer.localEulerAngles.x, -headAvatarPlayer.localEulerAngles.y, -headAvatarPlayer.localEulerAngles.z);

                leftHandAvatarMirror.localPosition = new Vector3(leftHandAvatarPlayer.localPosition.x, leftHandAvatarPlayer.localPosition.y, leftHandAvatarPlayer.localPosition.z) + offsetMirror;
                leftHandAvatarMirror.localEulerAngles = new Vector3(-leftHandAvatarPlayer.eulerAngles.x, leftHandAvatarPlayer.eulerAngles.y, leftHandAvatarPlayer.eulerAngles.z) + offsetRotationLeftHandMirror;

                rightHandAvatarMirror.localPosition = new Vector3(rightHandAvatarPlayer.localPosition.x, rightHandAvatarPlayer.localPosition.y, rightHandAvatarPlayer.localPosition.z) + offsetMirror;
                rightHandAvatarMirror.localEulerAngles = new Vector3(-rightHandAvatarPlayer.localEulerAngles.x, rightHandAvatarPlayer.localEulerAngles.y, rightHandAvatarPlayer.localEulerAngles.z) + offsetRotationRightHandMirror;

                mirrorAvatar.transform.localPosition = new Vector3(avatar.transform.localPosition.x, avatar.transform.localPosition.y, -avatar.transform.localPosition.z) + offsetMirror;
                mirrorAvatar.transform.localEulerAngles = new Vector3(avatar.transform.localEulerAngles.x, -avatar.transform.localEulerAngles.y, -avatar.transform.localEulerAngles.z) + offsetRotationMirror;

            }
        }

        void LoadButtons()
        {
            for (int i = 0; i < buttonsToChangeAvatar.Count; i++)
            {
                buttonsToChangeAvatar[i].name = i.ToString();
                buttonsToChangeAvatar[i].onClick.AddListener(ChangeAvatar);
            }

        }

        void ChangeAvatar()
        {
            ChangeAvatar(int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name));
        }

        /// <summary>
        /// Equip you the avatar and made the mirror avatar
        /// </summary>
        void ChangeAvatar(int newAvatarIndex)
        {
            if (newAvatarIndex == avatarIndex || newAvatarIndex >= avatarPrefabs.Count) return;

            foreach (Button b in buttonsInCanvas) b.interactable = false;

            avatarIndex = newAvatarIndex;
            GameObject currentAvatar = avatar;
            if (mirrorAvatar != null) Destroy(mirrorAvatar);

            avatar = GameObject.Instantiate(avatarPrefabs[newAvatarIndex], localController.transform);

            mirrorAvatar = GameObject.Instantiate(avatarPrefabs[newAvatarIndex], mirrorAvatarParent);

            AvatarPlayer mirrorAvatarPlayer = mirrorAvatar.GetComponent<AvatarPlayer>();
            headAvatarMirror = mirrorAvatarPlayer.GetAvatarHead();
            leftHandAvatarMirror = mirrorAvatarPlayer.GetAvatarLeftHand();
            rightHandAvatarMirror = mirrorAvatarPlayer.GetAvatarRightHand();

            DestroyImmediate(mirrorAvatar.GetComponent<AvatarPlayer>());

            mirrorAvatar.transform.localPosition = Vector3.zero;
            mirrorAvatar.transform.localRotation = Quaternion.identity;
            mirrorAvatar.transform.localScale = new Vector3(-1, 1, 1);
            UnityEngine.Animations.Rigging.RigBuilder rb = mirrorAvatar.GetComponent<UnityEngine.Animations.Rigging.RigBuilder>();
            if (rb)
            {
                mirrorAvatar.GetComponent<UnityEngine.Animations.Rigging.RigBuilder>().enabled = localController.GetComponent<Unity.XR.CoreUtils.XROrigin>() != null;
            }
            AvatarPlayer avatarPlayer = avatar.GetComponent<AvatarPlayer>();
            headAvatarPlayer = avatarPlayer.GetAvatarHead();
            leftHandAvatarPlayer = avatarPlayer.GetAvatarLeftHand();
            rightHandAvatarPlayer = avatarPlayer.GetAvatarRightHand();

            if (currentAvatar != null) Destroy(currentAvatar);

            foreach (Button b in buttonsInCanvas) b.interactable = true;

            NetworkRoom.room.avatarSelected = avatarIndex;
        }

        public void CloseCanvas()
        {
            this.gameObject.SetActive(false);
        }

    }
}
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace metaverse_template
{

    /// <summary>
    /// Used to configurate the avatar to this owner
    /// This class is spawned with the avatar model
    /// </summary>
    public class AvatarPlayer : MonoBehaviourPun
    {
        /// <summary>
        /// If have, his controller
        /// </summary>
        GameObject localController;

        [Tooltip("Transform of the avatar head")]
        [SerializeField] Transform headContrains;
        [Tooltip("Transform of the avatar left hand")]
        [SerializeField] Transform leftHandContrains;
        [Tooltip("Transform of the avatar right hand")]
        [SerializeField] Transform rightHandContrains;

        [Tooltip("List of parts of the model that need to be hidden for the first-person view in VR to be seen properly")]
        [SerializeField] List<Transform> hiddenPartsforVR;

        void Start()
        {
            if (this.photonView.IsMine || !PhotonNetwork.InRoom)
            {
                SetUpAvatar();
            }
            else if (!this.photonView.IsMine && PhotonNetwork.InRoom)
            {
                AskAboutIK();
            }

        }

        /// <summary>
        /// Prepares tha tracking, IK, and set all stuff necesaries for the avatar logic
        /// </summary>
        private void SetUpAvatar()
        {
            localController = PlayerLimbFinder.Player();
            this.transform.SetParent(localController.transform);
            this.transform.localPosition = new Vector3(0, this.transform.localPosition.y, 0);
            this.transform.localRotation = Quaternion.identity;

            if (localController.GetComponent<Unity.XR.CoreUtils.XROrigin>())
            {
                //VR
                localController.GetComponent<VRRigIK>().SetAvatarIK(this.transform, headContrains, leftHandContrains, rightHandContrains);

                HideAvatarVR();
                RigBuilder rb = this.GetComponent<RigBuilder>();
                if (rb)
                {
                    if (!PhotonNetwork.InRoom) this.GetComponent<RigBuilder>().enabled = enabled;
                    else this.GetComponent<RigBuilder>().enabled = true;
                }
            }
            else
            {
                //NotVR
                Camera.main.GetComponent<MovementPlayer>().SetNetworkingAnimator(this.GetComponent<NetworkingAnimator>());

                HideAvatar();
            }
        }

        /// <summary>
        /// Hides the avatar completely
        /// </summary>
        private void HideAvatar()
        {
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount - 2; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// hides the avatar completely in VR
        /// </summary>
        private void HideAvatarVR()
        {
            foreach (Transform part in hiddenPartsforVR)
            {
                part.localScale = Vector3.zero;
            }
        }

        /// <summary>
        /// Sends a request to the owner of this avatar to see if needs to sctivate the IK system
        /// By default the IK system is disabled
        /// </summary>
        private void AskAboutIK()
        {
            this.photonView.RPC(nameof(RPC_RequestIK), this.photonView.Owner, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        /// <summary>
        /// Sends a reply to the player who asked if he needs the ik system activated.
        /// If you don't need it, it doesn't respond to you
        /// </summary>
        /// <param name="idRequestPlayer"></param>
        [PunRPC]
        private void RPC_RequestIK(int idRequestPlayer)
        {
            if (localController.GetComponent<Unity.XR.CoreUtils.XROrigin>())
            {
                Player[] safePlayersList = PhotonNetwork.PlayerListOthers.Clone() as Player[];
                int i = 0;
                bool found = false;
                while (!found && i < safePlayersList.Length)
                {
                    if (safePlayersList[i].ActorNumber == idRequestPlayer)
                    {
                        found = true;
                    }
                    else
                    {
                        i++;
                    }

                }
                if (found)
                {
                    this.photonView.RPC(nameof(RPC_SetIKEnabled), safePlayersList[i]);
                }
            }
        }

        /// <summary>
        /// Activate the IK system
        /// This function must be called in avatar whithout controller(not necesary to check)
        /// </summary>
        [PunRPC]
        private void RPC_SetIKEnabled()
        {
            this.GetComponent<RigBuilder>().enabled = true;
        }

        public Transform GetAvatarHead()
        {
            return headContrains;
        }
        public Transform GetAvatarLeftHand()
        {
            return leftHandContrains;
        }

        public Transform GetAvatarRightHand()
        {
            return rightHandContrains;
        }

    }
}

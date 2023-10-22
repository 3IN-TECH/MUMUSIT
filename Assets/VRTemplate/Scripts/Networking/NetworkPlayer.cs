using UnityEngine;
using Photon.Pun;

namespace metaverse_template
{

    /// <summary>
    /// Managed the spawn and instantiation of the avatar
    /// and all things necesaries to play online
    /// </summary>
    public class NetworkPlayer : MonoBehaviourPun
    {
        [SerializeField] GameObject localController;
        public GameObject photonAvatar;
        int avatarId;

        void Start()
        {
            if (this.photonView.IsMine || !PhotonNetwork.InRoom)
            {
                localController = GameObject.FindGameObjectWithTag("Player");

                //Eliminamos el avatar de pruebas de lobby
                bool found = false;
                int cont = localController.transform.childCount - 1;
                while (!found && -1 < cont)
                {
                    if (localController.transform.GetChild(cont).GetComponent<AvatarPlayer>() != null)
                    {
                        found = true;
                        Destroy(localController.transform.GetChild(cont).gameObject);
                    }
                    cont--;
                }

                //Creamos el nuevo avatar

                avatarId = NetworkRoom.room.avatarSelected;
                string avatarResource = "Avatar/Avatar_" + avatarId;
                photonAvatar = PhotonNetwork.Instantiate(avatarResource, Vector3.zero, Quaternion.identity);


                //Configuramos al jugador segun que tipo sea
                if (localController.GetComponent<Unity.XR.CoreUtils.XROrigin>() != null)
                {
                    //VR
                    SpawnPlayerVR();
                }
                else
                {
                    //No VR
                    SpawnPlayer();
                    localController.transform.GetChild(0).GetComponent<NotVRCameraControl>().enabled = true;
                    localController.transform.GetChild(0).GetComponent<MovementPlayer>().enabled = true;
                }

                this.transform.GetChild(0).GetComponent<OverHeadPlayer>().SetHead(Camera.main.transform);
            }
            else
            {
                // Not my PV
            }

        }

        void SpawnPlayerVR()
        {
            Transform respawn = SpawnSystem.instance.GetSpawn();
            if (respawn)
            {
                Transform headTransform = Camera.main.transform;
                Vector3 difference = localController.transform.position - headTransform.position;
                difference.y = localController.transform.position.y;
                localController.transform.position = new Vector3(respawn.position.x + difference.x, difference.y, respawn.position.z + difference.z);

                localController.transform.RotateAround(headTransform.position, new Vector3(0f, 1f, 0f), 180);
            }
        }

        void SpawnPlayer()
        {
            Transform respawn = SpawnSystem.instance.GetSpawn();
            if (respawn)
            {
                localController.transform.position = respawn.position;
                localController.transform.rotation = respawn.rotation;
            }
        }

    }

}
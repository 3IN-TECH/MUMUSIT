using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace metaverse_template
{

    /// <summary>
    /// Transport important info between scenes, info like the selected avatar.
    /// Manages the Starts the Online Player when enter in a Photon-Room
    /// and the Destroy when left
    /// </summary>
    public class NetworkRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        /// <summary>
        /// Saves the room information and has the functions that manage the creation of the player once you enter a room.
        /// Dont destroy on load
        /// </summary>
        public static NetworkRoom room;

        [Tooltip("Name of the scene loaded when entering a Room")]
        [SerializeField] string multiplayerLobbyScene = "";

        [HideInInspector] public int avatarSelected = 0;

        GameObject photonNetworkPlayer;
        string offlineScene = "";

        void Awake()
        {
            if (NetworkRoom.room == null)
            {
                NetworkRoom.room = this;
            }
            else
            {
                if (NetworkRoom.room != this)
                {
                    Destroy(NetworkRoom.room.gameObject);
                    NetworkRoom.room = this;
                }
            }
            this.transform.parent = null;
            DontDestroyOnLoad(this.gameObject);

            offlineScene = SceneManager.GetActiveScene().name;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void CreatePlayer()
        {
            if (!photonNetworkPlayer)
            {
                if (PhotonNetwork.InRoom)
                {
                    photonNetworkPlayer = PhotonNetwork.Instantiate("Networking/NetworkPlayer", new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if (PhotonNetwork.IsMasterClient)
            {
                SceneManager.sceneLoaded += OnSceneFinishedLoading;
                PhotonNetwork.LoadLevel(multiplayerLobbyScene);
            }
            else
            {
                CreatePlayer();
            }
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CreatePlayer();
                SceneManager.sceneLoaded -= OnSceneFinishedLoading;
            }
        }

        public override void OnLeftRoom()
        {
            LeftRoom();
        }

        public void LeftRoom()
        {
            StartCoroutine(DisconnectAndLoad());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (PhotonNetwork.InRoom)
            {
                int playerID = otherPlayer.ActorNumber;
                Debug.Log(string.Format("ID : " + playerID));
            }
        }

        IEnumerator DisconnectAndLoad()
        {
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected) yield return null;

            GameObject[] ddolList = this.gameObject.scene.GetRootGameObjects();
            int cont = 0;
            bool found = false;
            while (!found && cont < ddolList.Length)
            {
                if (ddolList[cont].name == "Pun Voice" || ddolList[cont].name == "NotVR Player" || ddolList[cont].name == "VR Player")
                {
                    found = true;
                    DestroyImmediate(ddolList[cont]);
                }
                cont++;
            }
            SceneManager.LoadScene(offlineScene);
        }

    }
}
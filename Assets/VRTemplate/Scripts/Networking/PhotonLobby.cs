using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Android;

namespace metaverse_template
{

    /// <summary>
    /// Starts Microphone, photon and photonvoice.
    /// and the logic of join in a Photon-Room
    /// </summary>
    public class PhotonLobby : MonoBehaviourPunCallbacks
    {
        const int maxPlayers = 5;
        const string UniqueRoomName = "VirtualRoom";

        bool connected = false;
        const float TIMELAPSE = 3f;
        float timerLapse = 3f;
        int errorCounter = 0;

        [Tooltip("List of buttons that you want to activate when photon server will ready")]
        [SerializeField] List<Button> buttons;

        private void Awake()
        {
            RecordingAuth();
        }

        void RecordingAuth()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
        }

        private void Start()
        {
            Connect();
        }

        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.ConnectToRegion("eu");
        }

        private void Update()
        {
            if (!connected)
            {
                if ((timerLapse -= Time.deltaTime) < 0)
                {
                    timerLapse = TIMELAPSE;
                    PhotonNetwork.ConnectUsingSettings();
                    PhotonNetwork.ConnectToRegion("eu");
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            connected = true;
            PhotonNetwork.AutomaticallySyncScene = true;
            TurnBtnTo(true);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (connected)
            {
                connected = false;
            }
        }

        public void StartJoinRoom()
        {
            if (connected)
            {
                TurnBtnTo(false);

                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if (errorCounter < 5)
            {
                StartCoroutine(TryAgain());
            }
            else
            {
                errorCounter = 0;
                CreateRoom(); // If there is no room available, the player creates it himself
            }
        }

        IEnumerator TryAgain()
        {
            errorCounter++;
            yield return new WaitForSeconds(0.5f);
            PhotonNetwork.JoinRandomRoom();
        }

        public void CreateRoom()
        {
            if (connected)
            {
                RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayers };
                int num = Random.Range(0, 200000);
                PhotonNetwork.CreateRoom(UniqueRoomName + "_" + num, roomOps);
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            CreateRoom();
        }

        void TurnBtnTo(bool enabled)
        {
            foreach (Button b in buttons) b.interactable = enabled;
        }
    }
}
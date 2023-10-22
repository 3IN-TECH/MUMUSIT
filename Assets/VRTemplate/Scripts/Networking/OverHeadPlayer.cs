using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using UnityEngine.UI;

public class OverHeadPlayer : MonoBehaviourPun
{
    Transform Head;
    float offsetYOverHead = 0.5f;
    float specialOffsetClass = 0f;

    Transform maincamera;

    [SerializeField] Image speakerIcon;
    [SerializeField] PhotonVoiceView photonVoiceView;

    void Update()
    {
        if (Head)
            this.transform.position = new Vector3(Head.position.x, Head.position.y + offsetYOverHead + specialOffsetClass, Head.position.z);

        if (maincamera == null) maincamera = Camera.main.transform;

        this.transform.LookAt(maincamera.transform, Vector3.up);

        speakerIcon.enabled = photonVoiceView.IsSpeaking;
    }

    public void SetHead(Transform Head)
    {
        if (this.Head == null) this.Head = Head;
    }

}
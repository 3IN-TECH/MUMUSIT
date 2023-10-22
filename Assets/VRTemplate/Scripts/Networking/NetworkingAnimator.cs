using Photon.Pun;
using UnityEngine;

/// <summary>
/// synchronise animations, online
/// </summary>
public class NetworkingAnimator : MonoBehaviourPun
{
    private int animationState = 0;
    [SerializeField] Animator animator;

    public void SetAnimation(int state)
    {
        if (PhotonNetwork.InRoom && state != animationState)
        {
            // Debug.Log("Animation changed. Animation State: " + animationState + " state: " + state);
            animationState = state;
            this.photonView.RPC(nameof(RPC_SetAnimation), RpcTarget.All, state);
        }
    }

    [PunRPC]
    private void RPC_SetAnimation(int state)
    {
        if (animator)
        {
            animationState = state;
            animator.SetInteger("State", state);
        }
    }
}

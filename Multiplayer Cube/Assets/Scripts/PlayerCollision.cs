using UnityEngine;
using Photon.Pun;

public class PlayerCollision : MonoBehaviourPunCallbacks
{
    public PlayerMovement movement;
    public bool alreadyCollided;
    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.tag== "Obstacle" && !alreadyCollided)
        {
            movement.enabled = false;
            FindObjectOfType<GameManager>().EndGame();
        }
        photonView.RPC("destroyItem", RpcTarget.Others, collisionInfo);
    }

    void destroyItem(Collision collisionCause)
    {
        PhotonNetwork.Destroy(collisionCause.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
    }
}

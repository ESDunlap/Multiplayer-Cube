using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

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
            Destroy(collisionInfo.gameObject);
        }
    }
}

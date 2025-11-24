using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Vector3 reverseOffset;
    public bool reverse = false;
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
            reverse= !reverse;
            FindObjectOfType<PlayerMovement>().sidewaysForce = -FindObjectOfType<PlayerMovement>().sidewaysForce;
        }

        if (reverse)
        {
            transform.position = player.position + reverseOffset;
        }
        else
        {
            transform.position = player.position + offset;
        }
    }
}

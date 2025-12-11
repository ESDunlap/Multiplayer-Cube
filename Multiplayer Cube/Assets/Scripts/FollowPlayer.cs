using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Vector3 reverseOffset;
    public bool reverse = false;
    void Update()
    {
        if (reverse)
        {
            transform.position = player.position + reverseOffset;
        }
        else
        {
            transform.position = player.position + offset;
            transform.rotation = Quaternion.identity;
        }
    }
}

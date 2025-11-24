using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverseAttack : MonoBehaviour
{
    public Transform player;
    public float distanceActivation;
    public float reverseSpeed = 5000f;
    public Rigidbody rb;
    private bool activated= false;

    void Update()
    {
        if (player.position.z > distanceActivation)
        {
            activated = true;
            GetComponent<BoxCollider>().enabled = true;
        }

        if(activated && !FindObjectOfType<GameManager>().gameHasEnded)
        {
            rb.AddForce(0, 0, reverseSpeed * Time.deltaTime);
        }
    }
}

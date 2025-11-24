using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public bool winGame= false;
    void OnTriggerEnter()
    {
        if (FindObjectOfType<GameManager>().gameHasEnded == false)
        {
            gameManager.CompleteLevel();
        }
    }
}

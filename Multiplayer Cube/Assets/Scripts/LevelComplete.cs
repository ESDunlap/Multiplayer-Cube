using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class LevelComplete : MonoBehaviourPunCallbacks
{
    public void OnStartGameButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Level0" + SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

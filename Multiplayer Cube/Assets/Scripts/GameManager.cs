using NUnit;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    private int curPlayers= 0;
    private int collidedPlayers= 0;
    public bool gameHasEnded = false;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;

    //Pun
    [Header("Players")]
    public string playerPrefabLocation;
    public Transform spawnPoint;
    public PlayerMovement[] players;
    private int playersInGame;

    public static GameManager instance;
    void Awake()
    {
        // instance
        instance = this;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            curPlayers++;
        }
    }

    public void CompleteLevel()
    {
        gameHasEnded = true;
        completeLevelUI.SetActive(true);
    }

    [PunRPC]
    public void EndGame()
    {
        collidedPlayers++;
        if (gameHasEnded == false && collidedPlayers==curPlayers)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {
        players = new PlayerMovement[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.All);
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            Invoke("SpawnPlayer",(PhotonNetwork.LocalPlayer.ActorNumber-1000)*10000000000);
        }

    }

    void SpawnPlayer()
    {
        // instantiate the player across the network
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoint.position, Quaternion.identity);
        // get the player script
        PlayerMovement playerScript = playerObj.GetComponent<PlayerMovement>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
}

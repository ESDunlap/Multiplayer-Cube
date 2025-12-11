using NUnit;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

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

    //Playfab
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;

    public static GameManager instance;
    void Awake()
    {
        // instance
        instance = this;
        DisplayLeaderboard();
    }

    public void CompleteLevel()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateHighScore",
            FunctionParameter = new { score = SceneManager.GetActiveScene().buildIndex}
        };
        PlayFabClientAPI.ExecuteCloudScript(request,
        result => DisplayLeaderboard(),
        error => Debug.Log(error.ErrorMessage)
        );
        gameHasEnded = true;
        completeLevelUI.SetActive(true);
    }

    [PunRPC]
    public void EndGame()
    {
        collidedPlayers++;
        if (gameHasEnded == false && collidedPlayers== PhotonNetwork.PlayerList.Length)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Invoke("Restart", restartDelay);
        }
    }

    [PunRPC]
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
            Invoke("SpawnPlayer", (PhotonNetwork.LocalPlayer.ActorNumber-1) * 1.0f);
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

    public void DisplayLeaderboard()
    {
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "Deaths",
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
        result => UpdateLeaderboardUI(result.Leaderboard),
        error => Debug.Log(error.ErrorMessage)
        );

    }

    void UpdateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        for (int x = 0; x < leaderboardEntries.Length; x++)
        {
            leaderboardEntries[x].SetActive(x < leaderboard.Count);
            if (x >= leaderboard.Count) continue;
            leaderboardEntries[x].transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            leaderboardEntries[x].transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = ((float)leaderboard[x].StatValue).ToString();
        }
    }

    public void SetLeaderboardEntry(int newScore)
    {
    }
}

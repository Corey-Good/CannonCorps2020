/************************************************************************/
/* Author:             Corey Good                                     */
/* Date Created:       ??                                      */
/* Last Modified Date: 3/6/2020                                        */
/* Modified By:        Corey Good                                     */
/************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Photon.Pun;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviourPunCallbacks
{
    #region Classes
    private Tank tank;
    private Player player;
    private PlayerController playerController;
    #endregion

    #region Player Info
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScoreText;
    public Slider healthBar;
    public Image reloadDial;
    public Image bulletIcon;
    public Image freezeBulletIcon;
    public Image dynamiteBulletIcon;
    public Image laserBulletIcon;
    
    #endregion

    #region Table of Players
    public GameObject playerTable;
    public GameObject playerListing;
    private List<GameObject> scoreListings = new List<GameObject>();
    #endregion

    #region Game Timer
    public TextMeshProUGUI gameTimer;
    public static double matchTimer = 0;
    private int minute;
    private int second;
    private double startTime;
    private double matchLength = 300;
    #endregion

    #region Team Points
    public Slider redTeamScore;
    public Slider blueTeamScore;
    #endregion    

    public RectTransform transitionPanel;
    public GameObject gameOverRect;
    public TextMeshProUGUI gameOverText;
    public GameObject hitIndicator;
    public List<TextMeshProUGUI> textPoints = new List<TextMeshProUGUI>();
    public List<RectTransform> rectPoints = new List<RectTransform>();

    void Awake()
    {
        Cursor.visible = false;

        // Get the instance of the Tank and Player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        playerController = GameObject.FindGameObjectWithTag("PlayerGO").GetComponent<PlayerController>();

        // Set default values
        playerScoreText.text = "0";
        tank.reloadProgress = 1.0f;
        if (player.PlayerName != null)
            playerName.text = player.PlayerName;
        else
            playerName.text = "Blank";
        playerScoreText.text = player.ScoreCurrent.ToString();
        matchTimer = 0;

        // Turn on game timer for Sharks and Minnows
        if (player.gameState == Player.GameState.SM)
        {
            SetTimer();
        }

        // Turn on the team scores for Team Battle
        if (player.gameState == Player.GameState.TB)
        {
            redTeamScore.gameObject.SetActive(true);
            blueTeamScore.gameObject.SetActive(true);
        }

        // Update the table of players
        UpdateTable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Constantly update the various UI rendered
        healthBar.value = tank.healthCurrent / tank.healthMax;
        reloadDial.fillAmount = tank.reloadProgress;
        //bulletIcon.fillAmount = tank.reloadProgress;
        playerScoreText.text = player.ScoreCurrent.ToString();

        bulletIcon.fillAmount = tank.reloadProgress;
        freezeBulletIcon.fillAmount = (playerController.numOfFreezeBullets / playerController.maxNumOfFreezeBullets);
        dynamiteBulletIcon.fillAmount = (playerController.numOfDynamiteBullets / playerController.maxNumOfDynamiteBullets);
        laserBulletIcon.fillAmount = (playerController.numOfLaserBullets / playerController.maxNumOfLaserBullets);

        switch (playerController.currentBulletType)
        {
            case PlayerController.BulletType.Normal:
                bulletIcon.CrossFadeAlpha(1.0f, .5f, true);
                if (freezeBulletIcon.fillAmount != 0.0f)
                    freezeBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                if (dynamiteBulletIcon.fillAmount != 0.0f)
                    dynamiteBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                if (laserBulletIcon.fillAmount != 0.0f)
                    laserBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                break;
            case PlayerController.BulletType.FreezeBullet:
                freezeBulletIcon.CrossFadeAlpha(1.0f, .5f, true);
                if (bulletIcon.fillAmount != 0.0f)
                    bulletIcon.CrossFadeAlpha(.3f, .5f, false);
                if (dynamiteBulletIcon.fillAmount != 0.0f)
                    dynamiteBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                if (laserBulletIcon.fillAmount != 0.0f)
                    laserBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                break;
            case PlayerController.BulletType.DynamiteBullet:
                dynamiteBulletIcon.CrossFadeAlpha(1.0f, .5f, true);
                if (freezeBulletIcon.fillAmount != 0.0f)
                    freezeBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                if (bulletIcon.fillAmount != 0.0f)
                    bulletIcon.CrossFadeAlpha(.3f, .5f, false);
                if (laserBulletIcon.fillAmount != 0.0f)
                    laserBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                break;
            case PlayerController.BulletType.LaserBullet:
                laserBulletIcon.CrossFadeAlpha(1.0f, .5f, true);
                if (freezeBulletIcon.fillAmount != 0.0f)
                    freezeBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                if (dynamiteBulletIcon.fillAmount != 0.0f)
                    dynamiteBulletIcon.CrossFadeAlpha(.3f, .5f, false);
                if (bulletIcon.fillAmount != 0.0f)
                    bulletIcon.CrossFadeAlpha(.3f, .5f, false);
                break;
        }

        if (player.gameState == Player.GameState.SM)
        {
            UpdateTimer();
        }

        if (player.gameState == Player.GameState.TB)
        {
            redTeamScore.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedScore"];
            blueTeamScore.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueScore"];
        }

        // Display the list of players 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerTable.SetActive(!playerTable.activeSelf);
        }

        if (player.leaveGame)
        {
            StartCoroutine(SwitchScene());
        }

        if (tank.tankHit)
        {
            FlashHit();
            tank.tankHit = false;
        }

        if (player.gotPoints)
        {
            ShowPoints();
            player.gotPoints = false;
        }
    }

    private void UpdateTable()
    {

        if (scoreListings != null)
        {
            foreach (GameObject listing in scoreListings)
            {
                Destroy(listing);
            }
            scoreListings.Clear();
        }

        foreach (Photon.Realtime.Player photonPlayer in PhotonNetwork.PlayerList)
        {
            // Create and add a player listing
            GameObject tempListing = Instantiate(playerListing);
            tempListing.transform.SetParent(playerTable.transform, false);

            // Add the player listing to the list
            scoreListings.Add(tempListing);

            // Set the players name
            TextMeshProUGUI tempText = tempListing.GetComponentInChildren<TextMeshProUGUI>();
            tempText.text = photonPlayer.NickName;
            if(string.Equals(photonPlayer.NickName, player.PlayerName))
            {
                tempText.color = Color.white;
            }
            else
            {
                tempText.color = Color.black;
            }
        }
    }

    public void SetTimer()
    {
        //bool timeSet = false;
        gameTimer.gameObject.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "StartTime", startTime } });
        }
        else
        {
            try
            {
                startTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
            }
            catch
            {
                startTime = PhotonNetwork.Time;
            }
        }
    }

    public void UpdateTimer()
    {
        double timer;
        matchTimer = PhotonNetwork.Time - startTime;
        timer = matchLength - matchTimer;
        second = (int)(timer % 60.0f);
        minute = (int)(timer / 60.0f);
        gameTimer.text = "";
        gameTimer.text = minute.ToString() + ":" + second.ToString("00");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdateTable();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateTable();
    }

    private IEnumerator SwitchScene()
    {
        player.leaveGame = false;
        player.returning = true;
        TutorialMode.tutorialModeOn = false;
        // Start the scene transition, wait 1 second before proceeding to the next line
        LeanTween.alpha(transitionPanel, 1, 1);
        SetEndText();
        gameOverRect.SetActive(true);
        yield return new WaitForSeconds(3);

        // Leave the room, waiting until we are disconnected from the room to proceed
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;

        // Make the cursor visible and free to move on the screen
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;

        // Move back to main menu, unload the UI scene (this must be done last)
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
        SceneManager.UnloadSceneAsync(1);
    }

    public void FlashHit()
    {
        RectTransform[] edges = hitIndicator.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform edge in edges)
        {
            LeanTween.alpha(edge, 1, 0.75f);
            LeanTween.alpha(edge, 0, 1f);
        }
    }

    public void ShowPoints()
    {
        int randomInt = Random.Range(0, textPoints.Count);
        StartCoroutine(MoveText(randomInt));
    }

    IEnumerator MoveText(int index)
    {
        textPoints[index].text = "+10";
        yield return new WaitForSeconds(1.2f);
        textPoints[index].text = "";
    }

    void SetEndText()
    {
        switch(player.gameState)
        {
            case Player.GameState.FFA:
                gameOverText.text = "You were eliminated";
                break;
            case Player.GameState.SM:
                if(matchTimer >= 300.0)
                {
                    gameOverText.text = "The Minnows evaded the Shark!";
                }
                else
                {
                    gameOverText.text = "All Minnows were eliminated.";
                }
                
                break;
            case Player.GameState.TB:
                if(blueTeamScore.value >= 250)
                {
                    gameOverText.text = "The Blue Team won!";
                }
                else if(redTeamScore.value >= 250)
                {
                    gameOverText.text = "The Red Team won!";
                }
                else
                {
                    gameOverText.text = "You quit the game . . .";
                }
                break;
            case Player.GameState.TT:
                gameOverText.text = "";
                break;
        }
    }
}

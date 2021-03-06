﻿/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       2/10/2020                                        */
/* Last Modified Date: 4/13/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/
#region Libraries
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#endregion
public class UIManager : MonoBehaviourPunCallbacks
{
    #region Variables
    #region Classes
    private Tank tank;
    private Player player;
    private PlayerController playerController;
    #endregion
    #region Player UI
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScoreText;
    public Slider healthBar;
    public Image reloadDial;
    public Image bulletIcon;
    public Image freezeBulletIcon;
    public Image dynamiteBulletIcon;
    public Image laserBulletIcon;
    public Image reloadIcon;
    public Image speedIcon;
    public Image shieldIcon;
    public GameObject hitIndicator;
    public GameObject freezeIndicator;
    public List<TextMeshProUGUI> textPoints = new List<TextMeshProUGUI>();
    private bool flashFreeze = true;
    public static string powerupName;
    #endregion
    #region SharksAndMinnows
    public TextMeshProUGUI gameTimer;
    public static double matchTimer = 0;
    private int minute;
    private int second;
    private double startTime;
    private double matchLength = 300;
    #endregion // Game Timer
    #region TeamBattle
    public Slider redTeamScore;
    public Slider blueTeamScore;
    #endregion           // Team Points
    #region Tutorial
    private GameObject checkpoint1;
    private GameObject checkpoint2;
    private GameObject trigger1;
    private GameObject trigger2;
    private double tutorialTimer;
    public static bool cameraIsEnabled = true;
    public static bool movementIsEnabled = true;
    public static bool firingIsEnabled = true;
    public static bool[] tutorialSteps = new bool[6];
    #endregion         //
    #region Table of Players
    public GameObject playerTable;
    public GameObject playerListing;
    private List<GameObject> scoreListings = new List<GameObject>();
    #endregion
    #region Text Panel
    public GameObject textPanel;
    public TextMeshProUGUI headingText;
    public TextMeshProUGUI subtitleText;
    #endregion
    #region Transition Scene
    public RectTransform transitionPanel;
    public GameObject gameOverObj;
    public TextMeshProUGUI gameOverText;
    #endregion
    #endregion
    void Awake()
    {
        Cursor.visible = false;                       // Removes the cursor after scene loads to accomodate preset crosshair
        #region Get the instance of the Tank and Player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        playerController = GameObject.FindGameObjectWithTag("PlayerGO").GetComponent<PlayerController>();
        LeanTween.scale(gameOverObj, Vector3.zero, 0); // Used to transition into scene?
        #endregion
        #region Set default values
        playerScoreText.text = "0";
        tank.reloadProgress = 1.0f;
        if (player.PlayerName != null)
            playerName.text = player.PlayerName;
        else
            playerName.text = "Blank";
        playerScoreText.text = player.ScoreCurrent.ToString();
        matchTimer = tutorialTimer = 0;

        #endregion
        #region Initialize Sharks and Minnows variables
        if (player.gameState == Player.GameState.SM)
            SetTimer();
        #endregion
        #region Initialize Team Battle variables
        if (player.gameState == Player.GameState.TB)
        {
            redTeamScore.gameObject.SetActive(true);
            blueTeamScore.gameObject.SetActive(true);
        }
        #endregion              
        #region Initialize Tutorial variables
        if (player.gameState == Player.GameState.TT)
        {
            for (int i = 0; i < tutorialSteps.Length; i++)
                tutorialSteps[i] = false;
            tutorialSteps[0] = true;
            TurnPanelOn();
            cameraIsEnabled = movementIsEnabled = firingIsEnabled = false;
            checkpoint1 = GameObject.Find("Checkpoint (1)");
            checkpoint2 = GameObject.Find("Checkpoint (2)");
            trigger1 = GameObject.Find("Trigger (1)");
            trigger2 = GameObject.Find("Trigger (2)");
            startTime = (int)PhotonNetwork.Time;
            playerScoreText.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(false);
            bulletIcon.gameObject.SetActive(false);
            freezeBulletIcon.gameObject.SetActive(false);
            dynamiteBulletIcon.gameObject.SetActive(false);
            laserBulletIcon.gameObject.SetActive(false);
            reloadIcon.gameObject.SetActive(false);
            speedIcon.gameObject.SetActive(false);
            shieldIcon.gameObject.SetActive(false);
            gameOverObj.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
            hitIndicator.gameObject.SetActive(false);
            freezeIndicator.gameObject.SetActive(false);
        }
        #endregion
        UpdateTable();                                // Updates the table of players
    }
    void FixedUpdate()
    {
        #region If-Statements
        if (player.gameState == Player.GameState.SM)
        {
            UpdateTimer();
        }
        if (player.gameState == Player.GameState.TB)
        {
            redTeamScore.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedScore"];
            blueTeamScore.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueScore"];
        }
        if (player.gameState == Player.GameState.TT)
        {
            DisplayTutorialPrompts();
            tutorialTimer = (int)(PhotonNetwork.Time - startTime);
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
        if (playerController.isFrozen && flashFreeze)
        {
            FlashFreeze();
            flashFreeze = false;
        }
        if (!playerController.isFrozen && !flashFreeze)
        {
            BreakFreeze();
            flashFreeze = true;
        }
        if (playerController.powerupAcquired)
        {
            StartCoroutine(DisplayPowerupPrompts());
        }
        #endregion
        #region Constantly updates the various UI rendered
        healthBar.value = tank.healthCurrent / tank.healthMax;
        reloadDial.fillAmount = tank.reloadProgress;
        playerScoreText.text = player.ScoreCurrent.ToString();
        #endregion
        #region Powerups UI
        shieldIcon.enabled = playerController.invulnerable;
        reloadIcon.fillAmount = (playerController.reloadBoostTimer / playerController.maxReloadBoostTimer);
        speedIcon.fillAmount = (playerController.speedBoostTimer / playerController.maxSpeedBoostTimer);
        freezeBulletIcon.fillAmount = (playerController.numOfFreezeBullets / playerController.maxNumOfFreezeBullets);
        dynamiteBulletIcon.fillAmount = (playerController.numOfDynamiteBullets / playerController.maxNumOfDynamiteBullets);
        laserBulletIcon.fillAmount = (playerController.numOfLaserBullets / playerController.maxNumOfLaserBullets);

        switch (playerController.currentBulletType)
        {
            case PlayerController.BulletType.Normal:
                bulletIcon.CrossFadeAlpha(1.0f, .2f, true);
                if (freezeBulletIcon.fillAmount != 0.0f)
                    freezeBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                if (dynamiteBulletIcon.fillAmount != 0.0f)
                    dynamiteBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                if (laserBulletIcon.fillAmount != 0.0f)
                    laserBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                break;
            case PlayerController.BulletType.FreezeBullet:
                freezeBulletIcon.CrossFadeAlpha(1.0f, .2f, true);
                if (bulletIcon.fillAmount != 0.0f)
                    bulletIcon.CrossFadeAlpha(.0f, .2f, false);
                if (dynamiteBulletIcon.fillAmount != 0.0f)
                    dynamiteBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                if (laserBulletIcon.fillAmount != 0.0f)
                    laserBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                break;
            case PlayerController.BulletType.DynamiteBullet:
                dynamiteBulletIcon.CrossFadeAlpha(1.0f, .2f, true);
                if (freezeBulletIcon.fillAmount != 0.0f)
                    freezeBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                if (bulletIcon.fillAmount != 0.0f)
                    bulletIcon.CrossFadeAlpha(.0f, .2f, false);
                if (laserBulletIcon.fillAmount != 0.0f)
                    laserBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                break;
            case PlayerController.BulletType.LaserBullet:
                laserBulletIcon.CrossFadeAlpha(1.0f, .2f, true);
                if (freezeBulletIcon.fillAmount != 0.0f)
                    freezeBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                if (dynamiteBulletIcon.fillAmount != 0.0f)
                    dynamiteBulletIcon.CrossFadeAlpha(.0f, .2f, false);
                if (bulletIcon.fillAmount != 0.0f)
                    bulletIcon.CrossFadeAlpha(.0f, .2f, false);
                break;
        }
        #endregion
        #region Display the list of players
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerTable.SetActive(!playerTable.activeSelf);
        }
        #endregion
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
            if (string.Equals(photonPlayer.NickName, player.PlayerName))
            {
                tempText.color = Color.black;
            }
            else
            {
                tempText.color = Color.white;
            }
        }
    }
    public void SetTimer()
    {
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
        //Debug.Log("UI right before switching scenes: Tank Model" + tank.tankModel);
        player.leaveGame = false;
        player.returning = true;

        // Start the scene transition, wait 1 second before proceeding to the next line
        LeanTween.alpha(transitionPanel, 1, 1);
        SetEndText();
        yield return new WaitForSeconds(0.65f);
        LeanTween.scale(gameOverObj, Vector3.one, 1); ;
        yield return new WaitForSeconds(2);

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
    public void FlashFreeze()
    {
        RectTransform[] edges = freezeIndicator.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform edge in edges)
        {
            LeanTween.alpha(edge, 1, 0.75f);
        }
    }
    public void BreakFreeze()
    {
        RectTransform[] edges = freezeIndicator.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform edge in edges)
        {
            LeanTween.alpha(edge, 0, 1f);
        }
    }
    public void ShowPoints()
    {
        int randomInt = Random.Range(0, textPoints.Count);
        StartCoroutine(MoveText(randomInt));
    }
    private IEnumerator MoveText(int index)
    {
        textPoints[index].text = "+10";
        yield return new WaitForSeconds(1.2f);
        textPoints[index].text = "";
    }
    private void SetEndText()
    {
        switch (player.gameState)
        {
            case Player.GameState.FFA:
                if (PauseMenuManager.playerQuit)
                {
                    gameOverText.text = "You quit the game . . . ";
                }
                else
                {
                    gameOverText.text = "You were eliminated";
                }
                break;

            case Player.GameState.SM:
                if (matchTimer >= 300.0)
                {
                    gameOverText.text = "The Minnows evaded the Shark!";
                }
                else if (PauseMenuManager.playerQuit)
                {
                    gameOverText.text = "You quit the game . . .";
                }
                else
                {
                    gameOverText.text = "All Minnows were eliminated.";
                }
                break;

            case Player.GameState.TB:
                if (blueTeamScore.value >= 250)
                {
                    gameOverText.text = "The Blue Team won!";
                }
                else if (redTeamScore.value >= 250)
                {
                    gameOverText.text = "The Red Team won!";
                }
                else if (PauseMenuManager.playerQuit)
                {
                    gameOverText.text = "You quit the game . . .";
                }
                break;

            case Player.GameState.TT:
                if (PauseMenuManager.playerQuit)
                {
                    gameOverText.text = "You quit the game . . .";
                }
                else
                {
                    gameOverText.text = "Tutorial Mode Completed!";
                }
                break;
        }
    }
    private IEnumerator DisplayPowerupPrompts()
    {
        TurnPanelOn();
        headingText.text = string.Format("Acquired [{0}] powerup!", powerupName.ToLower());
        switch (powerupName)
        {
            #region Active Powerups
            case "Reload":
                subtitleText.text = string.Format("Press [{0}] to activate {1} boost!", AlphaFilter(KeyBindings.activateReloadBoost).ToLower(), powerupName.ToLower());
                break;
            case "Speed":
                subtitleText.text = string.Format("Press [{0}] to activate {1} boost!", AlphaFilter(KeyBindings.activateMovementBoost).ToLower(), powerupName.ToLower());
                break;
            #endregion
            #region Passive Powerups
            case "Health":
                subtitleText.text = string.Format("[{0}] restored!", powerupName);
                break;
            case "Shield":
                subtitleText.text = string.Format("[{0}] activated!", powerupName);
                break;
            #endregion
            #region Bullet Powerups
            case "Dynamite":
            case "Freeze":
            case "Laser":
                subtitleText.text = string.Format("Press [{0}] to switch to {1} bullets!", AlphaFilter(KeyBindings.switchBulletType).ToLower(), powerupName.ToLower());
                break;
            #endregion
            default:
                break;
        }
        playerController.powerupAcquired = false;
        yield return new WaitForSeconds(3);
        TurnPanelOff();
    }     // 
    private void DisplayTutorialPrompts()
    {
        #region Autoplay Step 1-3
        switch (tutorialTimer)
        {
            case 5:
                tutorialSteps[1] = true;
                break;
            case 10:
                tutorialSteps[2] = true;
                break;
            case 15:
                tutorialSteps[3] = true;
                break;
            default:
                break;
        }
        #endregion
        #region Step 1: Welcome Message
        if (tutorialSteps[0])
        {
            headingText.text = "Welcome to the training camp!";
            subtitleText.text = "Here you will learn the basic skills required in battle.";
        }
        #endregion Step 1
        #region Step 2: Camera Control
        if (tutorialSteps[1])
        {
            cameraIsEnabled = true;
            headingText.text = "Use your mouse to control the camera.";
            subtitleText.text = "The turret follows the camera as you move it.";
        }
        #endregion Step 2
        #region Step 3: Movement Control
        if (tutorialSteps[2])
        {
            movementIsEnabled = true;
            headingText.text = string.Format("Use the {0}, {1}, {2}, and {3} keys to control your vehicle.",
                                      KeyBindings.forwardKey, KeyBindings.leftKey, KeyBindings.backwardKey, KeyBindings.rightKey);
            subtitleText.text = "";
        }
        #endregion Step 3
        #region Step 4: Move to Designated Area
        if (tutorialSteps[3])
        {
            checkpoint1.LeanMoveLocalY(-5, 2f);
            headingText.text = "Move to the designated location.";
            subtitleText.text = "";
        }
        #endregion Step 4
        #region Step 5: Firing Control
        if (tutorialSteps[4])
        {
            checkpoint2.LeanMoveLocalY(-5, 2f);
            firingIsEnabled = true;
            headingText.text = string.Format("Aim at the designated target and {0} to fire.",
                                     ((KeyBindings.clickIndex == 0) ? "Left Click" : "Right Click"));
            subtitleText.text = "";
        }
        #endregion Step 5
        #region Step 6: Complete Tutorial
        if (tutorialSteps[5])
        {
            headingText.text = "CONGRATULATIONS!";
            subtitleText.text = "You have successfully completed the tutorial!";
            StartCoroutine(RemoveTutorialPrompts());
        }
        #endregion Step 6
    }           // 
    private IEnumerator RemoveTutorialPrompts()
    {
        yield return new WaitForSeconds(2);
        TurnPanelOff();
        player.leaveGame = cameraIsEnabled = movementIsEnabled = firingIsEnabled = true;
    }     // 
    private void TurnPanelOn()
    {
        textPanel.gameObject.SetActive(true);
        headingText.gameObject.SetActive(true);
        subtitleText.gameObject.SetActive(true);
    }                      // 
    private void TurnPanelOff()
    {
        textPanel.gameObject.SetActive(false);
        headingText.gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
    }                     // 
    private string AlphaFilter(KeyCode oldKeyPress)
    {
        string newKeyPress = oldKeyPress.ToString();
        if (newKeyPress.Contains("Alpha"))
            newKeyPress = newKeyPress.Replace("Alpha", "");
        return newKeyPress;
    } // 
}
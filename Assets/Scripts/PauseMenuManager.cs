﻿/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       3/03/2020                                        */
/* Last Modified Date: 3/31/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/

using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    #region Variables
    public GameObject    pauseMenu;
    public         GameObject    optionsMenu;
    public         GameObject    controlsMenu;
    public         GameObject    infoMenu;
    private        Player        player;

    public  static bool          gameIsPaused = false;
    public  static bool          playerQuit = false;
    public  static RectTransform transitionPanel;
    #endregion

    private void Awake()
    {
        player     = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        playerQuit = false;
    }

    // Lock the mouse, pause the game, and open the pause menu and vice versa
    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Cursor.lockState  = CursorLockMode.Locked;
                Cursor.visible    = false;
                pauseMenu.SetActive(false);
                ResumeGame();
            }
            else
            {
                Cursor.lockState  = CursorLockMode.None;
                Cursor.visible    = true;
                pauseMenu.SetActive(true);
                PauseGame();
            }
        }
    }

    // Open the pause menu
    public void PauseGame()
    {
        gameIsPaused   = true;
        Cursor.visible = true;
    }

    // Close all menus and resume the game
    public void ResumeGame()
    {
        gameIsPaused   = false;
        Cursor.visible = false;

        pauseMenu.   SetActive(false);
        optionsMenu. SetActive(false);
        controlsMenu.SetActive(false);
        infoMenu.    SetActive(false);
    }

    // Exit for the game and switch to the main menu
    public void QuitGame()
    {
        playerQuit       = false;
        gameIsPaused     = false;
        player.leaveGame = true;
    }
}
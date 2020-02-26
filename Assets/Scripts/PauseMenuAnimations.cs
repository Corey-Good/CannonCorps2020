﻿/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       2/23/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuAnimations : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject controlsMenu;
    public GameObject infoMenu;

    // Lock the mouse, pause the game, and open the pause menu and vice versa
    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Resume();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Pause();
            }
        }
    }

    // Open the pause menu
    public void Pause()
    {
        Cursor.visible = true;
        GameIsPaused   = true;

        pauseMenu.   SetActive(true);
        optionsMenu. SetActive(false);
        controlsMenu.SetActive(false);
        infoMenu.    SetActive(false);
    }

    // Close all menus and resume the game
    public void Resume()
    {
        GameIsPaused = false;

        pauseMenu.   SetActive(false);
        optionsMenu. SetActive(false);
        controlsMenu.SetActive(false);
        infoMenu.    SetActive(false);
    }

    // Switch to the options menu
    public void Options()
    {
        Cursor.visible = true;
        GameIsPaused   = true;

        pauseMenu.   SetActive(false);
        optionsMenu. SetActive(true);
        controlsMenu.SetActive(false);
        infoMenu.    SetActive(false);
    }

    // Switch to the Controls menu
    public void Controls()
    {
        Cursor.visible = true;
        GameIsPaused   = true;

        pauseMenu.   SetActive(false);
        optionsMenu. SetActive(false);
        controlsMenu.SetActive(true);
        infoMenu.    SetActive(false);
    }

    // Switch to the Info menu
    public void Info()
    {
        Cursor.visible = true;
        GameIsPaused   = true;

        pauseMenu.   SetActive(false);
        optionsMenu. SetActive(false);
        controlsMenu.SetActive(false);
        infoMenu.    SetActive(true);
    }

    // Exit for the game and switch to the main menu
    public void Quit()
    {
        GameIsPaused = false;
        StartCoroutine(DisconnectAndLoad());
    }

    // Scale the menu as an animation
    public void OpenPauseMenu(GameObject Menu)
    {
        Menu.SetActive(true);
        Menu.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(Menu, new Vector3(1, 1, 1), 0.5f);
    }

    // Scale the menue as an animation
    public void ClosePauseMenu(GameObject Menu)
    {
        LeanTween.scale(Menu, new Vector3(0, 0, 0), 0.5f);
        Invoke("turnOffMenu", 0.5f);
    }

    // Disconnect from the current game and switch to the main menu
    private IEnumerator DisconnectAndLoad()
    {
        // center the mouse
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);

        // disconnect from the game
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;

        Cursor.lockState = CursorLockMode.None;
        
        // load the menu scene
        SceneManager. UnloadSceneAsync(2);
        PhotonNetwork.LoadLevel(0);
    }
}
/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/23/20                                         */
/* Last Modified Date: 02/25/20                                         */
/* Modified By:        Jaben Calas                                      */
/************************************************************************/

using UnityEngine;

public class PauseMenuAnimations : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject  pauseMenu;
    public GameObject  optionsMenu;
    public GameObject  controlsMenu;
    public GameObject  infoMenu;

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

    public void Pause()
    {
        Cursor.visible       = true  ;
        GameIsPaused         = true  ;

        pauseMenu.   SetActive(true) ;
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        infoMenu    .SetActive(false);
    }

    public void Resume()
    {
        GameIsPaused         = false ;

        pauseMenu.   SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        infoMenu.    SetActive(false);
    }

    public void Options()
    {
        Cursor.visible       = true  ;
        GameIsPaused         = true  ;

        pauseMenu.   SetActive(false);
        optionsMenu.SetActive(true) ;
        controlsMenu.SetActive(false);
        infoMenu.    SetActive(false);
    }
    public void Controls()
    {
        Cursor.visible       = true  ;
        GameIsPaused         = true  ;

        pauseMenu.   SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(true) ;
        infoMenu.    SetActive(false);
    }

    public void Info()
    {
        Cursor.visible       = true  ;
        GameIsPaused         = true  ;

        pauseMenu.   SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        infoMenu.    SetActive(true) ;
    }

    public void Quit()
    {
        GameIsPaused         = false;
    }

    public void OpenPauseMenu(GameObject Menu)
    {
        Menu.SetActive(true);

        Menu.transform.localScale = new Vector3(0, 0, 0);

        LeanTween.scale(Menu, new Vector3(1, 1, 1), 0.5f);
    }

    public void ClosePauseMenu(GameObject Menu)
    {
        LeanTween.scale(Menu, new Vector3(0, 0, 0), 0.5f);

        Invoke("turnOffMenu", 0.5f);
    }
}
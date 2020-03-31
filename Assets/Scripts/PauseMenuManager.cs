/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       3/03/2020                                        */
/* Last Modified Date: 3/31/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/

using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public static  bool          GameIsPaused = false;
    public         GameObject    pauseMenu;
    public         GameObject    optionsMenu;
    public         GameObject    controlsMenu;
    public         GameObject    infoMenu;
    public  static RectTransform transitionPanel;
    private        Player        player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    // Lock the mouse, pause the game, and open the pause menu and vice versa
    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
                ResumeGame();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
                PauseGame();
            }
        }
    }

    // Open the pause menu
    public void PauseGame()
    {
        GameIsPaused   = true;
        Cursor.visible = true;
    }

    // Close all menus and resume the game
    public void ResumeGame()
    {
        GameIsPaused   = false;
        Cursor.visible = false;

        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        infoMenu.SetActive(false);
    }

    // Exit for the game and switch to the main menu
    public void QuitGame()
    {
        GameIsPaused     = false;
        player.leaveGame = true;
    }
}
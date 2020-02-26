/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       2/20/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using UnityEngine;

public class MainMenuAnimations : MonoBehaviour
{
    public GameObject mainMenu;

    // Used for debug purposes
    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    CloseMenu();
        //}
    }
    
    // Scale the menu to look like an animation
    public void OpenMenu()
    {
        mainMenu.SetActive(true);
        mainMenu.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(mainMenu, new Vector3(1, 1, 1), 0.5f);
    }

    // Scale the menu to look like an animation
    public void CloseMenu()
    {
        LeanTween.scale(mainMenu, new Vector3(0, 0, 0), 0.5f);
        Invoke("turnOffMenu", 0.5f);
    }

    // Deactivate the main menu
    public void turnOffMenu()
    {
        mainMenu.SetActive(false);
    }
}
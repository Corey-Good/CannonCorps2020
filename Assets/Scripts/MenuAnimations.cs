/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       2/20/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    public GameObject currentMenu;
    public static float transitionTime = 0.5f;

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
        currentMenu.SetActive(true);
        currentMenu.transform.localScale = Vector3.zero;
        LeanTween.scale(currentMenu, Vector3.one, transitionTime);
    }

    // Scale the menu to look like an animation
    public void CloseMenu()
    {
        LeanTween.scale(currentMenu, Vector3.zero, transitionTime);
        Invoke("turnOffMenu", transitionTime);
    }

    // Deactivate the main menu
    public void turnOffMenu()
    {
        currentMenu.SetActive(false);
    }
}
/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       2/20/2020                                        */
/* Last Modified Date: 3/01/2020                                        */
/* Modified By:        J. Calas                                     */
/************************************************************************/

using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject currentMenu;
    public GameObject nextMenu;
    private float transitionTime = 0.5f;

    // Scale the menu to look like an animation
    public void OpenNewMenu()
    {
        nextMenu.SetActive(true);
        nextMenu.transform.localScale = Vector3.zero;
        LeanTween.scale(nextMenu, Vector3.one, transitionTime);
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

    public void CustomizeToPlay()
    {
        CloseMenu();
        Invoke("OpenNewMenu", MenuAnimations.transitionTime);
    }
}
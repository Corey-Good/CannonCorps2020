/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/20/20                                         */
/* Last Modified Date: 02/20/20                                         */
/* Modified By:        Jaben Calas                                      */
/************************************************************************/

using UnityEngine;

public class MainMenuAnimations : MonoBehaviour
{
    public GameObject mainMenu;

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    CloseMenu();
        //}
    }
    
    public void OpenMenu()
    {
        mainMenu.SetActive(true);

        mainMenu.transform.localScale = new Vector3(0, 0, 0);

        LeanTween.scale(mainMenu, new Vector3(1, 1, 1), 0.5f);
    }

    public void CloseMenu()
    {
        LeanTween.scale(mainMenu, new Vector3(0, 0, 0), 0.5f);

        Invoke("turnOffMenu", 0.5f);
    }

    public void turnOffMenu()
    {
        mainMenu.SetActive(false);
    }
}
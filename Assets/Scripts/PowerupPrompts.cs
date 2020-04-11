/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       4/06/2020                                        */
/* Last Modified Date: 4/11/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/

#region Libraries

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

#endregion

public class PowerupPrompts : MonoBehaviourPun
{
    #region Variables

    public  static bool     powerupAcquired;

    public  static string   powerupName;

    public  GameObject      PlayerUI;
    public  GameObject      TutorialUI;
    public  GameObject      panel;

    public  TextMeshProUGUI headingText;
    public  TextMeshProUGUI subtitleText;

    private Player          player;

    private string          sceneName;

    #endregion Variables

    private void Awake()
    {
        #region Changes TutorialUI to GameUI

        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "FFA")
        {
            PlayerUI.SetActive(true);
            TutorialUI.SetActive(false);
            TutorialPrompts.tutorialModeOn = false;
        }
        else
        {
            Destroy(this);
        }

        #endregion Changes TutorialUI to GameUI

        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (powerupAcquired && player.photonView.IsMine)
            DisplayPowerupText();
    }

    private void DisplayPowerupText()
    {
        panel.       gameObject.SetActive(true);
        headingText. gameObject.SetActive(true);
        subtitleText.gameObject.SetActive(true);

        headingText.text = string.Format("Acquired {0} powerup!", powerupName);

        switch (powerupName)
        {
            #region Active Powerups
            case "Reload":
                subtitleText.text = string.Format("Press {0} to activate {1} boost!", alphaFilter(KeyBindings.activateReloadBoost),   powerupName);
            break;

            case "Speed":
                subtitleText.text = string.Format("Press {0} to activate {1} boost!", alphaFilter(KeyBindings.activateMovementBoost), powerupName);
            break;
            #endregion

            #region Passive Powerups
            case "Health":
                subtitleText.text = string.Format("{0} restored!",  powerupName);
                break;

            case "Shield":
                subtitleText.text = string.Format("{0} activated!", powerupName);
                break;
            #endregion

            #region Bullet Powerups
            case "Dynamite":
            case "Freeze"  :
            case "Laser"   :
                subtitleText.text = string.Format("Press {0} to switch to {1} bullets!", KeyBindings.switchBulletType, powerupName);
            break;
            #endregion

            default:
                break;
        }

        Invoke("EndPowerupPrompts", 5);
    }

    public void EndPowerupPrompts()
    {
        powerupAcquired   = false;
        headingText. text = "";
        subtitleText.text = "";

        panel.       gameObject.SetActive(false);
        headingText. gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
    }

    private string alphaFilter(KeyCode oldKeyPress)
    {
        string newKeyPress = oldKeyPress.ToString();

        if (newKeyPress.Contains("Alpha"))
            newKeyPress = newKeyPress.Replace("Alpha", "");

        return newKeyPress;
    }
}
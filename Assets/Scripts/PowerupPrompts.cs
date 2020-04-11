/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       4/06/2020                                        */
/* Last Modified Date: 4/06/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerupPrompts : MonoBehaviour
{
    #region Variables

    public  static bool     powerupsOn;
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
        if (powerupsOn)
            DisplayPowerupText();
    }

    private void DisplayPowerupText()
    {
        panel.       gameObject.SetActive(true);
        headingText. gameObject.SetActive(true);
        subtitleText.gameObject.SetActive(true);

        if (powerupName.Contains("(Clone)"))
            powerupName = gameObject.name.Replace("(Clone)", "");

        if (powerupName.Contains("(Clone) (UnityEngine.GameObject)"))
            powerupName = gameObject.name.Replace("(Clone) (UnityEngine.GameObject)", "");

        if (powerupName.Contains("Bullet"))
            powerupName = gameObject.name.Replace("Bullet", "");

        if (powerupName.Contains("Bullets"))
            powerupName = gameObject.name.Replace("Bullets", "");

        headingText.text = string.Format("Acquired powerup: {0}", powerupName);

        switch (powerupName)
        {
            case "ReloadPowerUp":
            {
                subtitleText = string.Format("Use the {1} to activate Reload Boost.", KeyBindings.switchBulletType);
            } 
            break;

            case "ShieldPowerUp":
                subtitleText = "Shield is used automatically when you get hit!";
            break;

            case "SpeedPowerUp":
                subtitleText = string.Format("Use the {2} to activate Movement Boost", KeyBindings.switchBulletType);
            break;

            case "DynamiteBullet":
                subtitleText = string.Format("Use the {0} key to switch to Dynamite Bullet.", KeyBindings.switchBulletType);
            break;

            case "DynamiteBullets":
                subtitleText = string.Format("Use the {0} key to switch to Dynamite Bullets.", KeyBindings.switchBulletType);
            break;

            case "healthpowerup":
                subtitleText = "Gained some health back!";
            break;

            case "HealthPowerUp":
                subtitleText = "Gained some health back!";
            break;

            case "FreezeBullet":
                subtitleText = string.Format("Use the {0} key to switch to Freeze Bullet.", KeyBindings.switchBulletType);
            break;

            case "FreezeBullets":
                subtitleText = string.Format("Use the {0} key to switch to Freeze Bullets.", KeyBindings.switchBulletType);
            break;

            case "LaserBullet":
                subtitleText = string.Format("Use the {0} key to switch to Laser Bullet.", KeyBindings.switchBulletType);
            break;

            case "LaserBullets":
                subtitleText = string.Format("Use the {0} key to switch to Laser Bullets.", KeyBindings.switchBulletType);
            break;

            default:
                break;
        }

        Invoke("RemoveText", 5);
    }

    private void RemoveText()
    {
        powerupsOn        = false;
        headingText. text = "";
        subtitleText.text = "";

        panel.       gameObject.SetActive(false);
        headingText. gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
    }

    public void exitGameUI()
    {
        player.leaveGame = true;

        panel.       gameObject.SetActive(false);
        headingText. gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
    }
}
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

    public static bool powerupsOn;

    public GameObject PlayerUI;
    public GameObject TutorialUI;
    public GameObject panel;
    public TextMeshProUGUI headingText;
    public TextMeshProUGUI subtitleText;
    private Player player;
    private string sceneName;

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
        {
            PowerupText();
        }
    }

    private void PowerupText()
    {
        panel.gameObject.SetActive(true);
        headingText.gameObject.SetActive(true);
        subtitleText.gameObject.SetActive(true);

        headingText.text = "Acquired powerup!";
        subtitleText.text = string.Format(
                                "Use the {0} key to switch bullet types, {1} to activate Reload Boost and {2} to activate Movement Boost!",
                                KeyBindings.switchBulletType, KeyBindings.activateReloadBoost, KeyBindings.activateMovementBoost);

        Invoke("RemoveText", 5);
    }

    private void RemoveText()
    {
        powerupsOn = false;
        headingText.text = "";
        subtitleText.text = "";

        panel.gameObject.SetActive(false);
        headingText.gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
    }

    public void exitGameUI()
    {
        player.leaveGame = true;
        panel.gameObject.SetActive(false);
        headingText.gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
    }
}
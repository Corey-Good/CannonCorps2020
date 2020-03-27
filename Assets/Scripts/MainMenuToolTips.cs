/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       3/03/2020                                        */
/* Last Modified Date: 3/03/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/
using UnityEngine;
using TMPro;
public class MainMenuToolTips : MonoBehaviour
{
    public TextMeshProUGUI ToolTipsText;

    #region PlayMenu
    public void PlayMenuFFATips()
    {
        ToolTipsText.text = "Try to get as many points as possible by eliminating opponents!";
    }
    public void PlayMenuSMTips()
    {
       ToolTipsText.text =     "All players start off as minnows, with 1 player randomly becoming a shark." +
                           "\n\nThe shark will try to overpower as many minnows as possible, while minnows try to survive " +
                               "for as long as possible!";
    }
    public void PlayMenuTBTips()
    {
       ToolTipsText.text =     "Defeat players on the enemy team to earn points for your team. " +
                           "\n\nThe team that reaches the score limit, or has more points when time expires wins!";
    }
    public void PlayMenuTTTips()
    {
        ToolTipsText.text = "Click here to learn the basics!";
    }
    #endregion

    #region InfoMenu
    public void InfoMenuFFATips()
    {
        ToolTipsText.text = "In Free-For-All, every player in the match is an enemy. " +
            "\n\n\nPlayers no longer work in teams and must be dependent enough to survive and eliminate other players. " +
            "\n\n\nPlayers attempt to accumulate the most amount of points by hitting or eliminating other players. " +
            "\n\n\n10 Points are awarded for each hit.";

    }
    public void InfoMenuSMTips()
    {
        ToolTipsText.text = "In Sharks and Minnows, players are split into two teams: Sharks… and Minnows. " +
            "\n\n\nWhen a Shark defeats a Minnow, the Minnow switches to the Shark team. " +
            "\n\n\nA Minnow's goal is to remain alive until the end of the round, " +
            "while a Shark's goal is to defeat as many Minnows as possible. " +
            "\n\n\nThe round ends when either the last Minnow is defeated, or time runs out.";
    }
    public void InfoMenuTBTips()
    {
        ToolTipsText.text = "In Team Battle, players are split into two teams. " +
            "\n\n\nThe aim of the game mode is to eliminate players on the opposing team to gain points. " +
            "\n\n\nThe winner is the team that reaches the point limit first.";
    }
    public void InfoMenuTTTips()
    {
        ToolTipsText.text = "Learn the basics of gameplay, such as turret rotation, tank movement, aiming and firing.";
    }
    #endregion
}

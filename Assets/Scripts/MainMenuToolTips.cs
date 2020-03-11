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
        ToolTipsText.text = "Free For All, or FFA, is a Game Mode in"+" which players attempt to accumulate the most amount of points with no team for support; instead, they must rely on themselves. Points are scored by hitting or eliminating other players. 10 Points are awarded for each hit.";


    }
    public void InfoMenuSMTips()
    {
        ToolTipsText.text = "All players start off as minnows, with 1 player randomly becoming a shark." +
                            "\n\nThe shark will try to overpower as many minnows as possible, while minnows try to survive " +
                                "for as long as possible!";
    }
    public void InfoMenuTBTips()
    {
        ToolTipsText.text = "Defeat players on the enemy team to earn points for your team. " +
                            "\n\nThe team that reaches the score limit, or has more points when time expires wins!";
    }
    public void InfoMenuTTTips()
    {
        ToolTipsText.text = "Click here to learn the basics!";
    }
    #endregion
}

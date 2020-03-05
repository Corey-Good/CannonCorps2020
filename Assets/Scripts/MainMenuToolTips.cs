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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void FFATips()
    {
        ToolTipsText.text = "Try to get as many points as possible by eliminating opponents!";
    }
    public void SMTips()
    {
       ToolTipsText.text =     "All players start off as minnows, with 1 player randomly becoming a shark." +
                           "\n\nThe shark will try to overpower as many minnows as possible, while minnows try to survive " +
                               "for as long as possible!";
    }
    public void TBTips()
    {
       ToolTipsText.text =     "Defeat players on the enemy team to earn points for your team. " +
                           "\n\nThe team that reaches the score limit, or has more points when time expires wins!";
    }
    public void TTTips()
    {
        ToolTipsText.text = "Click here to learn the basics!";
    }
}

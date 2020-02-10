using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    private Tank tank;
    private Player player;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScoreText;
    public Slider healthBar;

    void Awake()
    {
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        playerScoreText.text = "0";
    }

    private void Start()
    {
        playerName.text = player.PlayerName;
        playerScoreText.text = player.ScoreCurrent.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.value = tank.healthCurrent / tank.healthMax;
        playerScoreText.text = player.ScoreCurrent.ToString();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

    public Text killsText;
    public Text crashesText;
    public Text combosText;
    public Text scoreText;

    private bool[] reveal;

    private int crashes;
    private int kills;
    private int tokens;
    private int combo;

    private int crashDisplayInt;
    private int killsDisplayInt;
    private int tokensDisplayInt;
    private int comboDisplayInt;
    private int scoreDisplayInt;

    private int revealIndex;

    private int score;

	// Use this for initialization
	void Start () {
        crashes = Scores.Crashes;
        kills = Scores.Kills;
        combo = Scores.Combos;

        reveal = new bool[4];

        score = kills * combo;

        crashDisplayInt = 0;
        killsDisplayInt = 0;
        comboDisplayInt = 0;
        scoreDisplayInt = 0;

    }
	
	// Update is called once per frame
	void Update () {
		switch (revealIndex)
        {
            case 0:
                if (killsDisplayInt < kills)
                {
                    killsDisplayInt += 1;
                    killsText.text = "" + killsDisplayInt;
                }
                else
                {
                    revealIndex += 1;
                }
                break;
            case 1:
                if (crashDisplayInt < crashes)
                {
                    crashDisplayInt += 1;
                    crashesText.text = "" + crashDisplayInt;
                }
                else
                {
                    revealIndex += 1;
                }
                break;
            case 2:
                if (comboDisplayInt < combo)
                {
                    comboDisplayInt += 1;
                    combosText.text = "" + comboDisplayInt;
                }
                else
                {
                    revealIndex += 1;
                }
                break;
            case 3:
                if (scoreDisplayInt < score)
                {
                    scoreDisplayInt += 1;
                    scoreText.text = "" + scoreDisplayInt;
                }
                else
                {
                    revealIndex += 1;
                }
                break;
            default:
                break;

        }
	}

    public void LoadNewScene()
    {
        SceneManager.LoadScene("main");
    }
}

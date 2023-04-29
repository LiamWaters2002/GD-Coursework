using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighScore : MonoBehaviour
{
    public Text score;
    ScoreController scoreController;

    // Start is called before the first frame update
    void Start()
    {
        scoreController = ScoreController.Instance;
    }

    private void Update()
    {
        if (score.text.Equals("0"))
        {
            scoreController.GetHighScore();
            score.text = scoreController.GetLatestHighScore().ToString();
        }

    }

}

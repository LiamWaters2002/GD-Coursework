using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighScore : MonoBehaviour
{
    Text score;

    // Start is called before the first frame update
    void Start()
    {
        ScoreController scoreController = new ScoreController();
        score.text = scoreController.GetLatestHighScore().ToString();
    }

}

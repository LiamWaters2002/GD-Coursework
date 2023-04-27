using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using UnityEngine.UI;

public class HighScoreContainer
{
    public int Score;
    public string code;
    public string message;
}


public class ScoreController : MonoBehaviour
{
    public Text score;

    public int latestHighScore;

    public static ScoreController Instance { get; private set; }

    private IEnumerator coroutineSend;
    private IEnumerator coroutineReceive;

    public const string APPLICATION_ID = "33484F83-4CF2-E01B-FF53-154523288300"; 
    public const string REST_SECRET_KEY = "2CED0861-E44B-4C02-A36F-BB20A469E0D7";

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }

        // don't destroy this object when we load scene
        DontDestroyOnLoad(gameObject);
    }

    public void incrementScore()
    {
        int intScore = int.Parse(score.text);
        intScore = intScore + 1;
        score.text = intScore.ToString();
    }

    public int GetLatestHighScore()
    {
        coroutineReceive = GetHighScoreCR();
        StartCoroutine(coroutineReceive);
        return latestHighScore;
    }

    public void GetHighScore()
    {
        coroutineReceive = GetHighScoreCR();
        StartCoroutine(coroutineReceive);

    }

    public void SetHighScore(int score)
    {
        coroutineSend = SetHighScoreCR(score);
        StartCoroutine(coroutineSend);

    }

    public IEnumerator GetHighScoreCR()
    {

        string strTableName = "HighScore";

        const string objectID = "3860E3BF-2B6D-41E1-9487-EE179E93D6AF";
        string url = "https://api.backendless.com/" +
                    APPLICATION_ID + "/" +
                    REST_SECRET_KEY +
                    "/data/" +
                    strTableName +
                    "/" +
                    objectID +
                    "";

        UnityWebRequest webreq = UnityWebRequest.Get(url);


        // TODO #4 - set the request headers as dictated by the backendless documentation (3 headers)
        webreq.SetRequestHeader("application-id", APPLICATION_ID);
        webreq.SetRequestHeader("secret-key", REST_SECRET_KEY);
        webreq.SetRequestHeader("application-type", "REST");

        // TODO #5 - Send the webrequest and yield (so the script waits until it returns with a result)
        yield return webreq.SendWebRequest();

        // TODO #6 - check for webrequest errors
        if (webreq.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("ConnectionError");
        }
        else if (webreq.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ProtocolError");
        }
        else if (webreq.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("DataProcessingError");
        }
        else if (webreq.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success2");
            Debug.Log(webreq.downloadHandler.text);
            HighScoreContainer highScoreContainer = JsonUtility.FromJson<HighScoreContainer>(webreq.downloadHandler.text);
            
            latestHighScore = highScoreContainer.Score;

            //Up to here is fine...........................................................................................

        }
        else
        {
            // TODO #7 - Convert the downloadHandler.text property to HighScoreContainer (currently JSON)
            HighScoreContainer highScoreData = JsonUtility.FromJson<HighScoreContainer>(webreq.downloadHandler.text);

            // TODO #8 - check that there are no backendless errors
            if (!string.IsNullOrEmpty(highScoreData.code))
            {
                Debug.Log("Error:" + highScoreData.code + " " + highScoreData.message);
            }
        }
    }

    // TODO #1 - change the signature to be a Coroutine, add callback parameter
    public IEnumerator SetHighScoreCR(int score)
    {
        string strTableName = "HighScore";

        const string objectID = "3860E3BF-2B6D-41E1-9487-EE179E93D6AF";

        // TODO #2 - construct the url for our request, including objectid!
        string url = "https://api.backendless.com/" +
                    APPLICATION_ID + "/" +
                    REST_SECRET_KEY +
                    "/data/" +
                    strTableName +
                    "/" +
                    objectID +
                    "";

        ;


        // TODO #3 - construct JSON string for data we want to send
        string data = JsonUtility.ToJson(new HighScoreContainer { Score = score });

        // TODO #4 - create PUT UnityWebRequest passing our url and data
        UnityWebRequest webreq = UnityWebRequest.Put(url, data);

        // TODO #5 set the request headers as dictated by the backendless documentation (4 headers)
        webreq.SetRequestHeader("Content-Type", "application/json");
        webreq.SetRequestHeader("application-id", APPLICATION_ID);
        webreq.SetRequestHeader("secret-key", REST_SECRET_KEY);
        webreq.SetRequestHeader("application-type", "REST");

        // TODO #6 - Send the webrequest and yield (so the script waits until it returns with a result)
        yield return webreq.SendWebRequest();

        // TODO #7 - check for webrequest errors
        if (webreq.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("ConnectionError");
        }
        else if (webreq.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ProtocolError");
        }
        else if (webreq.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("DataProcessingError");
        }
        else if (webreq.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success1");
        }
        else
        {
            // TODO #7 - Convert the downloadHandler.text property to HighScoreContainer (currently JSON)
            HighScoreContainer highScoreData = JsonUtility.FromJson<HighScoreContainer>(webreq.downloadHandler.text);

            // TODO #8 - check that there are no backendless errors
            if (!string.IsNullOrEmpty(highScoreData.code))
            {
                Debug.Log("Error:" + highScoreData.code + " " + highScoreData.message);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// Represents a High Score entry
/// </summary>
public class HighScore
{
    public int score;
}

public class HightScoreList
{
    public List<HighScore> highscoreList;

    public void Sort() // Bubble sort
    {
        for (int i = 0; i < highscoreList.Count; i++)
        {
            for (int j = 0; j < highscoreList.Count; j++)
            {
                if (highscoreList[j].score > highscoreList[i].score)
                {
                    HighScore temp = highscoreList[i];
                    highscoreList[i] = highscoreList[j];
                    highscoreList[j] = temp;
                }
            }
        }
    }
}
public class HighScoreManager : MonoBehaviour
{

    public HightScoreList GetHighscores()
    {
        string jsonString = PlayerPrefs.GetString("highscores");
        return JsonUtility.FromJson<HightScoreList>(jsonString);
    }

    /// <summary>
    /// Adds a new entry to the PlayerPrefs under key "highscores". Keeps track of up 5 five highscores. Automatically sorts the list.
    /// </summary>
    /// <param name="_score"></param>
    public void AddHighScore(int _score)
    {
        // Initialize a new entry
        HighScore newScore = new HighScore { score = _score };

        HightScoreList highScores = GetHighscores();

        // If failed to get it, initialize
        if (highScores == null)
        {
            highScores = new HightScoreList { highscoreList = new List<HighScore>() };
        }

        // Add new entry
        highScores.highscoreList.Add(newScore);

        // Sort the list
        highScores.Sort();

        // If there are now more than 5 high scores, removing the one at index 0 will remove the lowest score since the list is sorted.
        if (highScores.highscoreList.Count > 5)
            highScores.highscoreList.RemoveAt(0);

        // Store new table
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highscores", json);
        PlayerPrefs.Save();

    }

    /// <summary>
    /// </summary>
    /// <param name="_score"></param>
    /// <returns>true if _score is in top five of the high scores, false otherwise</returns>
    public bool IsTopFive(int _score)
    {
        HightScoreList highScores = GetHighscores();
        // If there are no entries, it is in top five
        if (highScores == null) return true;

        // Returns whether or not there is an high score lower than the given one
        return highScores.highscoreList.Exists(s => s.score < _score);
    }

    void Test()
    {
        Debug.Log(PlayerPrefs.GetString("highscores"));
    }
}


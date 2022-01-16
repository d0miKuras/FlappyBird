using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    public int Score { get; private set; }


    // Start is called before the first frame update
    void Awake()
    {
        Score = 0;
        scoreText.text = $"{Score}";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementScore()
    {
        Score++;
        scoreText.text = $"{Score}";
        Debug.Log("Score Increased");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private int score;
    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        scoreText.text = $"{score}";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = $"{score}";
        Debug.Log("Score Increased");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bombCountText;
    public int Score { get; private set; }
    public int BombCount { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Score = 0;
        scoreText.text = Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementScore()
    {
        Score++;
        scoreText.text = $"{Score}";
        if (Score % 10 == 0 && BombCount < 3)
        {
            BombCount++;
            bombCountText.text = BombCount.ToString();
        }

    }
    public void DecrementBomb()
    {
        BombCount--;
    }

}

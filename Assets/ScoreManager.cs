using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private UnityEvent<int> _onScoreUpdated = new();
    private int _score = 0;


    public void AddScore(int score)
    {
        _score += score;
        _text.text = "Score : " + _score.ToString();
        _onScoreUpdated.Invoke(_score);
    }
    
    public void ResetScore()
    {
        _score = 0; 
        _text.text = "Score : " + _score.ToString();
        _onScoreUpdated.Invoke(_score);
    }
}

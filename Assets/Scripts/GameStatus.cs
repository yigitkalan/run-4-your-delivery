using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatus : MonoBehaviour
{
    [SerializeField]private Text timerText;
    [SerializeField]private Text scoreText;
    
    [SerializeField] private float gameTime = 61;
    [SerializeField] private float score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
       gameTime -= 1 * Time.deltaTime;

        if (isThereTime()) //if there is still time
        {
            int minute = (int)gameTime / 60; //Calculating minute and seconds
            int second = (int)gameTime % 60;
            if (second / 10 < 1)   // if seconds are single digit we put a zero in front of 'em
                timerText.text = minute + ":0" + second;
            else
                timerText.text = minute + ":" + second;
        }

    }
    public bool isThereTime() {
        if(gameTime > 0)
            return true;
        return false;
    }

    public void addScore() {
        score += 5 * Time.deltaTime;
        scoreText.color = Color.white;

        if(isThereTime())
        {
            scoreText.text = "Score : " + (int) score;
        }
    }

    public void addScore(int i)
    {
        score += i;
    }

    public void punish(int punishment) {
        if(isThereTime())
        {
            if(score - punishment < 0) 
                score = 0;
            else
                score -= punishment;
            scoreText.text = "Score : " + (int) score;
            scoreText.color = Color.red;

        }
    }
    public void resetTimer() {
        gameTime = 61;
    }

    public int getTimeLeft(){
        return (int) gameTime;
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;
using Mono.Cecil.Cil;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    int score = 0;
    void Awake()
    {
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsInactive.Include).Length; //FindObjectsByType<GameSession>(FindObjectsSortMode.None) is deprecated.
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        livesText.text = $"Lives: {playerLives}";
        scoreText.text = $"Score: {score}";
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = $"Score: {score}";
    }

    void TakeLife()
    {
        playerLives --;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = $"Lives: {playerLives}";
        scoreText.text = $"Score: {score}";
    }
    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
        scoreText.text = "Score: 0";
    }
}

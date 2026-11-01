using UnityEngine;

public class GameSession : MonoBehaviour
{
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
        
    }

    void Update()
    {
        
    }
}

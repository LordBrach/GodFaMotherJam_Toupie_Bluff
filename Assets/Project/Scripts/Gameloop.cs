using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Gameloop : MonoBehaviour
{   
    // singleton pattern
    private static Gameloop instance;

    // timer
    [SerializeField] private float gameTimeInSeconds = 60.0f;
    [SerializeField] private TMP_Text TMPTimer;

    private float remainingGameTime = 0.0f;
    private bool timerEnabled = false;
    private bool SuddenDeathEnabled;

    // score
    [SerializeField] private ScoreBar score;
    private int currentspinForce = 0;
    private int maxSpinForce = 100;
    private int minSpinForce = -100;

    // debug
    [SerializeField] private bool debugStartImmediately = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (debugStartImmediately)
            StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }
    private void UpdateTimer()
    {
        if (timerEnabled) return;
        TMPTimer.text = ((int)remainingGameTime).ToString();
        remainingGameTime -= Time.deltaTime;

        if(!SuddenDeathEnabled && remainingGameTime <= 15)
        {
            Debug.Log("Enable suddendeath");
            SuddenDeathEnabled = true;
        }
        if (remainingGameTime <= 0)
        {
            Debug.Log("End Game");
            remainingGameTime = 0;
            EndGame();
        }
    }


    void StartGame()
    {
        // reset timer
        remainingGameTime = gameTimeInSeconds;
        // start timer
        timerEnabled = true;
        score.SetupScoreBar();
        // enable player input
    
    }

    private void UpdateScore()
    {

    }

    void EndGame()
    {
        // Block Player input (send event)
        // Show End Screen UI
    }
}

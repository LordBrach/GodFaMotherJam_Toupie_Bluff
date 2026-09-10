using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;
public class Gameloop : MonoBehaviour
{   
    // singleton pattern
    private static Gameloop instance;
    // refs
    [SerializeField] PlayerControls playerControls;
    [SerializeField] Toupie toupie;

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

    // event
    public UnityEvent OnStartGame;
    public UnityEvent OnEndGame;
    public UnityEvent OnSuddenDeathStart;

    // debug
    [SerializeField] private bool debugStartImmediately = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        //singleton
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

    private void OnEnable()
    {
        playerControls.OnPlayerInput.AddListener(UpdateScore);
        playerControls.OnPlayerCounter.AddListener(UpdateScore);

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
        if (!timerEnabled) return;
        TMPTimer.text = ((int)remainingGameTime).ToString();
        remainingGameTime -= Time.deltaTime;

/*        if(!SuddenDeathEnabled && remainingGameTime <= 15)
        {
            Debug.Log("Enable suddendeath");
            SuddenDeathEnabled = true;
        }*/
        if (remainingGameTime <= 0)
        {
            timerEnabled = false;
            Debug.Log("Mort subite");
            remainingGameTime = 0;
            SuddenDeathEnabled = true;
            OnSuddenDeathStart.Invoke();
        }
    }


    public void StartGame()
        // reset timer
    {
        remainingGameTime = gameTimeInSeconds;
        // start timer
        OnStartGame.Invoke();
        timerEnabled = true;
        toupie.StartToupie();
        score.SetupScoreBar();
        // enable player input
    
    }

    private void UpdateScore(Player Attacker, int dmgValue)
    {
        if (!timerEnabled) return;

        if (SuddenDeathEnabled)
            dmgValue = dmgValue * 2;

        currentspinForce += dmgValue;
        score.UpdateScoreBar(currentspinForce);
        toupie.UpdateSpeed(currentspinForce);
        if(currentspinForce <= minSpinForce)
        {
            //currentspinForce = 0;
            EndGame(Player.PlayerOne);
        } else if (currentspinForce >= maxSpinForce)
        {
            //currentspinForce = 0;
            score.UpdateScoreBar(currentspinForce);
            EndGame(Player.PlayerTwo);
        } 
/*        else
        {
            score.UpdateScoreBar(currentspinForce);
        }*/
    }

    void EndGame(Player Winner)
    {
        OnEndGame.Invoke();
        Debug.Log("Winner is: " + Winner.ToString());
        // Block Player input (send event)
        playerControls.DisableInputs();
        timerEnabled = false;
        // TODO Show End Screen UI

    }
}

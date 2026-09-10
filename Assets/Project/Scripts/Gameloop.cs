using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class Gameloop : MonoBehaviour
{   
    // singleton pattern
    private static Gameloop instance;
    public static Gameloop public_instance;
    // refs
    [Header("Refs")]
    [SerializeField] PlayerControls playerControls;
    [SerializeField] Toupie toupie;

    // timer
    [Header("Timer")]
    [SerializeField] private float gameTimeInSeconds = 60.0f;
    [SerializeField] private TMP_Text TMPTimer;

    private float remainingGameTime = 0.0f;
    private bool timerEnabled = false;
    private bool SuddenDeathEnabled;
    private bool gameRunning = false;
    // score
    [Header("Score")]
    [SerializeField] private ScoreBar score;
    private int currentspinForce = 0;
    private int maxSpinForce = 100;
    private int minSpinForce = -100;
    // random events
    [Header("Random Events")]
    [SerializeField] private float minTimeBeforeEvent = 0.5f;
    [SerializeField] private float maxTimeBeforeEvent = 0.5f;
    [SerializeField] private float eventDuration = 3f;
    [SerializeField] private EventTypes[] ActiveEvents;
    [SerializeField] private GameObject Popup;
    private bool isRandomEventRunning = false;
    private IEnumerator coroutineEvent;

    // event
    public UnityEvent OnStartGame;
    public UnityEvent OnEndGame;
    public UnityEvent OnSuddenDeathStart;
    public UnityEvent<EventTypes> OnCallRandomEvent;

    // debug
    [Header("Debug")]
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
            public_instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        Popup.SetActive(false);
        playerControls.OnPlayerInput.AddListener(UpdateScore);
        playerControls.OnPlayerCounter.AddListener(UpdateScore);

    }

    void Start()
    {
        if (debugStartImmediately)
            StartGame();
    }
    public void StartGame()
    {
        // reset timer
        gameRunning = true;
        remainingGameTime = gameTimeInSeconds;
        // start timer
        OnStartGame.Invoke();
        timerEnabled = true;
        toupie.StartToupie();
        score.SetupScoreBar();
        SetupNextEvent();
    }

    private void SetupNextEvent()
    {
        if(gameRunning)
        {
            float randValue = Random.Range(minTimeBeforeEvent* gameTimeInSeconds, maxTimeBeforeEvent* gameTimeInSeconds);
            EventTypes randEventSelected = ActiveEvents[Random.Range(0, ActiveEvents.Length - 1)];

            Debug.Log("Next event in: " + randValue + " seconds, type is: " + randEventSelected.ToString());
            coroutineEvent = EventCaller(randValue, randEventSelected);
            StartCoroutine(coroutineEvent);
        }
        // Coroutine(TempsRandom, 
    }

    IEnumerator EventCaller(float timeToWait, EventTypes eventType)
    {
        yield return new WaitForSeconds(timeToWait);
        OnCallRandomEvent.Invoke(eventType);
        switch (eventType)
        {
            case EventTypes.InvertInputs:
                playerControls.Inverted = true;
                Popup.SetActive(true);
                break;
            case EventTypes.BlockInputsTODO:
                break;
            case EventTypes.MashKeyboardTODO:
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(eventDuration);
        switch (eventType)
        {
            case EventTypes.InvertInputs:
                playerControls.Inverted = false;
                Popup.SetActive(false);
                break;
            case EventTypes.BlockInputsTODO:
                break;
            case EventTypes.MashKeyboardTODO:
                break;
            default:
                break;
        }
        if (gameRunning)
            SetupNextEvent();
        yield return null;
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

    private void UpdateScore(Player Attacker, int dmgValue)
    {
        //if (!timerEnabled) return;
        if (!gameRunning) return;
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
        if (gameRunning == false)
            return;

        OnEndGame.Invoke();
        gameRunning = false;
        Debug.Log("Winner is: " + Winner.ToString());
        // Block Player input (send event)
        playerControls.DisableInputs();
        timerEnabled = false;
        // TODO Show End Screen UI

    }
}

public enum EventTypes
{
    DEFAULT,
    InvertInputs,
    BlockInputsTODO,
    MashKeyboardTODO,
}

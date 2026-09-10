using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class PlayerControls : MonoBehaviour
{

    private PlayerInput inputs = null;
    [SerializeField] float WeakTime = 1.0f;
    [SerializeField] int BaseDmg = 2;
    [SerializeField] int WeakDMGMultiplier = 5;
    [SerializeField] Hand handPlayerOne;
    [SerializeField] Hand handPlayerTwo;

    private bool PlayerOneWeak = false;
    private bool PlayerTwoWeak = false;
    private IEnumerator coroutineOne;
    private IEnumerator coroutineTwo;

    public bool Inverted = false;
    //Events
    public UnityEvent<Player, int> OnPlayerInput;
    public UnityEvent<Player, int> OnPlayerCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputs = new PlayerInput();
        //inputs.Enable();

        inputs.Default.PlayerOne.performed += ctx => PlayerOneInput();

        inputs.Default.PlayerTwo.performed += ctx => PlayerTwoInput();
    }

    public void EnableInputs() => inputs.Enable();

    public void DisableInputs()
    {
        inputs.Disable();
        inputs.Default.PlayerOne.performed -= ctx => PlayerOneInput();

        inputs.Default.PlayerTwo.performed -= ctx => PlayerTwoInput();
    }
    //new []{1,2}

    private void PlayerOneInput()
    {
        int i = 1;
        if (Inverted)
            i = -1;
        handPlayerOne.CallAttack();
        Debug.Log("Player one input");
        if (PlayerTwoWeak)
        {
            PlayerTwoWeak = false;
            Debug.Log("Counter");
            OnPlayerCounter.Invoke(Player.PlayerOne, -BaseDmg * WeakDMGMultiplier * i);
        }
        else
        {
            if(coroutineOne != null)
                StopCoroutine(coroutineOne);
            coroutineOne = TimerPlayerOne();
            StartCoroutine(coroutineOne);
            OnPlayerInput.Invoke(Player.PlayerOne, -BaseDmg * i);

        }
    }

    private void PlayerTwoInput()
    {
        int i = 1;
        if (Inverted)
            i = -1;
        handPlayerTwo.CallAttack();
        Debug.Log("Player two input");
        if (PlayerOneWeak)
        {
            PlayerOneWeak = false;
            Debug.Log("Counter");
            OnPlayerCounter.Invoke(Player.PlayerTwo, BaseDmg * WeakDMGMultiplier * i);
        }
        else
        {
            if (coroutineTwo != null)
                StopCoroutine(coroutineTwo);
            coroutineTwo = TimerPlayerTwo();
            StartCoroutine(coroutineTwo);
            OnPlayerInput.Invoke(Player.PlayerTwo, BaseDmg * i);
        }
    }

    IEnumerator TimerPlayerOne()
    {
        PlayerOneWeak = true;
        Debug.Log("Player one is weak");
        yield return new WaitForSeconds(WeakTime);
        PlayerOneWeak = false;
        Debug.Log("Player one isnt weak");
        yield return null;

    }

    IEnumerator TimerPlayerTwo()
    {
        PlayerTwoWeak = true;
        Debug.Log("Player two is weak");
        yield return new WaitForSeconds(WeakTime);
        PlayerTwoWeak = false;
        Debug.Log("Player two isnt weak");
        yield return null;

    }
}

public enum Player
{
    PlayerOne,
    PlayerTwo
}

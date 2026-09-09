using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerControls : MonoBehaviour
{

    private PlayerInput inputs = null;
    [SerializeField] float WeakTime = 1.0f;
    [SerializeField] float WeakDMGMultiplier = 2.0f;

    private bool PlayerOneWeak = false;
    private bool PlayerTwoWeak = false;
    private IEnumerator coroutineOne;
    private IEnumerator coroutineTwo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputs = new PlayerInput();
        inputs.Enable();

        inputs.Default.PlayerOne.performed += ctx => PlayerOneInput();

        inputs.Default.PlayerTwo.performed += ctx => PlayerTwoInput();
    }


    private void PlayerOneInput()
    {
        Debug.Log("Player one input");
        if (PlayerTwoWeak)
        {
            Debug.Log("Counter");
        }
        else
        {
            if(coroutineOne != null)
                StopCoroutine(coroutineOne);
            coroutineOne = TimerPlayerOne();
            StartCoroutine(coroutineOne);
        }
    }

    private void PlayerTwoInput()
    {
        Debug.Log("Player two input");
        if (PlayerOneWeak)
        {
            Debug.Log("Counter");
        }
        else
        {
            if (coroutineTwo != null)
                StopCoroutine(coroutineTwo);
            coroutineTwo = TimerPlayerTwo();
            StartCoroutine(coroutineTwo);
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
        PlayerOneWeak = false;
        Debug.Log("Player two isnt weak");
        yield return null;

    }
}

public enum Player
{
    PlayerOne,
    PlayerTwo
}

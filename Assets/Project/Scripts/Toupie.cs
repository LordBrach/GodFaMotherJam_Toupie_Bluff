using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Toupie : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float baseSpeed = 2f;
    private float currentSpeed;
    [SerializeField] float maxSpeed = 10f;
    public UnityEvent ToupieStop;
    public UnityEvent ToupieStart;

    public bool debugLogs = false;
    public void StartToupie()
    {
        currentSpeed = 0f;
        ToupieStart.Invoke();
        UpdateSpeed(baseSpeed);
    }


    public void UpdateSpeed(float target)
    {
        target = CalcToupieVal(-100, 100, target);
        if (target >= maxSpeed)
            target = maxSpeed;
        if(target <= 0)
        {
            ToupieStop.Invoke();
            // Play anim ou autre chose;
        }
        currentSpeed = target;
        animator.SetFloat("SpeedValue", currentSpeed);
        if(debugLogs)
            Debug.Log("Toupie speed:" + currentSpeed);
    }

    private float CalcToupieVal(float min, float max, float inVal)
    {
        // x (m, M, i)
        // 0,5  (-100, 100, 0)
        // -100 = 0
        // 100 = 10
        // InLerp(0, Max, 0,5) => 5
        float x  = Mathf.InverseLerp(min, max, inVal);
        float y = Mathf.Lerp(0, maxSpeed, x);
        return y;

    }
}

using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Toupie : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject toupieParent;
    [SerializeField] float baseSpeed = 2f;
    private float currentSpeed;
    [SerializeField] float maxSpeed = 10f;
    public UnityEvent ToupieStop;
    public UnityEvent ToupieStart;
    [SerializeField]
    private float amplitude = 10.0f;

    [SerializeField]
    private float frequency = 2.0f;
    private bool fall = false;
    private float angle = 0;

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
        {
            target = maxSpeed;
            animator.SetTrigger("VictoryFly");
        }
        if (target <= 0)
        {
            Debug.Log("Stop");
            animator.SetFloat("SpeedValue", 1.0f);
            ToupieStop.Invoke();
            fall = true;
            // Play anim ou autre chose;
            animator.SetTrigger("VictoryStop");
            return;
        }
        currentSpeed = target;
        animator.SetFloat("SpeedValue", currentSpeed);
        if(debugLogs)
            Debug.Log("Toupie speed:" + currentSpeed);
    }

    private void Update()
    {
        if(toupieParent && fall == false)
        {
            angle = Mathf.Sin(Time.time * frequency) * amplitude * 1 / currentSpeed;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            toupieParent.transform.rotation = Quaternion.identity * rotation;
        }
    }

/*    IEnumerable Fall()
    {

    }*/

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

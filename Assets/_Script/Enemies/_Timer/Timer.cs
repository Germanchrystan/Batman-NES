using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private TimerDurationSO timerDurationSO;
    [SerializeField] private UnityEvent TimesUpEvent;
    private float initialTime = 0f;
    private float maxTime;
    private bool _isActive;
    void Awake()
    {
        maxTime = timerDurationSO.time;
        initialTime = maxTime;
    }
    void Update()
    {
        if (_isActive)
        {
            if(initialTime <= 0)
            {
                TimesUpEvent.Invoke();
                initialTime = maxTime;
            }
            else
            {
                initialTime -= Time.deltaTime;
            }
        }
    }
    public void ActivateTimer()
    {
        _isActive = true;
        initialTime = maxTime;
    }
    public void DeactivateTimer()
    {
        _isActive = false;
    }
    void OnEnable()
    {
        ActivateTimer();
    }
}

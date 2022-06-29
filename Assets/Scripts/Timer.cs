using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float currentTime;

    private float resetTime = 0f;

    private void Update()
    {
        currentTime = currentTime += Time.deltaTime;
        SetTimerText();

        //if (currentTime >= 3f)
        //{
        //    Reset();
        //}

    }

    private void SetTimerText()
    {
        timerText.text = currentTime.ToString("0.00");
    }

    private void Reset()
    {
        currentTime = resetTime;
        enabled = false;
        SetTimerText();
    }

    private void Pause()
    {
        enabled = false;
    }

    private void Play()
    {
        enabled = true;
    }
}
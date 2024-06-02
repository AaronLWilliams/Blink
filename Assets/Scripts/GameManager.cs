using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    private bool isTimerRunning = true;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //UI for timer
        if (isTimerRunning)
        {
            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            timerText.text = minutes + ":" + seconds;
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        startTime = Time.time;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
    }
}

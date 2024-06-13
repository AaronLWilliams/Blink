using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    private bool isTimerRunning = true;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        string levelName = SceneManager.GetActiveScene().name;
        SaveSystem.UnlockLevel(levelName);
    }

    // Update is called once per frame
    void Update()
    {
        //UI for timer
        if (isTimerRunning)
        {
            time = Time.time - startTime;

            string minutes = ((int)time / 60).ToString("00");
            string seconds = (time % 60).ToString("00");
            string milliseconds = ((int)((time * 1000) % 1000)).ToString("000");

            timerText.text = minutes + ":" + seconds + ":" + milliseconds;
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

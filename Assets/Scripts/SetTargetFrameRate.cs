using System.Collections;
using System.Threading;
using UnityEngine;

public class SetTargetFrameRate : MonoBehaviour
{
    [Header("Frame Settings")]
    int MaxRate = 9999;
    public float TargetFrameRate = 60.0f;
    float currentFrameTime;
    public float interval;
    private float timer;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }

    void Update()
    {
        // Accumulate time passed
        timer += Time.deltaTime;

        // Check if it's time to log a message and control the frame rate
        if (timer >= interval)
        {
            float frameRate = 1.0f / Time.deltaTime;
            Debug.Log($"Frame rate: {frameRate} FPS");

            // Limit the frame rate
            Application.targetFrameRate = (int)TargetFrameRate;

            // Reset the timer
            timer = 0.0f;
        }
        else
        {
            // Ensure the frame rate is unlimited when not logging
            Application.targetFrameRate = MaxRate;
        }
    }
}

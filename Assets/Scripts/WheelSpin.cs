using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSpin : MonoBehaviour
{
    public Sprite BackDark;
    public Sprite BackLight;
    public GameObject wheel;
    // public GameObject indicator;
    public GameObject share;
    public Sprite ShareDark;
    public Sprite ShareLight;
    public float BaseSpinSpeed = 4000f; // Initial spin speed
    public float spinDuration = 1.25f; // Set the spin duration to 2 seconds
    public string desiredOutput = "";
    public string Output = "";

    private bool isSpinning = false;
    private float totalRotation = 0f;
    private bool isLIGHTOutput;
    private float spinStartTime;
    public float BaseSpinDuration = 1.3f; // Base spin duration in seconds
    public float timeDifference = 0.12f;   // Adjustment in seconds (positive or negative)
    private float currentSpinSpeed;

    public TMP_Text title;
    public TMP_Text Result;
    public Image panelBackground;
    public AudioSource audioSource;
    public AudioSource TickAudioSource;

    public float friction = 0.9f;
    private Color originalTextColor1;
    private Color originalTextColor2;
    private Coroutine animationCoroutine;
    public Vector3 targetScale = new Vector3(2f, 2f, 1f);
    private Vector3 initialScale;
    private float rotationTick = 0;
    private float desiredDegree;
    private bool randomOutput;
    private void Update()
    {
        if (isSpinning)
        {
            float elapsedTime = Time.time - spinStartTime;

            if (elapsedTime >= spinDuration)
            {
                currentSpinSpeed = 0f;
                isSpinning = false;
        Invoke("StartTextAnimation", 0.5f);
            }
            else
            {
                float t = elapsedTime / spinDuration;
                currentSpinSpeed = Mathf.Lerp(BaseSpinSpeed, 0f, t);

                float rotationAmount = currentSpinSpeed * Time.deltaTime;
                wheel.transform.Rotate(Vector3.back, rotationAmount);
                float previousRotation = totalRotation;
                totalRotation += rotationAmount;
                float rotationDifference = Mathf.Abs(totalRotation - previousRotation);
                rotationTick += rotationDifference;
                // Debug.Log("T: " + t + " CurrentSpinSpeed: " + currentSpinSpeed + " totalRotation: " + totalRotation + " rotationTick: " + rotationTick + " desiredDegree: " + desiredDegree);
                CheckResult();

                if (rotationTick >= 45f)
                {
                    TickAudioSource.Play();
                    rotationTick = 0;
                }
            }
        }
    }

    public void OnSpinButtonClick()
    {
        if (!isSpinning)
        {
            desiredOutput = randomOutput ? ((Random.Range(0f, 1f) < 0.5f) ? "LIGHT" : "DARK") : desiredOutput;

            desiredDegree = (desiredOutput == "DARK") ? Random.Range(-140f, 220f) : Random.Range(221f, 580f);
            // desiredDegree = 580f;
            
            isSpinning = true;
            spinStartTime = Time.time;

            totalRotation = 0;

            // Calculate the required rotation to reach the desired degree
            float totalDegreesToRotate = desiredDegree;

            // Ensure the rotation is positive
            // totalDegreesToRotate = (totalDegreesToRotate + 360f) % 360f;
            totalDegreesToRotate += 360.0f * 4;
            // Calculate the spinDuration based on the desired rotation
            spinDuration = totalDegreesToRotate / BaseSpinSpeed;

            // while (spinDuration <= 0.5)
            // {
            //     totalDegreesToRotate += 360.0f;
            //     spinDuration = totalDegreesToRotate / BaseSpinSpeed;
            // }

            // Use the calculated spinDuration to control the spin time.
            currentSpinSpeed = BaseSpinSpeed;
            totalRotation = 0f;
            wheel.transform.eulerAngles = Vector3.zero;
            // Debug.Log("Spin Speed: " + currentSpinSpeed);
            share.SetActive(false);
        }
    }

    public void OnExitButtonClick()
    {
        // This function will be called when the ExitButton is clicked
        Application.Quit();
    }

    private void CheckResult()
    {
        // Determine the result based on the wheel's rotation.
        float normalizedRotation = totalRotation % 360f;

        if (normalizedRotation <= 180f)
        {
            isLIGHTOutput = false;
            Output = "DARK";
        }
        else
        {
            isLIGHTOutput = true;
            Output = "LIGHT";
        }

        Result.text = isLIGHTOutput ? "LIGHT" : "DARK";

    }

    public void SetDesiredOutput()
    {
        if (!isSpinning)
        {
            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 wheelCenter = wheel.transform.position;

            // Set the desired output based on the angle
            if (mousePosition.x > 0)
            {
                if (desiredOutput == "DARK")
                {
                    desiredOutput = "";
                    randomOutput = true;
                }
                else
                {
                    desiredOutput = "DARK";
                    randomOutput = false;
                }
            }
            else
            {
                if (desiredOutput == "LIGHT")
                {
                    desiredOutput = "";
                    randomOutput = true;
                }
                else
                {
                    desiredOutput = "LIGHT";
                    randomOutput = false;
                }
            }

            Debug.Log(desiredOutput);
        }
    }

    private void Start()
    {
        // Store the original colors of the text.
        originalTextColor1 = title.color;
        originalTextColor2 = Result.color;

        initialScale = share.transform.localScale;

    }

    public void StartTextAnimation()
    {
        // Stop any previous animation coroutines if running.
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        // Start the animation coroutine.
        animationCoroutine = StartCoroutine(TextAnimationCoroutine(isLIGHTOutput));
    }

    private IEnumerator TextAnimationCoroutine(bool isLIGHT)
    {
        if (Output == "LIGHT" || Output == "DARK")
        {
            // Enlarge the text and change its color for 2 seconds.
            float elapsedTime = 0f;
            float animationDuration = 0.5f;
            int enlargedFontSize = 230;
            Color targetTextColor = isLIGHT ? new Color(74f / 255f, 74f / 255f, 74f / 255f) : Color.white;
            //Color BacktargetTextColor = isLIGHT ? new Color(247f / 255f, 247f / 255f, 247f / 255f) : new Color(28f / 255f, 28f / 255f, 28f / 255f);

            title.color = targetTextColor;
            Result.color = targetTextColor;
            //panelBackground.color = BacktargetTextColor;
            panelBackground.sprite = isLIGHT ? BackLight : BackDark;

            share.SetActive(true);
            share.GetComponent<Image>().sprite = isLIGHT ? ShareLight : ShareDark;
            audioSource.Play();

            while (elapsedTime < animationDuration)
            {
                float progress = elapsedTime / animationDuration;

                if (elapsedTime > animationDuration / 2.5f)
                {
                }

                if (elapsedTime < animationDuration / 2f)
                {
                    // Calculate the current scale based on the Lerp function
                    float t = Mathf.Clamp01(progress);
                    Vector3 currentScale = Vector3.Lerp(initialScale, targetScale, t);

                    // Apply the new scale to the GameObject
                    share.transform.localScale = currentScale;
                    // Enlarge the font size and change the text color smoothly.
                    Result.fontSize = (int)Mathf.Lerp(80f, enlargedFontSize, progress);
                }
                else
                {
                    // Enlarge the font size and change the text color smoothly.
                    Result.fontSize = (int)Mathf.Lerp(enlargedFontSize, 80f, progress);
                    // Calculate the current scale based on the Lerp function
                    float t = Mathf.Clamp01(progress);
                    Vector3 currentScale = Vector3.Lerp(targetScale, initialScale, t);

                    // Apply the new scale to the GameObject
                    share.transform.localScale = currentScale;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            animationCoroutine = null;
        }
    }
}

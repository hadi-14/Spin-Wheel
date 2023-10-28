using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public Sprite mutedImage; // Set this in the Inspector to the muted sprite image.
    public Sprite unmutedImage; // Set this in the Inspector to the unmuted sprite image.

    private Image buttonImage;
    private bool isMuted = false;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    private void Start()
    {
        // Set the initial image to unmuted.
        buttonImage.sprite = unmutedImage;
    }

    public void OnMouseDown()
    {
        // Toggle mute state.
        isMuted = !isMuted;

        // Change the image based on the mute state.
        buttonImage.sprite = isMuted ? mutedImage : unmutedImage;

        // Perform any additional actions on mute/unmute if needed.
        if (isMuted)
        {
            // Mute audio, for example:
            AudioListener.volume = 0f;
        }
        else
        {
            // Unmute audio, for example:
            AudioListener.volume = 1f;
        }
    }
}

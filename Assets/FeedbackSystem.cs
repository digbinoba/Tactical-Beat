using System.Collections;
using UnityEngine;
using TMPro;  // Make sure to include this for TextMeshPro

public class FeedbackSystem : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;  // Reference to the TextMeshProUGUI element that will display the feedback
    public float perfectWindow = 0.1f;  // Time window for "Perfect" hit
    public float goodWindow = 0.2f;     // Time window for "Good" hit
    public float okWindow = 0.3f;       // Time window for "Ok" hit
    public float badWindow = 0.4f;      // Time window for "Bad" hit

    public void ShowFeedback(float hitTime, float beatTime)
    {
        float timeDifference = Mathf.Abs(hitTime - beatTime);  // How far the hit time is from the beat time

        if (timeDifference <= perfectWindow)
        {
            feedbackText.text = "Perfect!";
        }
        else if (timeDifference <= goodWindow)
        {
            feedbackText.text = "Good";
        }
        else if (timeDifference <= okWindow)
        {
            feedbackText.text = "Ok";
        }
        else if (timeDifference <= badWindow)
        {
            feedbackText.text = "Bad";
        }
        else
        {
            feedbackText.text = "Missed!";
        }

        // Optionally, you can add a timer to clear the feedback text after a short time.
        StartCoroutine(ClearFeedbackText());
    }

    private IEnumerator ClearFeedbackText()
    {
        yield return new WaitForSeconds(0.5f);  // Clear after 0.5 seconds
        feedbackText.text = "";
    }
}
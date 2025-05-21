using UnityEngine;
using TMPro;

public class ControllerFeedback : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public float feedbackDuration = 1f;

    private float feedbackTimer = 0f;

    void Update()
    {
        if (feedbackTimer > 0)
        {
            feedbackTimer -= Time.deltaTime;
            if (feedbackTimer <= 0)
            {
                feedbackText.text = "";
                Debug.Log("[ControllerFeedback] Feedback cleared.");
            }
        }
    }

    public void ShowFeedback(string feedbackMessage)
    {
        feedbackText.text = feedbackMessage;
        feedbackTimer = feedbackDuration;

        Debug.Log($"[ControllerFeedback] Displaying feedback: {feedbackMessage}");
    }
}
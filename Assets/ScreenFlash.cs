using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;  // Reference to the UI Image
    public float flashDuration = 0.5f;  // How long the flash lasts
    public Color flashColor = Color.red; // The color of the flash

    private void Start()
    {
        if (flashImage == null)
        {
            // Find the Image if not assigned in the Inspector
            flashImage = GetComponent<Image>();
        }

        // Ensure the image is fully transparent at the start
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }

    // Call this method to trigger the red screen flash effect
    public void FlashScreen()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        // Fade in the red color
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / flashDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Keep the screen red for a brief moment
        yield return new WaitForSeconds(0.1f); // Flash time can be adjusted

        // Fade out the red color
        elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / flashDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
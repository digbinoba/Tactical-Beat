using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Player Score and Combo
    private int score = 0;
    private int combo = 0;
    private int maxCombo = 0;

    // References to the UI Text elements
    public TMP_Text scoreText;
    public TMP_Text comboText;

    private void Awake()
    {
        // Ensure only one instance of ScoreManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
        UpdateComboUI();
    }

    // Call this when the player hits an enemy
    public void IncreaseScore(int amount)
    {
        score += amount;
        combo++;  // Increment combo on a successful hit

        if (combo > maxCombo)
        {
            maxCombo = combo;  // Update max combo if needed
        }

        UpdateScoreUI();
        UpdateComboUI();
    }

    // Call this when the player misses an enemy or resets combo
    public void ResetCombo()
    {
        combo = 0;
        UpdateComboUI();
    }

    // Update the score UI
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Update the combo UI
    private void UpdateComboUI()
    {
        if (comboText != null)
        {
            comboText.text = "Combo: " + combo.ToString();
        }
    }

    // You can expose a public method to get the score, if needed
    public int GetScore()
    {
        return score;
    }

    // You can expose a public method to get the combo, if needed
    public int GetCombo()
    {
        return combo;
    }

    // You can expose a public method to get the max combo, if needed
    public int GetMaxCombo()
    {
        return maxCombo;
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Shield")]
    public float maxShield = 50f;
    private float currentShield;

    [Header("Shield Regen")]
    public float shieldRegenRate = 10f;      // Units per second
    public float shieldRegenDelay = 3f;      // Delay after damage before regen starts
    private float shieldRegenTimer = 0f;

    [Header("Score & Combo")]
    public int score = 0;
    public int combo = 0;
    public int maxCombo = 0;

    [Header("UI References")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    public Slider shieldSlider;
    public TextMeshProUGUI shieldText;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        UpdateUI();
    }

    private void Update()
    {
        HandleShieldRegen();
    }

    public void TakeDamage(float damage)
    {
        shieldRegenTimer = shieldRegenDelay; // Reset shield regen delay

        if (currentShield > 0)
        {
            float shieldDamage = Mathf.Min(currentShield, damage);
            currentShield -= shieldDamage;
            damage -= shieldDamage;
        }

        if (damage > 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                HandlePlayerDeath();
            }
        }

        ResetCombo();
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        combo++;
        maxCombo = Mathf.Max(maxCombo, combo);
        UpdateUI();
    }

    public void ResetCombo()
    {
        combo = 0;
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("[PlayerStateManager] Player has died!");
        // TODO: Trigger game over sequence
    }

    // private void HandleShieldRegen()
    // {
    //     if (shieldRegenTimer > 0f)
    //     {
    //         shieldRegenTimer -= Time.deltaTime;
    //         return;
    //     }
    //
    //     if (BlockZone.Instance != null && BlockZone.Instance.AreBothHandsBlocking)
    //     {
    //         return; // Don’t regen while guarding
    //     }
    //
    //     if (currentShield < maxShield)
    //     {
    //         currentShield += shieldRegenRate * Time.deltaTime;
    //         currentShield = Mathf.Min(currentShield, maxShield);
    //         UpdateUI();
    //     }
    // }
    private void HandleShieldRegen()
    {
        // Check if shield regen is delayed
        if (shieldRegenTimer > 0f)
        {
            shieldRegenTimer -= Time.deltaTime;
            return;
        }

        // Don't regenerate if blocking
        if (BlockZone.Instance != null && BlockZone.Instance.AreBothHandsBlocking)
        {
            return; // Shield does not regenerate while blocking
        }

        // Regenerate shield when not blocking
        if (currentShield < maxShield)
        {
            currentShield += shieldRegenRate * Time.deltaTime;
            currentShield = Mathf.Min(currentShield, maxShield); // Ensure it doesn't exceed max
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        if (healthSlider) healthSlider.value = currentHealth / maxHealth;
        if (healthText) healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";

        if (shieldSlider) shieldSlider.value = currentShield / maxShield;
        if (shieldText) shieldText.text = $"{Mathf.CeilToInt(currentShield)} / {Mathf.CeilToInt(maxShield)}";

        if (scoreText) scoreText.text = $"Score: {score}";
        if (comboText) comboText.text = $"Combo: {combo}";
    }
}

using UnityEngine;

public class TargetZone : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    public static TargetZone Instance;

    public AudioSource playerAudioSource; // Reference to the player's AudioSource
    public AudioClip hitSound; // Sound for player being hit
    private void Awake()
    {
        Instance = this;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("[TargetZone] Enemy collided — applying damage");
            
            // Play sound when the enemy hits the target zone
            playerAudioSource.PlayOneShot(hitSound);
            
            // Trigger the screen flash effect when the enemy hits the target zone
            ScreenFlash screenFlash = FindObjectOfType<ScreenFlash>();
            if (screenFlash != null)
            {
                screenFlash.FlashScreen();  // Flash the screen as feedback
            }
            
            if (PlayerStateManager.Instance != null)
            {
                PlayerStateManager.Instance.TakeDamage(damageAmount);
            }

            Destroy(other.gameObject); // Prevent repeat collisions
        }
    }
}
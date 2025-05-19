using UnityEngine;

public class PunchSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip punchClip;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayPunchSound();
        }
    }

    public void PlayPunchSound()
    {
        if (audioSource != null && punchClip != null)
        {
            audioSource.PlayOneShot(punchClip);
            Debug.Log("[PunchSoundPlayer] Played punch sound.");
        }
    }
}
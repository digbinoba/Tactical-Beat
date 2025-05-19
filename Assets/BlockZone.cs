using UnityEngine;

public class BlockZone : MonoBehaviour
{
    public static BlockZone Instance;

    private bool leftInZone = false;
    private bool rightInZone = false;
    private bool wasBlockingLastFrame = false;

    public bool AreBothHandsBlocking => leftInZone && rightInZone;

    [Header("Visual Feedback")]
    public Renderer visualRenderer; // Assign in Inspector
    public Color normalColor = new Color(1, 1, 1, 0.25f);      // Semi-transparent white
    public Color blockingColor = new Color(1f, 0.6f, 0f, 0.4f); // Yellow/Orange

    private void Awake()
    {
        Instance = this;
        if (visualRenderer != null)
        {
            SetColor(normalColor);
        }
    }

    private void Update()
    {
        bool isBlockingNow = AreBothHandsBlocking;

        if (isBlockingNow && !wasBlockingLastFrame)
        {
            Debug.Log("[BlockZone] Player entered blocking state.");
            SetColor(blockingColor);
        }
        else if (!isBlockingNow && wasBlockingLastFrame)
        {
            Debug.Log("[BlockZone] Player exited blocking state.");
            SetColor(normalColor);
        }

        wasBlockingLastFrame = isBlockingNow;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Left"))
        {
            leftInZone = true;
            Debug.Log("[BlockZone] Left hand entered block zone.");
        }
        if (other.name.Contains("Right"))
        {
            rightInZone = true;
            Debug.Log("[BlockZone] Right hand entered block zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Left"))
        {
            leftInZone = false;
            Debug.Log("[BlockZone] Left hand exited block zone.");
        }
        if (other.name.Contains("Right"))
        {
            rightInZone = false;
            Debug.Log("[BlockZone] Right hand exited block zone.");
        }
    }

    private void SetColor(Color color)
    {
        if (visualRenderer != null && visualRenderer.material.HasProperty("_Color"))
        {
            visualRenderer.material.color = color;
        }
    }
}

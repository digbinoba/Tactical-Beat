using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    public EnemyState currentState;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetRandomState()
    {
        currentState = (EnemyState)Random.Range(0, 3);
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (rend == null) return;

        switch (currentState)
        {
            case EnemyState.Neutral:
                rend.material.color = Color.blue;
                break;
            case EnemyState.Attacking:
                rend.material.color = Color.red;
                break;
            case EnemyState.Blocking:
                rend.material.color = new Color(1f, 0.6f, 0f); // orange-yellow
                break;
        }
    }
}
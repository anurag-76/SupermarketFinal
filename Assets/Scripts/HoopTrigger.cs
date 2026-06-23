using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private float scoreCooldown = 0f;
    private const float COOLDOWN_DURATION = 2f;

    private void Update()
    {
        if (scoreCooldown > 0)
            scoreCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && gameManager != null && scoreCooldown <= 0)
        {
            Rigidbody rb = other.attachedRigidbody;

            // Only score if ball is going downward
            if (rb != null && rb.linearVelocity.y < -2f)
            {
                gameManager.AddScore(1);        // Change this number if you want 2 or 3 points
                scoreCooldown = COOLDOWN_DURATION;

                Debug.Log("Basket Scored! +1");
            }
        }
    }
}
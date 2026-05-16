using UnityEngine;

public class BowlingPro : MonoBehaviour
{
    [SerializeField]
    private Rigidbody projectileRigidBody;
    [SerializeField]
    private float projectilePower = 3000;
    [SerializeField]
    private GameObject muzzle;
    [SerializeField]
    private float COOLDOWN_TIME = 0.5f;

    private float coolDown = 0;
    private bool playerInsideTrigger = false; // Is the player in the zone?

    void Update()
    {
        // Only shoot if player is inside the trigger zone
        if (playerInsideTrigger)
        {
            if (coolDown <= 0)
            {
                if (Input.GetButtonUp("Fire1"))
                {
                    coolDown = COOLDOWN_TIME;

                    // Instantiate the projectile
                    Rigidbody aInstance = Instantiate(projectileRigidBody,
                        muzzle.transform.position, transform.rotation) as Rigidbody;

                    // Add force
                    Vector3 forward = transform.TransformDirection(Vector3.forward);
                    aInstance.AddForce(forward * projectilePower);

                    // Destroy after 8 seconds
                    Destroy(aInstance.gameObject, 8);
                }
            }
            else
            {
                coolDown -= Time.deltaTime;
            }
        }
    }

    // Called when something ENTERS the trigger zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            Debug.Log("Player entered the zone — can shoot now!");
        }
    }

    // Called when something EXITS the trigger zone
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            Debug.Log("Player left the zone — shooting disabled!");
        }
    }
}
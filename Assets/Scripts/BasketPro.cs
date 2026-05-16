using UnityEngine;

public class BasketPro : MonoBehaviour
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
    private bool playerInsideTrigger = false;

    private Camera mainCamera; // Reference to the camera

    void Start()
    {
        // Grab the main camera once at the start
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (playerInsideTrigger)
        {
            if (coolDown <= 0)
            {
                if (Input.GetButtonUp("Fire1"))
                {
                    coolDown = COOLDOWN_TIME;

                    // Spawn at muzzle position, but rotate to match CAMERA direction
                    Rigidbody aInstance = Instantiate(projectileRigidBody,
                        muzzle.transform.position, mainCamera.transform.rotation) as Rigidbody;

                    // Use CAMERA's forward so ball follows crosshair direction
                    Vector3 shootDirection = mainCamera.transform.forward;
                    aInstance.AddForce(shootDirection * projectilePower);

                    Destroy(aInstance.gameObject, 8);
                }
            }
            else
            {
                coolDown -= Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            Debug.Log("Player entered zone — can shoot!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            Debug.Log("Player left zone — shooting off!");
        }
    }
}
using UnityEngine;
public class BasketPro : MonoBehaviour
{
    [SerializeField] private Rigidbody projectileRigidBody;
    [SerializeField] private float projectilePower = 3000;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private float COOLDOWN_TIME = 0.5f;
    private float coolDown = 0;
    private Camera mainCamera;
    // Reference to GameManager
    [SerializeField] private GameManager gameManager;
    void Start()
    {
        mainCamera = Camera.main;
        if (gameManager == null)
            Debug.LogWarning("GameManager is not assigned in BasketPro!");
    }
    void Update()
    {
        if (gameManager == null || !gameManager.IsPlayerInArea)
            return;
        if (coolDown <= 0)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                coolDown = COOLDOWN_TIME;
                Rigidbody aInstance = Instantiate(projectileRigidBody,
                muzzle.transform.position, mainCamera.transform.rotation) as Rigidbody;
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
                gameManager.PlayerEnteredArea();
            Debug.Log("Player entered zone");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
                gameManager.PlayerExitedArea();
            Debug.Log("Player left zone");
        }
    }
}
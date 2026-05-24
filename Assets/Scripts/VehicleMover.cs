using UnityEngine;

public class VehicleMover : MonoBehaviour
{
    [SerializeField] private GameObject waypoint1;
    [SerializeField] private GameObject waypoint2;
    [SerializeField] private GameObject waypoint3;
    [SerializeField] private GameObject waypoint4;
    [SerializeField] private GameObject target;
    [SerializeField] private bool flipLookDirection = false;

    [Header("Detection Settings")]
    [SerializeField] private float brakeDistance = 8f;
    [SerializeField] private float clearDistance = 10f;
    [SerializeField] private float vehicleFrontOffset = 2f;
    [SerializeField] private float sphereRadius = 1.5f;

    [Header("Horn Settings")]
    [SerializeField] private AudioClip hornSound;
    [SerializeField, Range(0f, 1f)] private float hornVolume = 1f;     // ← New
    [SerializeField] private float minHonkInterval = 3f;
    [SerializeField] private float maxHonkInterval = 6f;

    private const float CLOSE_DISTANCE = 1;
    private const float SPEED = 10.0f;

    private float currentSpeed = 0f;
    private bool isBraking = false;
    private Vector3 movementDirection;
    private AudioSource audioSource;
    private float honkTimer = 0f;
    private float nextHonkTime = 0f;

    void Start()
    {
        // Get or add AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = hornSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = hornVolume;           // ← Volume applied here
        audioSource.pitch = 1f;

        SetNextHonkTime();
    }

    void Update()
    {
        DetectObstacle();

        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0;
        float distance = direction.magnitude;

        if (distance > 0)
        {
            movementDirection = direction.normalized;
            Quaternion rotation = flipLookDirection
                ? Quaternion.LookRotation(-direction, Vector3.up)
                : Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = rotation;
        }

        if (isBraking)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, Time.deltaTime * 15f);
            HandleHonking();
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, SPEED, Time.deltaTime * 8f);
        }

        if (currentSpeed > 0)
        {
            Vector3 normDirection = direction / distance;
            transform.position += normDirection * currentSpeed * Time.deltaTime;
        }

        if (distance < CLOSE_DISTANCE)
        {
            if (target.Equals(waypoint1)) target = waypoint2;
            else if (target.Equals(waypoint2)) target = waypoint3;
            else if (target.Equals(waypoint3)) target = waypoint4;
            else if (target.Equals(waypoint4)) target = waypoint1;
        }
    }

    void DetectObstacle()
    {
        if (movementDirection == Vector3.zero) return;

        Vector3 frontPosition = transform.position + movementDirection * vehicleFrontOffset;
        RaycastHit hit;

        bool obstacleFound = Physics.SphereCast(
            frontPosition,
            sphereRadius,
            movementDirection,
            out hit,
            isBraking ? clearDistance : brakeDistance,
            ~0,
            QueryTriggerInteraction.Collide
        );

        if (obstacleFound)
        {
            if (hit.collider.CompareTag("Vehicle") ||
                hit.collider.CompareTag("Player") ||
                hit.collider.CompareTag("NPC"))
            {
                if (!isBraking) Honk();
                isBraking = true;
            }
            else
            {
                ResetHonk();
                isBraking = false;
            }
        }
        else
        {
            ResetHonk();
            isBraking = false;
        }
    }

    void HandleHonking()
    {
        honkTimer += Time.deltaTime;
        if (honkTimer >= nextHonkTime)
        {
            Honk();
            SetNextHonkTime();
        }
    }

    void Honk()
    {
        if (hornSound != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.volume = hornVolume;     // Ensure volume is correct before playing
            audioSource.Play();
            Debug.Log("Honking!");
        }
    }

    void ResetHonk()
    {
        honkTimer = 0f;
        SetNextHonkTime();
    }

    void SetNextHonkTime()
    {
        nextHonkTime = Random.Range(minHonkInterval, maxHonkInterval);
        honkTimer = 0f;
    }

    // Optional: Public method to change volume at runtime
    public void SetHornVolume(float newVolume)
    {
        hornVolume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
            audioSource.volume = hornVolume;
    }
}
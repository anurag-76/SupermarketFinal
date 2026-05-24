using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float fixedHeight = 50f;   // fixed altitude above terrain
    public LayerMask groundLayer;

    private Rigidbody rb;
    private float yaw;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Place drone at fixed height above terrain at start
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 500f, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + fixedHeight, transform.position.z);
        }
    }

    void Update()
    {
        // Mouse yaw rotation (left/right)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        yaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // WASD movement (horizontal only)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = (transform.forward * v + transform.right * h) * moveSpeed;

        rb.linearVelocity = new Vector3(move.x, 0f, move.z);

        // Keep altitude fixed above terrain
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 500f, groundLayer))
        {
            rb.position = new Vector3(rb.position.x, hit.point.y + fixedHeight, rb.position.z);
        }
    }
}

using UnityEngine;

public class GateController : MonoBehaviour
{
    public float openAngle = 60f;
    private bool isOpen = false;
    private bool playerNearby = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F pressed while player nearby!");
            ToggleGate();
        }
    }

    void ToggleGate()
    {
        transform.localRotation = isOpen ? closedRotation : openRotation;
        isOpen = !isOpen;
        Debug.Log("Gate toggled. Now open? " + isOpen);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger!");
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger!");
            playerNearby = false;
        }
    }
}

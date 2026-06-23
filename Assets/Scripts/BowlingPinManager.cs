using UnityEngine;

public class BowlingPinManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Pins")]
    public GameObject[] pins = new GameObject[10];

    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;
    private bool strikeDetected = false;

    private void Start()
    {
        SaveInitialPositions();
    }

    private void SaveInitialPositions()
    {
        initialPositions = new Vector3[pins.Length];
        initialRotations = new Quaternion[pins.Length];

        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i] != null)
            {
                initialPositions[i] = pins[i].transform.position;
                initialRotations[i] = pins[i].transform.rotation;
            }
        }
    }

    private void Update()
    {
        if (strikeDetected || gameManager == null) return;

        if (AreAllPinsDown())
        {
            strikeDetected = true;
            gameManager.AddScore(5);     // +5 for all pins knocked
            Debug.Log("🎳 STRIKE! All pins down → +5");
        }
    }

    private bool AreAllPinsDown()
    {
        int knocked = 0;
        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i] == null) continue;

            float tilt = Vector3.Dot(pins[i].transform.up, Vector3.up);
            if (tilt < 0.7f) // Pin is fallen
                knocked++;
        }
        return knocked == pins.Length;
    }

    public void ResetPins()
    {
        strikeDetected = false;
        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i] != null)
            {
                pins[i].transform.position = initialPositions[i];
                pins[i].transform.rotation = initialRotations[i];

                Rigidbody rb = pins[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}
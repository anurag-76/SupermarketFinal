using UnityEngine;

public class ClothesSwingWorld : MonoBehaviour
{
    public float swingSpeed = 2f;   // oscillation speed
    public float swingAngle = 5f;   // max swing angle in degrees
    public Vector3 swingAxis = Vector3.right; // world axis for left-right swing

    private float[] phaseOffsets;

    void Start()
    {
        // Random offsets for natural variation
        phaseOffsets = new float[transform.childCount];
        for (int i = 0; i < phaseOffsets.Length; i++)
        {
            phaseOffsets[i] = Random.Range(0f, Mathf.PI * 2f);
        }
    }

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Base rotation (keep their original placement)
            Quaternion baseRotation = Quaternion.identity;

            // Calculate swing
            float angle = Mathf.Sin(Time.time * swingSpeed + phaseOffsets[i]) * swingAngle;
            Quaternion swing = Quaternion.AngleAxis(angle, swingAxis);

            // Apply swing relative to world axis
            child.rotation = baseRotation * swing;
        }
    }
}

using UnityEngine;

public class FerrisWheelRotation : MonoBehaviour
{
    public float speed = 10f;   // Rotation speed

    void Update()
    {
        transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }
}
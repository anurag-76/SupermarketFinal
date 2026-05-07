using UnityEngine;

/// <summary>
/// CabinController.cs
/// Attach to: Cabin GameObject
///
/// Cancels out the parent wheel's X-axis rotation every frame so the
/// cabin always stays perfectly upright — exactly like a real ferris wheel.
/// </summary>
public class CabinController : MonoBehaviour
{
    void LateUpdate()
    {
        // Lock the cabin to world-upright on X every frame.
        // Y and Z are preserved from the cabin's original placement.
        transform.rotation = Quaternion.Euler(
            0f,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z
        );
    }
}
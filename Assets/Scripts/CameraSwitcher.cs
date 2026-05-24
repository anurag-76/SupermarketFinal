using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera playerCamera;
    public Camera droneCamera;
    public GameObject drone;
    public Text switchText;

    private bool isDroneActive = false;

    void Start()
    {
        playerCamera.enabled = true;
        droneCamera.enabled = false;
        switchText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isDroneActive = !isDroneActive;
            playerCamera.enabled = !isDroneActive;
            droneCamera.enabled = isDroneActive;

            // Hide UI after switching
            if (isDroneActive)
                switchText.gameObject.SetActive(false);
        }
    }
}

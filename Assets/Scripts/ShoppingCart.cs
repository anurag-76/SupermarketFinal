using UnityEngine;
using TMPro;

public class ShoppingCart : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 5f;
    public TextMeshProUGUI promptText;

    private GameObject currentTarget;

    void Update()
    {
        // Hide prompt by default
        if (promptText != null)
            promptText.gameObject.SetActive(false);

        // Debug ray
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance, Color.green);

        // Cast ray
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("Cart"))
            {
                currentTarget = hit.collider.gameObject;

                if (promptText != null)
                {
                    promptText.gameObject.SetActive(true);
                    promptText.text = "Press [Q] to Grab";
                }

                Debug.Log("Looking at cart!");

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("Holding Cart!");
                }
            }
            else
            {
                currentTarget = null;
            }
        }
    }
}

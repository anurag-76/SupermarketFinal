using UnityEngine;

public class ATMInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public Camera playerCamera;

    [Header("UI")]
    public GameObject interactPrompt; // Drag your PromptPanel here

    private ATMUIManager atmUI;

    void Start()
    {
        atmUI = FindObjectOfType<ATMUIManager>();
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (atmUI == null || atmUI.IsOpen) return;

        Ray ray = playerCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                interactPrompt.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactPrompt.SetActive(false);
                    atmUI.OpenATM();
                }
                return;
            }
        }

        interactPrompt.SetActive(false);
    }
}

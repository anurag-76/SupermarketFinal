using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public LayerMask itemLayer;
    public Camera playerCamera;

    [Header("UI")]
    public GameObject interactPrompt;        // "Press E to pick up" UI
    public TextMeshProUGUI interactPromptTxt;

    private Inventory inventory;
    private ItemPickup currentItem;

    private void Start()
    {
        inventory = Object.FindFirstObjectByType<Inventory>();
        interactPrompt.SetActive(false);
    }

    private void Update()
    {
        CheckForItem();

        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    private void CheckForItem()
    {
        Ray ray = playerCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, itemLayer))
        {
            ItemPickup pickup = hit.collider.GetComponent<ItemPickup>();

            if (pickup != null)
            {
                currentItem = pickup;
                interactPrompt.SetActive(true);
                interactPromptTxt.text = "Press [E] to pick up\n" + pickup.item.itemname;
                return;
            }
        }

        // Nothing found
        currentItem = null;
        interactPrompt.SetActive(false);
    }

    private void PickupItem()
    {
        inventory.AddItem(currentItem.item, currentItem.amount);

        // Don't destroy the shelf! Just clear the pickup script
        currentItem.enabled = false;
        currentItem = null;
        interactPrompt.SetActive(false);
    }
}
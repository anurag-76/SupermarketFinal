using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DragHandler : MonoBehaviour
{
    private static DragHandler instance;

    [Header("References")]
    public Canvas canvas;

    private GameObject dragIcon;
    private Slot originSlot;
    private Image dragImage;

    private void Awake()
    {
        instance = this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Slot slot = eventData.pointerPress?.GetComponent<Slot>();
        if (slot == null || !slot.HasItem()) return;

        originSlot = slot;

        // Create drag icon
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        dragImage = dragIcon.AddComponent<Image>();
        dragImage.sprite = slot.GetItem().icon;
        dragImage.raycastTarget = false;

        RectTransform rt = dragIcon.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(60, 60);

        // Add amount text
        GameObject textObj = new GameObject("Amount");
        textObj.transform.SetParent(dragIcon.transform, false);
        TextMeshProUGUI txt = textObj.AddComponent<TextMeshProUGUI>();
        txt.text = slot.GetAmount().ToString();
        txt.fontSize = 14;
        txt.raycastTarget = false;
        RectTransform txtRt = textObj.GetComponent<RectTransform>();
        txtRt.anchorMin = Vector2.zero;
        txtRt.anchorMax = Vector2.zero;
        txtRt.anchoredPosition = new Vector2(8, 4);
        txtRt.sizeDelta = new Vector2(40, 20);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        dragIcon.GetComponent<RectTransform>().localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon == null) return;

        Destroy(dragIcon);
        dragIcon = null;

        if (originSlot == null) return;

        // Check if dropped on a slot
        Slot targetSlot = eventData.pointerCurrentRaycast.gameObject?.GetComponent<Slot>();

        if (targetSlot != null)
        {
            SwapSlots(originSlot, targetSlot);
        }
        else
        {
            // Dropped outside — drop item in world
            DropItem(originSlot);
        }

        originSlot = null;
    }

    private void SwapSlots(Slot from, Slot to)
    {
        ItemSO fromItem = from.GetItem();
        int fromAmount = from.GetAmount();
        ItemSO toItem = to.GetItem();
        int toAmount = to.GetAmount();

        // If same item type, try to stack
        if (toItem != null && toItem == fromItem)
        {
            int space = toItem.maxStackSize - toAmount;
            if (space > 0)
            {
                int transfer = Mathf.Min(space, fromAmount);
                to.SetItem(toItem, toAmount + transfer);
                if (fromAmount - transfer <= 0)
                    from.ClearSlot();
                else
                    from.SetItem(fromItem, fromAmount - transfer);
                return;
            }
        }

        // Otherwise swap
        if (toItem != null)
            from.SetItem(toItem, toAmount);
        else
            from.ClearSlot();

        to.SetItem(fromItem, fromAmount);
    }

    private void DropItem(Slot slot)
    {
        if (slot.GetItem().itemPrefab != null)
        {
            // Find player to drop near them
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 dropPos = player != null
                ? player.transform.position + player.transform.forward * 2f
                : Vector3.zero;

            Instantiate(slot.GetItem().itemPrefab, dropPos, Quaternion.identity);
        }

        slot.ClearSlot();
    }
}
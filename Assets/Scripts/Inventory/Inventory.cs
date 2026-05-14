using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public ItemSO woodItem;
    public ItemSO axeItem;

    public GameObject hotbarObj;
    public GameObject inventorySlotParent;
    public GameObject inventoryUI;

    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>(true));
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<Slot>(true));

        allSlots.AddRange(hotbarSlots);
        allSlots.AddRange(inventorySlots);        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = !inventoryUI.activeSelf;
            inventoryUI.SetActive(isActive);

            // To Enable/disable mouse cursor on inventory opening
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isActive;
        }
    }

    public void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;

        foreach (Slot slot in allSlots)
        {
            if(slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if(currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= amountToAdd;

                    if(remaining <= 0)
                        return;                    
                }
            }
        }


        foreach (Slot slot in allSlots)
        {
            if(!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;

                if (remaining <= 0)
                    return;
            }
        }

        if(remaining > 0)
        {
            Debug.Log("Inventory Is full, could not add " + remaining + " of " + itemToAdd.itemname);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour
{
    public Item assignedItem;
    public TextMeshProUGUI itemNameText;
    public Image itemImage;
    public bool inInventory;
    public GameObject tooltip;
    public GameObject border;
    public int slotSize;
    public bool selected = false;
    public InventoryManager inventoryManager;

    void Start()
    {
        slotSize = (int)gameObject.GetComponent<RectTransform>().rect.width;
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void CreateItemSlot(Item item)
    {
        assignedItem = item;
        itemNameText.text = item.itemName;
        itemImage.sprite = item.icon;

        if (assignedItem != null)
        {
            itemNameText.gameObject.SetActive(true);
            itemImage.gameObject.SetActive(true);

        }
        else
        {
            itemNameText.gameObject.SetActive(false);
            itemImage.gameObject.SetActive(false);
        }
    }

    public void ShowTooltip()
    {
        SetTooltipPosition();
        if (assignedItem != null)
        {
            tooltip.SetActive(true);
            tooltip.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = assignedItem.itemName;
            tooltip.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = assignedItem.description;
        }
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void SetTooltipPosition()
    {
        if (gameObject.GetComponent<RectTransform>().localPosition.x < slotSize / 2)
        {
            tooltip.GetComponent<RectTransform>().localPosition = new Vector2(slotSize/2, -slotSize);
        }
        else if (gameObject.GetComponent<RectTransform>().localPosition.x + slotSize > Screen.width / 2)
        {
            tooltip.GetComponent<RectTransform>().localPosition = new Vector2(-slotSize / 2, -slotSize);
        }
        else
        {
            tooltip.GetComponent<RectTransform>().localPosition = new Vector2(0, -slotSize);
        }
    }

    public void SelectItem()
    {
        if (assignedItem != null)
        {
            foreach (GameObject slot in inventoryManager.inventorySlots)
            {
                slot.GetComponent<InventoryItemSlot>().selected = false;
                slot.GetComponent<InventoryItemSlot>().border.SetActive(false);
            }
            selected = true;
            border.SetActive(true);
        }
    }

    public void HandleItemUse()
    {
        if (!selected)
        {
            SelectItem();
        }
    }


}

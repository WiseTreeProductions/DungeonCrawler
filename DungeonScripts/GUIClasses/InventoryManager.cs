using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject itemSlotPrefab;
    public GameObject bagPanel;
    public GameObject bagGrid;
    public GameObject equipmentPanel;
    public GameObject goldValue;
    public List<GameObject> inventorySlots;
    public DungeonMaster dungeonMaster;

    public GameObject charNameText;
    public GameObject rightHandSlot;
    public GameObject leftHandSlot;
    public GameObject torsloSlot;
    public GameObject legSlot;
    public GameObject feetSlot;
    public GameObject armSlot;
    public GameObject backSlot;
    public GameObject accessory1Slot;
    public GameObject accessory2Slot;
    public GameObject accessory3Slot;
    public GameObject accessory4Slot;
    public GameObject accessory5Slot;

    public List<GameObject> equipmentSlots;

    public GameObject selectedItemSlot;
    // Start is called before the first frame update
    void Start()
    {
        equipmentSlots.Add(rightHandSlot);
        equipmentSlots.Add(leftHandSlot);
        equipmentSlots.Add(torsloSlot);
        equipmentSlots.Add(legSlot);
        equipmentSlots.Add(feetSlot);
        equipmentSlots.Add(armSlot);
        equipmentSlots.Add(backSlot);
        equipmentSlots.Add(accessory1Slot);
        equipmentSlots.Add(accessory2Slot);
        equipmentSlots.Add(accessory3Slot);
        equipmentSlots.Add(accessory4Slot);
        equipmentSlots.Add(accessory5Slot);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectableItem = GetSelectableItem();
            if (selectableItem)
            {
                if (selectedItemSlot == null)
                {
                    SelectItem(selectableItem);
                }
                else
                {
                    if (selectedItemSlot.Equals(GetSelectableItem()))
                    {
                        InteractWithSelectedItem();
                    }
                    else
                    {
                        DeselectItem();
                        SelectItem(selectableItem);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DeselectItem();
        }
    }

    public void ToggleInventory()
    {
        if (!inventoryPanel.activeInHierarchy)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        goldValue.GetComponent<TextMeshProUGUI>().text = dungeonMaster.playerParty.partyGold.ToString();
        PopulateBag();
        PopulateCharacterInventory();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        ClearInventory();
    }

    public void PopulateBag()
    {
        foreach (Item item in dungeonMaster.playerParty.partyInventory)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, bagGrid.transform);
            itemSlot.GetComponent<InventoryItemSlot>().CreateItemSlot(item);
            inventorySlots.Add(itemSlot);
        }
    }

    public void PopulateCharacterInventory()
    {
        Debug.Log(dungeonMaster.playerParty.selectedUnit.unitName);
        charNameText.GetComponent<TextMeshProUGUI>().text = dungeonMaster.playerParty.selectedUnit.unitName;
    }

    public void ShowNextInventory()
    {
        dungeonMaster.playerParty.CycleUnitForward();
        PopulateCharacterInventory();
    }

    public void ShowPreviousInventory()
    {
        dungeonMaster.playerParty.CycleUnitBack();
        PopulateCharacterInventory();
    }

    public void ClearInventory()
    {
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot);
        }

        inventorySlots.Clear();
    }

    public GameObject GetSelectableItem()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Get the pointer event data
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = Input.mousePosition;

            // Create a list to store raycast results
            List<RaycastResult> raycastResults = new List<RaycastResult>();

            // Raycast to find all UI elements under the mouse pointer
            EventSystem.current.RaycastAll(eventDataCurrentPosition, raycastResults);

            if (raycastResults.Count > 0)
            {
                // Aaccess the first UI element in the list (the topmost one)
                GameObject uiObjectUnderMouse = raycastResults[0].gameObject;
                if (uiObjectUnderMouse.GetComponent<InventoryItemSlot>())
                {
                    if (uiObjectUnderMouse.GetComponent<InventoryItemSlot>().assignedItem != null)
                    {
                        return uiObjectUnderMouse;
                    }
                } 
                else if (uiObjectUnderMouse.transform.parent.GetComponent<InventoryItemSlot>())
                {
                    if (uiObjectUnderMouse.transform.parent.GetComponent<InventoryItemSlot>().assignedItem != null)
                    {
                        return uiObjectUnderMouse.transform.parent.gameObject;
                    }
                }
            }
        }
        return null;
    }

    public void SelectItem(GameObject itemSlot)
    {
        itemSlot.GetComponent<InventoryItemSlot>().selected = true;
        itemSlot.GetComponent<InventoryItemSlot>().border.SetActive(true);
        selectedItemSlot = itemSlot;
    }

    public void DeselectItem()
    {
        selectedItemSlot = null;
        foreach (GameObject equipSlot in equipmentSlots)
        {
            equipSlot.GetComponent<InventoryItemSlot>().selected = false;
            equipSlot.GetComponent<InventoryItemSlot>().border.SetActive(false);
        }

        foreach (GameObject inventorySlot in inventorySlots)
        {
            inventorySlot.GetComponent<InventoryItemSlot>().selected = false;
            inventorySlot.GetComponent<InventoryItemSlot>().border.SetActive(false);
        }
    }

    public void InteractWithSelectedItem()
    {
        Item selectedItem = selectedItemSlot.GetComponent<InventoryItemSlot>().assignedItem;
        if (selectedItem is Equipment)
        {
            Equipment equipment = (Equipment)selectedItem;
            dungeonMaster.playerParty.selectedUnit.EquipItem(equipment);
        }
        else
        {
            Debug.Log("NOT IMPLEMENTED YET");
        }
    }
}

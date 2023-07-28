using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject characterPanel;
    public GameObject inventoryPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCharacterPanel()
    {
        if (!characterPanel.activeInHierarchy)
        {
            inventoryPanel.GetComponent<InventoryManager>().CloseInventory();
            characterPanel.GetComponent<CharacterScreenManager>().OpenCharScreen();
        }
        else
        {
            characterPanel.GetComponent<CharacterScreenManager>().CloseCharScreen();
        }
    }

    public void ToggleInventoryPanel()
    {
        if (!inventoryPanel.activeInHierarchy)
        {
            characterPanel.GetComponent<CharacterScreenManager>().CloseCharScreen();
            inventoryPanel.GetComponent<InventoryManager>().OpenInventory();
        }
        else
        {
            inventoryPanel.GetComponent<InventoryManager>().CloseInventory();
        }

    }
}

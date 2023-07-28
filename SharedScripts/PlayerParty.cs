using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public List<PlayerUnitData> playerCharacters;
    public List<Item> partyInventory = new List<Item>();
    public int partyGold;
    public PlayerUnitData selectedUnit;
    public int selectedUnitId;
    // Start is called before the first frame update
    void Start()
    {
        PlayerUnitData playerUnit1 = new PlayerUnitData();
        playerUnit1.CreateCharacter("ZULUL", Resources.Load<Sprite>("Portraits/zulul"), Resources.Load<Sprite>("Portraits/zulul"), 7, 5, 5, 4, 8, 2, 3);
        playerCharacters.Add(playerUnit1);

        PlayerUnitData playerUnit2 = new PlayerUnitData();
        playerUnit2.CreateCharacter("Okayeg", Resources.Load<Sprite>("Portraits/okayeg"), Resources.Load<Sprite>("Portraits/okayeg"), 5, 7, 7, 5, 5, 4, 4);
        playerCharacters.Add(playerUnit2);

        PlayerUnitData playerUnit3 = new PlayerUnitData();
        playerUnit3.CreateCharacter("MaN", Resources.Load<Sprite>("Portraits/MaN"), Resources.Load<Sprite>("Portraits/MaN"), 10, 3, 3, 3, 10, 1, 1);
        playerCharacters.Add(playerUnit3);

        PlayerUnitData playerUnit4 = new PlayerUnitData();
        playerUnit4.CreateCharacter("Jokerge", Resources.Load<Sprite>("Portraits/jokerge"), Resources.Load<Sprite>("Portraits/jokerge"), 5, 8, 7, 8, 5, 5, 5);
        playerCharacters.Add(playerUnit4);

        PlayerUnitData playerUnit5 = new PlayerUnitData();
        playerUnit5.CreateCharacter("MMMM", Resources.Load<Sprite>("Portraits/MMMM"), Resources.Load<Sprite>("Portraits/MMMM"), 2, 5, 5, 4, 4, 9, 6);
        playerCharacters.Add(playerUnit5);

        PlayerUnitData playerUnit6 = new PlayerUnitData();
        playerUnit6.CreateCharacter("WiseTree", Resources.Load<Sprite>("Portraits/WiseTree"), Resources.Load<Sprite>("Portraits/WiseTree"), 5, 4, 4, 7, 7, 5, 9);
        playerCharacters.Add(playerUnit6);

        selectedUnit = playerCharacters[0];
        selectedUnitId = 0;

        foreach (UnitData unit in playerCharacters)
        {
            unit.PrintAllInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LootItem(Item item)
    {
        if (!item.stackable)
        {
            partyInventory.Add(item);
        }
        else
        {
            foreach (Item i in partyInventory)
            {
                if (item.itemName.Equals(i.itemName))
                {
                    i.stackSize += item.stackSize;
                }
                else
                {
                    partyInventory.Add(item);
                }
            }
        }
    }

    public void LootGold(int goldCount)
    {
        partyGold += goldCount;
    }

    public void CycleUnitForward()
    {
        selectedUnitId++;
        if (selectedUnitId == playerCharacters.Count)
        {
            selectedUnitId = 0;
        }
        selectedUnit = playerCharacters[selectedUnitId];
    }

    public void CycleUnitBack()
    {
        selectedUnitId--;
        if (selectedUnitId < 0)
        {
            selectedUnitId = playerCharacters.Count - 1;
        }
        selectedUnit = playerCharacters[selectedUnitId];
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
    public enum GameState
    {
        IDLE,
        WAITING,
        ENEMY_MOVEMENT
    }

    public GameState gameState;

    public List<Animator> sceneAnimators;
    public List<Interactable> sceneInteractables;
    public GUIManager guiManager;
    public DialogueHandler dialogueHandler;
    public GameObject player;
    public List<Lootable> lootables;
    public PlayerParty playerParty;
    public ItemRepository itemRepository;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(guiManager.ShowAreaNotifier("Dungeon: Floor 1"));
        gameState = GameState.IDLE;

        sceneAnimators = FindObjectsOfType<Animator>().ToList();
        sceneInteractables = FindObjectsOfType<Interactable>().ToList();
        FillChest(lootables[0], new List<Item> { new D1DoorKey(), itemRepository.rustySword });
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAnyInteractionHappening() || guiManager.dialoguePanel.activeInHierarchy == true)
        {
            gameState = GameState.WAITING;
        }
        else
        {
            gameState = GameState.IDLE;
        }
    }

    public bool IsAnyInteractionHappening()
    {
        foreach (Interactable interactable in sceneInteractables)
        {
            if (interactable.interactionInProgress)
            {
                return true;
            }
        }

        return false;
    }

    public void FillChest(Lootable chest, List<Item> items)
    {
        chest.items = items;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.CodeDom.Compiler;
using System.Reflection;
using UnityEditor;

public class DialogueHandler : MonoBehaviour
{
    public GameObject partyHolder;
    public GameObject controlButtonHolder;
    public GameObject dialoguePanel;
    public GameObject nextButton;
    public Dialogue currentDialogue;
    public GameObject speakerNamePanel;
    public GameObject speakerSpriteObject;
    public GameObject responsePanel;
    public GameObject responseButtonPrefab;
    public ScrollRect scrollRect;
    public DialogueActionManager actionManager;
    private int finalPageId;
    private float buttonPositionOrigin = 40f;
    private float buttonPositionSpacing = 30f;
    public int maxButtons = 4;
    // Start is called before the first frame update
    void Start()
    {
        finalPageId = currentDialogue.finalPageIndex;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {
        partyHolder.SetActive(false);
        controlButtonHolder.SetActive(false);
        dialoguePanel.SetActive(true);
        currentDialogue = dialogue;
        currentDialogue.currentPage = currentDialogue.GetPageWithIndex(0);
        DisplayPage(currentDialogue.currentPage);
    }

    public void EndDialogue()
    {
        partyHolder.SetActive(true);
        controlButtonHolder.SetActive(true);
        dialoguePanel.SetActive(false);
        currentDialogue = null;
    }


    public void ShowNextPage()
    {
        if (currentDialogue.currentPage.nextPageIndex == finalPageId)
        {
            EndDialogue();
        }
        else
        {
            currentDialogue.SetNextPage();
            DisplayPage(currentDialogue.GetPageWithIndex(currentDialogue.currentPage.index));
        }
    }

    public void NavigateToSpecificPage(int pageID)
    {
        currentDialogue.SetPageByID(pageID);
        DisplayPage(currentDialogue.GetPageWithIndex(pageID));
    }

    public void PopulateDialogue(List<string> messages)
    {
        //dialogueMessages = messages;
        //ShowDialogue();
        nextButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Next";
    }

    public void DisplayPage(Page page)
    {
        //Showing appropriate next button
        if (currentDialogue.currentPage.responses.Count > 0)
        {
            nextButton.gameObject.SetActive(false);
            responsePanel.gameObject.SetActive(true);
            PopulateResponses(currentDialogue.currentPage);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            responsePanel.gameObject.SetActive(false);
            if (currentDialogue.currentPage.nextPageIndex == finalPageId)
            {
                nextButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Close";
            }
            else
            {
                nextButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Next";
            }
        }

        //Populating dialogue panel contents
        dialoguePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = page.text;
        if (!string.IsNullOrWhiteSpace(page.associatedSpeakerName))
        {
            speakerNamePanel.SetActive(true);
            speakerNamePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = page.associatedSpeakerName;
        }
        else
        {
            speakerNamePanel.SetActive(false);
        }

        if (page.associatedSpeakerImage != null)
        {
            speakerSpriteObject.SetActive(true);
            speakerSpriteObject.GetComponent<Image>().sprite = page.associatedSpeakerImage;
        }
        else
        {
            speakerSpriteObject.SetActive(false);
        }
    }

    public void PopulateResponses(Page page)
    {
        for (int i = 0; i < page.responses.Count; i++)
        {
            page.responses[i].methodToExecute = actionManager.GetMethodToExecute(page.responses[i].type);
            GameObject responseButton = Instantiate(responseButtonPrefab, responsePanel.transform);
            Vector2 newPosition = responseButton.GetComponent<RectTransform>().anchoredPosition;
            newPosition.y = buttonPositionOrigin - buttonPositionSpacing * i;
            responseButton.GetComponent<RectTransform>().anchoredPosition = newPosition;
            responseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = page.responses[i].text;
            int currentIndex = i;
            responseButton.GetComponent<Button>().onClick.AddListener(() => page.responses[currentIndex].ExecuteMethod());
        }
    }
}

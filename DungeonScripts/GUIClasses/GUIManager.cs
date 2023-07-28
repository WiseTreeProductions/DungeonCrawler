using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public PlayerControls playerControls;
    public GameObject interactButton;
    public GameObject dialoguePanel;
    public GameObject partyHolder;
    public GameObject controlButtonHolder;
    public GameObject dialogueButton;
    public GameObject fpsCounter;
    public Image transitionImage;
    private float deltaTime = 0.0f;
    public bool floorTransitionPlaying = false;
    public bool areaNameShowing = false;
    public TextMeshProUGUI areaTextObject;
    public float areaNameFadeDuration = 1.5f;
    public Coroutine areaNameTransitionCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(PlayFloorTransition());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.canInteract && playerControls.dungeonMaster.gameState == DungeonMaster.GameState.IDLE)
        {
            interactButton.SetActive(true);
        }
        else
        {
            interactButton.SetActive(false);
        }
        UpdateFPS();
    }

    public void UpdateFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        int fps = Mathf.RoundToInt(1.0f / deltaTime);
        fpsCounter.GetComponent<TextMeshProUGUI>().text = fps.ToString();
    }

    public void UpdateInteractText(string text)
    {
        interactButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
    }

    public IEnumerator PlayFloorTransition()
    {
        floorTransitionPlaying = true;
        // Fade in
        yield return FadeAlpha(1f);

        // Wait for a second before fading out
        yield return new WaitForSeconds(1f);

        // Fade out
        yield return FadeAlpha(0f);
        floorTransitionPlaying = false;
    }

    public IEnumerator FadeAlpha(float targetAlpha)
    {
        float fadeDuration = 1f;
        Color startColor = transitionImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            transitionImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        transitionImage.color = targetColor;
    }

    public IEnumerator ShowAreaNotifier(string areaName)
    {
        areaNameShowing = true;
        areaTextObject.text = areaName;
        // Fade in
        areaTextObject.color = new Color(areaTextObject.color.r, areaTextObject.color.g, areaTextObject.color.b, 0);
        float t = 0;
        while (t < areaNameFadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, t / areaNameFadeDuration);
            areaTextObject.color = new Color(areaTextObject.color.r, areaTextObject.color.g, areaTextObject.color.b, alpha);
            yield return null;
        }

        // Wait for a short period of time before fading out
        yield return new WaitForSeconds(1f);

        // Fade out
        t = 0;
        while (t < areaNameFadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, t / areaNameFadeDuration);
            areaTextObject.color = new Color(areaTextObject.color.r, areaTextObject.color.g, areaTextObject.color.b, alpha);
            yield return null;
        }

        // Set the text object to be invisible
        areaTextObject.color = new Color(areaTextObject.color.r, areaTextObject.color.g, areaTextObject.color.b, 0);
        areaNameShowing = false;
    }

    public void PlayAreaNameCoroutine(string areaName)
    {
        if (areaNameTransitionCoroutine == null && !areaNameShowing)
        {
            areaNameTransitionCoroutine = StartCoroutine(ShowAreaNotifier(areaName));
        }
        else
        {
            StopCoroutine(areaNameTransitionCoroutine);
            areaNameTransitionCoroutine = StartCoroutine(ShowAreaNotifier(areaName));
        }
    }




}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public Animator animator;


    public static TooltipManager Instance;
    bool selected;
    float selectedTime;
    GameObject lastSelected;

    [SerializeField] GameObject Header;

    [SerializeField] TMP_Text CardTitle;
    [SerializeField] TMP_Text CardDescripton;

    [SerializeField] GameObject Painless;
    [SerializeField] GameObject Forgetful;
    [SerializeField] GameObject Cathartic;
    [SerializeField] GameObject Power;
    [SerializeField] GameObject Resilience;
    [SerializeField] GameObject Story;


    private void Awake()
    {
        TooltipManager.Instance = this;
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (lastSelected == null && selected)
        {
            selected = false;
            if (Header.activeSelf)
            {
                CardUnHovered();
            }
            else
            {
                HandCardUnhovered();
            }
        }

        if (selected)
        {
            selectedTime += Time.deltaTime;
            if (selectedTime > 0.1f)
            {
                animator.SetBool("Toggled", true);
            }
            else
            {
                animator.SetBool("Toggled", false);
            }
        }
        else
        {
            animator.SetBool("Toggled", false);
        }
    }


    public void CardHovered(Card currentCard, GameObject card)
    {
        selected = true;
        CardTitle.text = currentCard.CardName;
        CardDescripton.text = currentCard.CardDescription;

        ShowKeywords(currentCard.Keywords);

        Header.SetActive(true);
        lastSelected = card;
    }

    internal void CardHovered(StoryData currentStory, GameObject gameObject)
    {
        Header.SetActive(true);
        selected = true;
        CardTitle.text = currentStory.StoryTitle;
        CardDescripton.text = currentStory.StoryDescription;

        Story.SetActive(true);

        lastSelected = gameObject;
    }

    public void HandCardHovered(List<eCardKeyword> keywords, GameObject gameObject)
    {
        Header.SetActive(false);
        ShowKeywords(keywords);
        selected = true;
        lastSelected = gameObject;
    }

    public void CardUnHovered()
    {
        selected = false;
        selectedTime = 0;
        lastSelected = null;
    }

    public void HandCardUnhovered()
    {
        selected = false;
        selectedTime = 0;
        lastSelected = null;
        ClearKeywords();
    }

    internal void ClearKeywords()
    {
        Painless.SetActive(false);
        Forgetful.SetActive(false);
        Cathartic.SetActive(false);
        Power.SetActive(false);
        Resilience.SetActive(false);
        Story.SetActive(false);
    }

    internal void ShowKeywords(List<eCardKeyword> keywords)
    {
        Painless.SetActive(keywords.Contains(eCardKeyword.Painless));
        Forgetful.SetActive(keywords.Contains(eCardKeyword.Forgetful));
        Cathartic.SetActive(keywords.Contains(eCardKeyword.Cathartic));
        Power.SetActive(keywords.Contains(eCardKeyword.Power));
        Resilience.SetActive(keywords.Contains(eCardKeyword.Resilience));
        Story.SetActive(keywords.Contains(eCardKeyword.Story));
    }
}

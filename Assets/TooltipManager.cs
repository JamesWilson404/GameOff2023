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
        if (lastSelected == null)
        {
            selected = false;
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

        Painless.SetActive(currentCard.Keywords.Contains(eCardKeyword.Painless));
        Forgetful.SetActive(currentCard.Keywords.Contains(eCardKeyword.Forgetful));
        Cathartic.SetActive(currentCard.Keywords.Contains(eCardKeyword.Cathartic));
        Power.SetActive(currentCard.Keywords.Contains(eCardKeyword.Power));
        Resilience.SetActive(currentCard.Keywords.Contains(eCardKeyword.Resilience));
        Story.SetActive(currentCard.Keywords.Contains(eCardKeyword.Story));

        lastSelected = card;
    }

    public void CardUnHovered()
    {
        selected = false;
        selectedTime = 0;
        lastSelected = null;
    }

}

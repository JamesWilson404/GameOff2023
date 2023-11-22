using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum eEventType
{
    Shop,
    BadCardPick,
    GoodCardPick,
    Scales,

}


public class GameUI : MonoBehaviour
{

    [SerializeField] TMP_Text BloodTokenText;
    [SerializeField] TMP_Text HopeTokenText;

    [SerializeField] GameObject BloodTarget;

    [SerializeField] Animator animator;

    [SerializeField] BloodUITrail BloodTrail;
    [SerializeField] Camera Camera;



    private void Awake()
    {
        animator = GetComponent<Animator>();   
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public void StartTrail()
    {
        BloodTrail.StartPath(BoardManager.Instance.GetCellCenter(BoardManager.Instance.DeckPosition), BloodTarget.transform.position);
    }

    public void UpdateUI()
    {
        BloodTokenText.text = Game.Instance.BloodTokens.ToString();
        HopeTokenText.text = Game.Instance.HopeTokens.ToString();
    }


    public void LoadEventUI(eEventType eventType)
    {
        animator.SetBool("Shop", false);
        animator.SetBool("BadCard", false);
        animator.SetBool("GoodCard", false);
        animator.SetBool("Scales", false);

        BoardManager.Instance.ToggleCardDisplay(false);

        switch (eventType)
        {
            case eEventType.Shop:
                animator.SetBool("Shop", true);
                break;
            case eEventType.BadCardPick:
                animator.SetBool("BadCard", true);
                break;
            case eEventType.GoodCardPick:
                animator.SetBool("GoodCard", true);
                break;
            case eEventType.Scales:
                animator.SetBool("Scales", true);
                break;
            default:
                break;
        }

    }

    internal void FinishEvent()
    {
        animator.SetBool("Shop", false);
        animator.SetBool("BadCard", false);
        animator.SetBool("GoodCard", false);
        animator.SetBool("Scales", false);
        BoardManager.Instance.ToggleCardDisplay(true);
    }


    public void NotEnoughResources(eCardPolarity polarity)
    {
        if (polarity == eCardPolarity.Hope)
        {
            animator.Play("HopeJiggle");
        }
        else if (polarity == eCardPolarity.Blood)
        {
            animator.Play("NightmareJiggle");
        }
        AudioManager.Instance.PlaySound(SoundFX.CANT_AFFORD);
    }


    public void ResourcesAdded(eCardPolarity polarity, int amount)
    {
        if (polarity == eCardPolarity.Hope)
        {
            animator.Play("HopeAdded");
        }
        else if (polarity == eCardPolarity.Blood)
        {
            animator.Play("NightmareAdded");
        }
    
    }




}

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
    Boss,
}


public class GameUI : MonoBehaviour
{

    [SerializeField] TMP_Text BloodTokenText;
    [SerializeField] TMP_Text HopeTokenText;

    [SerializeField] GameObject BloodTarget;
    [SerializeField] GameObject ScalesTarget;

    [SerializeField] Animator animator;

    [SerializeField] BloodUITrail BloodTrail;
    [SerializeField] Camera Camera;

    [SerializeField] TheScalesUI TheScalesUI;


    private void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    public void SetScalesHealth(int max, int current)
    {
        TheScalesUI.BarFill.fillAmount = (float)current / (float)max;
    }

    public void SetScaleTurnCount(int TurnsLeft)
    {
        if (TurnsLeft > 0)
        {
            TheScalesUI.TurnsText.text = TurnsLeft.ToString() + " Rounds Until Judgement";
        }
        else
        {
            TheScalesUI.TurnsText.text = "Judgement!";
            VoiceManager.Instance.PlaySound(VoiceFX.JUDGEMENT);
        }
    }

    public void HideResources()
    {
        animator.Play("HideResources");
    }


    public void StartTrail()
    {
        if (Game.Instance.inBoss)
        {
            BloodTrail.StartPath(BoardManager.Instance.GetCellCenter(BoardManager.Instance.DeckPosition), ScalesTarget.transform.position);
        }
        else
        {
            BloodTrail.StartPath(BoardManager.Instance.GetCellCenter(BoardManager.Instance.DeckPosition), BloodTarget.transform.position);
        }
    }

    public void UpdateUI()
    {
        BloodTokenText.text = Game.Instance.BloodTokens.ToString();
        HopeTokenText.text = Game.Instance.HopeTokens.ToString();

        if (Game.Instance.inBoss)
        {
            SetScalesHealth(Game.Instance.BossMaxHealth, Game.Instance.BossHealth);
        }
    }


    public void LoadEventUI(eEventType eventType)
    {
        animator.SetBool("Shop", false);
        animator.SetBool("BadCard", false);
        animator.SetBool("GoodCard", false);
        animator.SetBool("Scales", false);
        animator.SetBool("Boss", false);

        if (eventType != eEventType.Boss)
        {
            BoardManager.Instance.ToggleCardDisplay(false);
        }

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
            case eEventType.Boss:
                animator.SetBool("Boss", true);
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
        animator.SetBool("Boss", false);
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
        if (Game.Instance.inBoss)
        {
            return;
        }

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

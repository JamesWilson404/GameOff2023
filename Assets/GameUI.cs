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

}


public class GameUI : MonoBehaviour
{

    [SerializeField] TMP_Text BloodTokenText;
    [SerializeField] TMP_Text HopeTokenText;

    [SerializeField] Animator animator;


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
            default:
                break;
        }

    }

    internal void FinishEvent()
    {
        animator.SetBool("Shop", false);
        animator.SetBool("BadCard", false);
        animator.SetBool("GoodCard", false);
    }
}

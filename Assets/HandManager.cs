using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;
    GameObject CurrentCard;

    HandRotator handRotator;
    CardLayerSorter layerSorter;

    private void Awake()
    {
        if (Instance == null)
        {
            HandManager.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        handRotator = GetComponent<HandRotator>();
        layerSorter = GetComponent<CardLayerSorter>();

    }

    internal void CardUnHovered(Canvas cardCanvas)
    {
        if (cardCanvas.gameObject == CurrentCard)
        {
            UnHoverCard(cardCanvas);
            CurrentCard = null;
            handRotator.SetHandRotation();
        }
    }

    internal void CardHovered(Canvas cardCanvas)
    {
        if (cardCanvas.gameObject == CurrentCard)
        {
            return;
        }

        // New Card Hovered
        if (CurrentCard == null)
        {
            HoverCard(cardCanvas);
            CurrentCard = cardCanvas.gameObject;
            cardCanvas.transform.transform.rotation = Quaternion.identity;
        }
        // Unhover old card, hover on new one
        else
        {
            UnHoverCard(CurrentCard.GetComponent<Canvas>());
            handRotator.SetHandRotation();
            HoverCard(cardCanvas);
            cardCanvas.transform.transform.rotation = Quaternion.identity;
            CurrentCard = cardCanvas.gameObject;
        }
    }

    private void UnHoverCard(Canvas cardCanvas)
    {
        cardCanvas.sortingOrder = cardCanvas.transform.GetSiblingIndex();
        cardCanvas.transform.localScale = Vector3.one * 1f;
        var anim = cardCanvas.transform.GetComponent<Animator>();
        anim.SetBool("Selected", false);
        anim.Play("Default");
    }

    private void HoverCard(Canvas cardCanvas)
    {
        cardCanvas.sortingOrder = 99;
        cardCanvas.transform.localScale = Vector3.one * 1.2f;
        cardCanvas.transform.GetComponent<Animator>().SetBool("Selected", true);
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
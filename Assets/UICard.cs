using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class UICard : MonoBehaviour
{
    protected AudioSource AudioPlayer;
    [SerializeField] protected AudioClip Audio_CardPlacement;

    public SpriteRenderer spriteRenderer;

    public Card CurrentCard;


    public virtual void Init(Card newCard)
    {
        CurrentCard = newCard;
        spriteRenderer.sprite = newCard.CardArt;
    }

    private void Awake()
    {
        AudioPlayer = GetComponent<AudioSource>();
    }

    public void PlayPlacementAudio()
    {
        AudioPlayer.PlayOneShot(Audio_CardPlacement);
    }

}

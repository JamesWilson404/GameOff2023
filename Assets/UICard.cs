using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICard : MonoBehaviour
{
    AudioSource AudioPlayer;
    [SerializeField] AudioClip Audio_CardPlacement;

    public SpriteRenderer spriteRenderer;

    public Card CurrentCard;

    private void Awake()
    {
        AudioPlayer = GetComponent<AudioSource>();
    }


    public void PlayPlacementAudio()
    {
        AudioPlayer.PlayOneShot(Audio_CardPlacement);
    }

    internal void Init(Card newCard)
    {
        CurrentCard = newCard;
        spriteRenderer.sprite = newCard.CardArt;
    }
}

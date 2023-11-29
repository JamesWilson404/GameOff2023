using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class UICard : MonoBehaviour
{
    protected AudioSource AudioPlayer;
    [SerializeField] protected AudioClip Audio_CardPlacement;

    public SpriteRenderer CardShadow;
    public SpriteRenderer CardBacking;
    public SpriteRenderer CardSprite;
    public Collider2D collider2D;

    public Card CurrentCard;

    public Vector2Int position; 

    public virtual void Init(Card newCard, Vector2Int pos)
    {
        CurrentCard = newCard;
        CardSprite.sprite = newCard.CardArt;
        position = pos;
    }

    private void Awake()
    {
        AudioPlayer = GetComponent<AudioSource>();
        collider2D = GetComponent<Collider2D>();
    }

    public void PlayPlacementAudio()
    {
        AudioPlayer.PlayOneShot(Audio_CardPlacement);
    }

    public void ToggleVisibility(bool v)
    {
        CardShadow.enabled = v;
        CardBacking.enabled = v;
        CardSprite.enabled = v;
        collider2D.enabled = v;

    }


}

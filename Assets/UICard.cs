using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICard : MonoBehaviour
{
    AudioSource AudioPlayer;
    [SerializeField] AudioClip Audio_CardPlacement;

    public bool RequiresBloodTrail;
    [SerializeField] GameObject BloodTrailPrefab;
    public int NumberOfTrails;

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

    public void TrySpawnBloodTrail()
    {
        if (RequiresBloodTrail)
        {
            for (int i = 0; i < NumberOfTrails; i++)
            {
                var newBloodTrail = Instantiate(BloodTrailPrefab, transform.position, Quaternion.identity, BoardManager.Instance.BloodParent.transform);
                newBloodTrail.GetComponent<BloodTrail>().Init();
            }
            AudioManager.Instance.PlaySound(SoundFX.BLOOD_SPLAT);
        }
    }    

}

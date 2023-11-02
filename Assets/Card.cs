using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    AudioSource AudioPlayer;
    [SerializeField] AudioClip Audio_CardPlacement;

    private void Awake()
    {
        AudioPlayer = GetComponent<AudioSource>();
    }


    public void PlayPlacementAudio()
    {
        AudioPlayer.PlayOneShot(Audio_CardPlacement);
    }

}

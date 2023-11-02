using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundFX
{
    CARD_DRAWN,
    CARD_BURN,
}

public class AudioManager : MonoBehaviour
{
    public AudioClip CardDrawm;
    public AudioClip CardBurn;



    private AudioSource source;
    public static AudioManager Instance;

    void Awake()
    {
        AudioManager.Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundFX sound)
    {
        switch (sound)
        {
            case SoundFX.CARD_DRAWN:
                source.PlayOneShot(CardDrawm);
                break;

            case SoundFX.CARD_BURN:
                source.PlayOneShot(CardBurn);
                break;

            default:
                break;
        }
    }
}


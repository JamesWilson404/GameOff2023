using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundFX
{
    CARD_DRAWN,
    CARD_BURN,
    BLOOD_SPLAT,
    CANT_AFFORD,
    GHOUL,
    HOPE,
    NIGHTMARE,
    BlOOD_DRIP,
    BOOP,
    BONE_SNAP,
    BELL,
    WRITING,
    GULP,
    UIHOVER,
    UICLICK,
    THUD,
}

public class AudioManager : MonoBehaviour
{
    public AudioClip CardDrawm;
    public AudioClip CardBurn;
    public AudioClip BloodSplat;
    public AudioClip CantAfford;
    public AudioClip Ghoul;
    public AudioClip Nightmare;
    public AudioClip Hope;
    public AudioClip BloodDrip;
    public AudioClip Boop;
    public AudioClip BoneSnap;
    public AudioClip Bell;
    public AudioClip Writting;
    public AudioClip Gulp;
    public AudioClip Hover;
    public AudioClip Click;
    public AudioClip Thud;


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

            case SoundFX.BLOOD_SPLAT:
                source.PlayOneShot(BloodSplat);
                break;

            case SoundFX.CANT_AFFORD:
                source.PlayOneShot(CantAfford);
                break;

            case SoundFX.GHOUL:
                source.PlayOneShot(Ghoul);
                break;

            case SoundFX.THUD:
                source.PlayOneShot(Thud);
                break;

            case SoundFX.HOPE:
                source.PlayOneShot(Hope);
                break;

            case SoundFX.NIGHTMARE:
                source.PlayOneShot(Nightmare);
                break;

            case SoundFX.BOOP:
                source.PlayOneShot(Boop);
                break;

            case SoundFX.BONE_SNAP:
                source.PlayOneShot(BoneSnap);
                break;

            case SoundFX.BlOOD_DRIP:
                source.PlayOneShot(BloodDrip);
                break;

            case SoundFX.BELL:
                source.PlayOneShot(Bell);
                break;

            case SoundFX.WRITING:
                source.PlayOneShot(Writting);
                break;
            case SoundFX.GULP:
                source.PlayOneShot(Gulp);
                break;

            case SoundFX.UIHOVER:
                source.PlayOneShot(Hover);
                break;

            case SoundFX.UICLICK:
                source.PlayOneShot(Click);
                break;


            default:
                break;
        }
    }
}


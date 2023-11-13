using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum VoiceFX
{
    THE_DECEASED,
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
}

public class VoiceManager : MonoBehaviour
{
    public AudioClip TheDeceased;
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



    private AudioSource source;
    public static VoiceManager Instance;

    void Awake()
    {
        VoiceManager.Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(VoiceFX sound)
    {
        switch (sound)
        {
            case VoiceFX.THE_DECEASED:
                source.PlayOneShot(TheDeceased);
                break;

            case VoiceFX.CARD_BURN:
                source.PlayOneShot(CardBurn);
                break;

            case VoiceFX.BLOOD_SPLAT:
                source.PlayOneShot(BloodSplat);
                break;

            case VoiceFX.CANT_AFFORD:
                source.PlayOneShot(CantAfford);
                break;

            case VoiceFX.GHOUL:
                source.PlayOneShot(Ghoul);
                break;

            default:
                break;
        }
    }
}


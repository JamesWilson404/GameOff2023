using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum VoiceFX
{
    NIGHTMARE1,
    NIGHTMARE2,
    NIGHTMARE3,
    NIGHTMARE4,
    INTRO1,
    INTRO2,
    INTRO3,
    HOPE1,
    HOPE2,
    HOPE3,
    BOSSINTRO,
    BOSSLOSE,
    BOSSWIN,
    JUDGEMENT,
    THE_DECEASED,
    THE_INNOCENT,
    THE_ADDICTED,
    THE_REJECTED,
    THE_TRAPPED,
    THE_DERANGED,
    THE_DEMONS,
    THE_GREEDY,
    THE_ISOLATED,
}

public class VoiceManager : MonoBehaviour
{
    public AudioClip TheDeceased;
    public AudioClip TheInnocent;
    public AudioClip TheAddicted;
    public AudioClip TheRejected;
    public AudioClip TheTrapped;
    public AudioClip TheDeranged;
    public AudioClip TheDemons;
    public AudioClip TheGreedy;
    public AudioClip TheIsolated;
    public AudioClip Nightmare1;
    public AudioClip Nightmare2;
    public AudioClip Nightmare3;
    public AudioClip Nightmare4;
    public AudioClip Intro1;
    public AudioClip Intro2;
    public AudioClip Intro3;
    public AudioClip Hope1;
    public AudioClip Hope2;
    public AudioClip Hope3;
    public AudioClip BossIntro;
    public AudioClip BossLose;
    public AudioClip BossWin;
    public AudioClip Judgement;
    



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
            case VoiceFX.THE_INNOCENT:
                source.PlayOneShot(TheInnocent);
                break;
            case VoiceFX.THE_ADDICTED:
                source.PlayOneShot(TheAddicted);
                break;
            case VoiceFX.THE_REJECTED:
                source.PlayOneShot(TheRejected);
                break;
            case VoiceFX.THE_TRAPPED:
                source.PlayOneShot(TheTrapped);
                break;
            case VoiceFX.THE_DERANGED:
                source.PlayOneShot(TheDeranged);
                break;
            case VoiceFX.THE_DEMONS:
                source.PlayOneShot(TheDemons);
                break;
            case VoiceFX.THE_GREEDY:
                source.PlayOneShot(TheGreedy);
                break;
            case VoiceFX.THE_ISOLATED:
                source.PlayOneShot(TheIsolated);
                break;
            case VoiceFX.NIGHTMARE1:
                source.PlayOneShot(Nightmare1);
                break;
            case VoiceFX.NIGHTMARE2:
                source.PlayOneShot(Nightmare2);
                break;
            case VoiceFX.NIGHTMARE3:
                source.PlayOneShot(Nightmare3);
                break;
            case VoiceFX.NIGHTMARE4:
                source.PlayOneShot(Nightmare4);
                break;
            case VoiceFX.INTRO1:
                source.PlayOneShot(Intro1);
                break;
            case VoiceFX.INTRO2:
                source.PlayOneShot(Intro2);
                break;
            case VoiceFX.INTRO3:
                source.PlayOneShot(Intro3);
                break;
            case VoiceFX.HOPE1:
                source.PlayOneShot(Hope1);
                break;
            case VoiceFX.HOPE2:
                source.PlayOneShot(Hope2);
                break;
            case VoiceFX.HOPE3:
                source.PlayOneShot(Hope3);
                break;
            case VoiceFX.BOSSINTRO:
                source.PlayOneShot(BossIntro);
                break;
            case VoiceFX.BOSSLOSE:
                source.PlayOneShot(BossLose);
                break;
            case VoiceFX.BOSSWIN:
                source.PlayOneShot(BossWin);
                break;
            case VoiceFX.JUDGEMENT:
                source.PlayOneShot(Judgement);
                break;
            default:
                // Handle any additional cases or provide a default action.
                break;
        }
    }
}


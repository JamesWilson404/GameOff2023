using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCard : MonoBehaviour
{
    public Sprite TestSprite;
    public Sprite ScaleSprite;
    public SpriteRenderer CardIcon;

    public StoryTitle storyTitle;

    [SerializeField] Animator animator;

    public void SetSprite()
    {
        CardIcon.sprite = TestSprite;
    }


    public void StartStory()
    {
        gameObject.SetActive(true);
        animator.Play("StoryPlaced");
    }

    public void PlayStoryCard()
    {
        //AudioManager.Instance.PlaySound(SoundFX.BELL);
    }

    public void StoryLanded()
    {
        AudioManager.Instance.PlaySound(SoundFX.CARD_DRAWN);
        storyTitle.SetTitle("The Deceased");
        Debug.Log("LANDED");
    }

    public void PlayVoiceLine()
    {
        VoiceManager.Instance.PlaySound(VoiceFX.THE_DECEASED);
    }

    public void RevealStory()
    {
        animator.SetTrigger("Reveal");
    }
    
    public void RevealStorySprite()
    {
        CardIcon.sprite = ScaleSprite;
    }

}

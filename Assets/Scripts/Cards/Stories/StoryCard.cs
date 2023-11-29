using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoryCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite TestSprite;
    public Sprite ScaleSprite;
    public SpriteRenderer CardIcon;

    public StoryTitle storyTitle;
    public StoryData storyData;


    [SerializeField] Animator animator;

    public void SetSprite()
    {
        CardIcon.sprite = storyData.StorySprite;
    }


    public void StartStory(StoryData data)
    {
        storyData = data;
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
        storyTitle.SetTitle(storyData.StoryTitle);
        Debug.Log("LANDED");
    }

    public void PlayVoiceLine()
    {
        VoiceManager.Instance.PlaySound(storyData.CardVO);
    }

    public void RevealStory()
    {
        animator.SetTrigger("Reveal");
    }
    
    public void RevealStorySprite()
    {
        CardIcon.sprite = ScaleSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Game.Instance.GameTooltip.CardUnHovered();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Game.Instance.CurrentStory == null)
        {
            return;
        }
        if (Game.Instance.TurnState == Game.eTurnState.InTurn || Game.Instance.TurnState == Game.eTurnState.InEvent)
        {
            Game.Instance.GameTooltip.CardHovered(Game.Instance.CurrentStory, gameObject);
        }
    }

}

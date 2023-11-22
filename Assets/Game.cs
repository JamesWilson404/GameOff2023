using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Game : MonoBehaviour
{
    public static Game Instance;
    public enum eTurnState
    {
        PreStart,
        StartOfTurn,
        InTurn,
        EndTurn,
        ResetTurn,
        PreEvent,
        InEvent,
        PostEvent,
        PresentStory,
        EndOfRound,
        Judgement,
        PostJudgement,
        StartOfRound,
    }


    [HideInInspector] public float TimeInState = 0;
    public eTurnState TurnState = eTurnState.PreStart;

    public System.Random rand;

    public int BloodTokens;
    public int HopeTokens;

    public GameUI GameUI;
    public StoryCard StoryCard;

    bool changedState;
    bool StartOfPhase = true;

    public Camera GameCamera;
    public Camera BloodCamera;

    float ZoomLevel = 3;
    float ZoomLerpRate = 0.1f;

    public bool EndOfRound = false;
    public int HopeSubmitted = 0;

    public StoryData[] Stories;
    public StoryData CurrentStory;

    public GameEvents EventManager;


    private void Awake()
    {
        if (Game.Instance == null)
        {
            Game.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        rand = new System.Random((int) DateTime.Now.TimeOfDay.TotalSeconds);
        EventManager = new GameEvents();
    }

    private void Update()
    {
        if (!changedState)
        {
            TimeInState += Time.deltaTime;
        }
        changedState = false;
        ResolveTurnState();
        GameUI.UpdateUI();
        ResolveCameraZoom();
    }

    private void ResolveCameraZoom()
    {
        var newZoom = Mathf.Lerp(GameCamera.orthographicSize, ZoomLevel, ZoomLerpRate);
        GameCamera.orthographicSize = newZoom;
        BloodCamera.orthographicSize = newZoom;
    }

    public void AwardResource(eCardPolarity polarity, int value)
    {
        if (polarity == eCardPolarity.Hope)
        {
            HopeTokens += value;
            AudioManager.Instance.PlaySound(SoundFX.HOPE);
        }
        else if (polarity == eCardPolarity.Blood)
        {
            BloodTokens += value;
            AudioManager.Instance.PlaySound(SoundFX.BlOOD_DRIP);
        }

        GameUI.ResourcesAdded(polarity, value);
    }

    private void ResolveTurnState()
    {
        switch (TurnState)
        {
            case eTurnState.PreStart:
                if (TimeInState == 0)
                {

                }

                if (TimeInState > 3f)
                {
                    if (StartOfPhase)
                    {
                        SwitchToState(eTurnState.PresentStory);
                    }
                    else
                    {
                        SwitchToState(eTurnState.StartOfTurn);
                    }
                }
                
                break;
            case eTurnState.StartOfTurn:
                if (TimeInState == 0)
                {
                    ZoomLevel = 3;
                    ZoomLerpRate = 0.05f;
                    HandManager.Instance.DrawHand();
                }
                if (TimeInState > 2f)
                {
                    SwitchToState(eTurnState.InTurn);
                }

                break;

            case eTurnState.EndOfRound:
                if (TimeInState == 0)
                {
                    StoryCard.RevealStory();
                }
                if (TimeInState > 2f)
                {
                    SwitchToState(eTurnState.Judgement);
                }

                break;

            case eTurnState.Judgement:
                if (TimeInState == 0)
                {
                    StartCoroutine(BoardManager.Instance.SpawnScaleCards());
                }
                if (TimeInState > 2f)
                {
                    GameUI.LoadEventUI(eEventType.Scales);
                }

                break;

            case eTurnState.PresentStory:
                if (TimeInState == 0)
                {
                    ZoomLevel = 2;
                    ZoomLerpRate = 0.1f;
                    CurrentStory = PickNewStory();
                    StoryCard.StartStory(CurrentStory);
                }
                if (TimeInState > 6f)
                {
                    SwitchToState(eTurnState.StartOfTurn);
                }

                break;
            case eTurnState.InTurn:
                if (TimeInState == 0)
                {

                }


                break;
            case eTurnState.InEvent:
                if (TimeInState == 0)
                {

                }


                break;
            case eTurnState.EndTurn:
                if (TimeInState == 0)
                {
                    EndTurn();
                }
                if (TimeInState > 2.5f && !EndOfRound)
                {
                    SwitchToState(eTurnState.PreEvent);
                }

                if (TimeInState > 5f && EndOfRound)
                {
                    SwitchToState(eTurnState.EndOfRound);
                    EndOfRound = false;
                }


                break;
            case eTurnState.ResetTurn:
                if (TimeInState == 0)
                {
                }
                if (TimeInState > 2f)
                {
                    SwitchToState(eTurnState.StartOfTurn);
                }
                break;

            case eTurnState.PreEvent:
                if (TimeInState == 0)
                {
                }

                if (TimeInState > 1.5f)
                {
                    ZoomLerpRate = 0.1f;
                    ZoomLevel = 2;
                }
                if (TimeInState > 3f)
                {
                    StartCoroutine(BoardManager.Instance.PresentShop());
                    SwitchToState(eTurnState.InEvent);
                }
                break;

            case eTurnState.PostEvent:
                if (TimeInState == 0)
                {
                }
                if (TimeInState > 1f)
                {
                    SwitchToState(eTurnState.ResetTurn);
                }
                break;

            case eTurnState.PostJudgement:
                if (TimeInState == 0)
                {
                    StartCoroutine(DeligateJudgementRewards());
                }
                if (TimeInState > 1f)
                {
                    
                }
                break;

            case eTurnState.StartOfRound:
                if (TimeInState == 0)
                {
                    DeckManager.Instance.ReshuffleDeck();
                }
                if (TimeInState > 1f)
                {
                    SwitchToState(eTurnState.StartOfTurn);
                }
                break;


            default:
                break;
        }
    }

    private IEnumerator DeligateJudgementRewards()
    {
        yield return new WaitForSeconds(1f);
        BoardManager.Instance.ToggleCardDisplay(true);

        if (HopeSubmitted == BloodTokens)
        {
            SwitchToState(eTurnState.StartOfRound);
        }
        else if (HopeSubmitted > BloodTokens)
        {
            HandManager.Instance.AddCardToHand(CurrentStory.GoodRewards[0]);
            HandManager.Instance.AddCardToHand(CurrentStory.GoodRewards[1]);
        }
        else if (BloodTokens > HopeSubmitted)
        {
            HandManager.Instance.AddCardToHand(CurrentStory.BadRewards[0]);
            HandManager.Instance.AddCardToHand(CurrentStory.BadRewards[1]);
        }

        HopeSubmitted = 0;
        BloodTokens = 0;
        GameUI.UpdateUI();
    }

    private StoryData PickNewStory()
    {
        return Stories[rand.Next(Stories.Length)];
    }

    public void SubmitJudgement()
    {
        HopeTokens -= HopeSubmitted;
        GameUI.UpdateUI();


        GameUI.FinishEvent();
        SwitchToState(eTurnState.PostJudgement);
    }

    
    public void RewardPlayed()
    {
        HandManager.Instance.DiscardHand();
        SwitchToState(eTurnState.StartOfRound);
    }

    internal bool TryPurchase(eCardPolarity polarity, int cost)
    {
        if (polarity == eCardPolarity.Hope)
        {
            if (HopeTokens >= cost)
            {
                HopeTokens -= cost;
                GameUI.UpdateUI();
                return true;
            }
            else
            {
                GameUI.NotEnoughResources(polarity);
                return false;
            }
        }
        else if (polarity == eCardPolarity.Blood)
        {
            if (BloodTokens >= cost)
            {
                BloodTokens -= cost;
                GameUI.UpdateUI();
                return true;
            }
            else
            {
                GameUI.NotEnoughResources(polarity);
                return false;
            }
        }
        return false;
    }

    void SwitchToState(eTurnState newState)
    {
        TurnState = newState;
        TimeInState = 0;
        changedState = true;
    }


    public void EndTurn()
    {
        StartCoroutine(BoardManager.Instance.AwardBloodTokens());
        StartCoroutine(BoardManager.Instance.CleanUpCards());
    }


    public void UIEndTurnPressed()
    {
        if (TurnState != eTurnState.InTurn)
        {
            return;
        }
        SwitchToState(eTurnState.EndTurn);
    }

    public void FinishEvent()
    {
        GameUI.FinishEvent();
        SwitchToState(eTurnState.PostEvent);
    }
}

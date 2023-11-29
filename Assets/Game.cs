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
        Boss,
        GameOver,
        GameWon,
        Preamble
    }



    [HideInInspector] public float TimeInState = 0;
    public eTurnState TurnState = eTurnState.Preamble;

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

    public StoryData[] StoriesAct1;
    public StoryData[] StoriesAct2;
    public StoryData[] StoriesAct3;
    public StoryData CurrentStory;

    public GameEvents EventManager;
    public int StoriesRevealed = 0;
    int MaxStories = 3;

    public bool inBoss = false;
    public int turnsInBoss = 0;
    public int MaxTurnsInBoss = 3;

    public int BossMaxHealth = 100;
    public int BossHealth = 100;

    public int hopeThisTurn = 0;
    public int nightmareThisTurn = 0;

    public TooltipManager GameTooltip;
    public TooltipManager ShopTooltip;

    public MusicTargetVolume GameMusic;

    public Card[] HopeCards;
    public Card[] FearCards;


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

        rand = new System.Random(Guid.NewGuid().GetHashCode());
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
            hopeThisTurn += value;
            AudioManager.Instance.PlaySound(SoundFX.HOPE);
        }
        else if (polarity == eCardPolarity.Blood)
        {
            BloodTokens += value;
            nightmareThisTurn += value;
            AudioManager.Instance.PlaySound(SoundFX.BlOOD_DRIP);
        }

        EventManager.OnResourceGained(polarity, value);
        GameUI.ResourcesAdded(polarity, value);
    }

    private void ResolveTurnState()
    {
        switch (TurnState)
        {
            case eTurnState.PreStart:
                if (TimeInState == 0)
                {
                    if (inBoss)
                    {
                        turnsInBoss++;
                        GameUI.SetScaleTurnCount(MaxTurnsInBoss - turnsInBoss);
                        AudioManager.Instance.PlaySound(SoundFX.BELL);
                    }
                    if (StoriesRevealed > 0)
                    {
                        GameMusic.SetVolume(0.1f, 0.01f);
                    }
                    else
                    {
                        GameMusic.SetVolume(0.0f, 0.01f);
                    }
                }

                if (TimeInState > 5f)
                {
                    if (StoriesRevealed < MaxStories)
                    {
                        SwitchToState(eTurnState.PresentStory);
                        
                    }
                    else
                    {
                        SwitchToState(eTurnState.Boss);
                    }
                }
                
                break;

            case eTurnState.Preamble:
                if (TimeInState == 0)
                {

                }

                if (TimeInState > 2f)
                {

                }

                break;

            case eTurnState.Boss:
                if (TimeInState == 0)
                {
                    if (!inBoss)
                    {
                        VoiceManager.Instance.PlaySound(VoiceFX.BOSSINTRO);
                    }

                    inBoss = true;
                    GameUI.LoadEventUI(eEventType.Boss);
                    GameUI.HideResources();
                }

                if (TimeInState > 1f)
                {
                    SwitchToState(eTurnState.StartOfTurn);
                }

                break;
            case eTurnState.StartOfTurn:
                if (TimeInState == 0)
                {
                    hopeThisTurn = 0;
                    nightmareThisTurn = 0;
                    HandManager.Instance.CardDrawnThisTurn = 0;
                    
                    ZoomLevel = 3;
                    ZoomLerpRate = 0.05f;
                    HandManager.Instance.DrawHand();
                    StoryCard.storyData.OnTurnStart();

                }
                if (TimeInState > 2f)
                {
                    SwitchToState(eTurnState.InTurn);
                }

                break;

            case eTurnState.GameOver:
                if (TimeInState == 0)
                {
                    VoiceManager.Instance.PlaySound(VoiceFX.BOSSLOSE);
                    Debug.Log("GAME LOST");
                }
                if (TimeInState > 2f)
                {
                }

                break;

            case eTurnState.GameWon:
                if (TimeInState == 0)
                {
                    VoiceManager.Instance.PlaySound(VoiceFX.BOSSWIN);
                    Debug.Log("GAME WON");
                }
                if (TimeInState > 2f)
                {
                }

                break;



            case eTurnState.EndOfRound:
                if (TimeInState == 0)
                {
                    if (inBoss)
                    {
                        SwitchToState(eTurnState.StartOfRound);
                    }
                    else
                    {
                        StoryCard.RevealStory();
                    }
                }
                if (TimeInState > 2f)
                {
                    SwitchToState(eTurnState.Judgement);
                }

                break;

            case eTurnState.Judgement:
                if (TimeInState == 0)
                {
                    BoardManager.Instance.ToggleCardDisplay(false);
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
                    StoriesRevealed++;
                }
                if (TimeInState > 6f)
                {
                    StoryCard.storyData.OnRoundStart();
                    SwitchToState(eTurnState.StartOfTurn);
                }

                break;
            case eTurnState.InTurn:
                if (TimeInState == 0)
                {
                    GameMusic.SetVolume(0.3f, 0.005f);
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
                    CurrentStory.OnTurnEnd();
                    EndTurn();
                }
                if (TimeInState > 3.5f && !EndOfRound)
                {
                    if (inBoss)
                    {
                        SwitchToState(eTurnState.ResetTurn);
                    }
                    else
                    {
                        SwitchToState(eTurnState.PreEvent);
                    }
                }


                if (TimeInState > 5f && EndOfRound)
                {
                    if (turnsInBoss >= MaxTurnsInBoss)
                    {
                        SwitchToState(eTurnState.GameOver);
                    }
                    else
                    {
                        SwitchToState(eTurnState.EndOfRound);
                        EndOfRound = false;
                    }
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
                    switch (rand.Next(3))
                    {
                        case 0:
                            StartCoroutine(BoardManager.Instance.PresentShop());
                            break;
                        case 1:
                            StartCoroutine(BoardManager.Instance.PresentHopePick());
                            break;
                        case 2:
                            StartCoroutine(BoardManager.Instance.PresentFearPick());
                            break;
                        default:
                            StartCoroutine(BoardManager.Instance.PresentShop());
                            break;
                    }


                    SwitchToState(eTurnState.InEvent);
                }
                break;

            case eTurnState.PostEvent:
                if (TimeInState == 0)
                {
                }
                if (TimeInState > 1f)
                {
                    BoardManager.Instance.ToggleCardDisplay(true);
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
                    SwitchToState(eTurnState.PreStart);
                }
                break;


            default:
                break;
        }
    }

    private IEnumerator DeligateJudgementRewards()
    {
        yield return new WaitForSeconds(1f);

        if (HopeSubmitted == BloodTokens)
        {
            VoiceManager.Instance.PlaySound(VoiceFX.HOPE3);
            SwitchToState(eTurnState.StartOfRound);
        }
        else if (HopeSubmitted > BloodTokens)
        {
            HandManager.Instance.AddCardToHand(CurrentStory.GoodRewards[0]);
            HandManager.Instance.AddCardToHand(CurrentStory.GoodRewards[1]);
            VoiceManager.Instance.PlaySound((VoiceFX)(7 + rand.Next(3)));
        }
        else if (BloodTokens > HopeSubmitted)
        {
            HandManager.Instance.AddCardToHand(CurrentStory.BadRewards[0]);
            HandManager.Instance.AddCardToHand(CurrentStory.BadRewards[1]);
            VoiceManager.Instance.PlaySound((VoiceFX)(rand.Next(4)));
        }

        HopeSubmitted = 0;
        BloodTokens = 0;
        GameUI.UpdateUI();

        yield return new WaitForSeconds(1f);
        BoardManager.Instance.ToggleCardDisplay(true);

    }


    internal void HealBoss(int value)
    {
        BossHealth = Mathf.Clamp(BossHealth + value, 0, BossMaxHealth);
    }

    internal void DamageBoss(int value)
    {
        BossHealth = Mathf.Clamp(BossHealth - value, 0, BossMaxHealth);
        if (BossHealth == 0)
        {
            SwitchToState(eTurnState.GameWon);
        }
    }

    private StoryData PickNewStory()
    {
        StoryData s = null;
        switch (StoriesRevealed)
        {
            case 0:
                s = StoriesAct1[rand.Next(StoriesAct1.Length)];
                break;
            case 1:
                s = StoriesAct2[rand.Next(StoriesAct2.Length)];
                break;
            case 2:
                s = StoriesAct3[rand.Next(StoriesAct3.Length)];
                break;
            default:
                s = StoriesAct1[rand.Next(StoriesAct1.Length)];
                break;
        }
        return s;

    }

    public void SubmitJudgement()
    {
        HopeTokens -= HopeSubmitted;
        GameUI.UpdateUI();


        GameUI.FinishEvent();
        SwitchToState(eTurnState.PostJudgement);
    }

    public void StartGame()
    {
        SwitchToState(eTurnState.PreStart);
        DeckManager.Instance.LandDeck();
    }

    
    public void RewardPlayed()
    {
        HandManager.Instance.DestroyHand();
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

        BoardManager.Instance.BloodTileGenerated = 0;
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

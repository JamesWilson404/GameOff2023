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
    }

    [HideInInspector] public float TimeInState = 0;
    public eTurnState TurnState = eTurnState.PreStart;

    public System.Random rand;

    public int BloodTokens;
    public int HopeTokens;

    public GameUI GameUI;

    bool changedState;

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
    }



    private void ResolveTurnState()
    {
        switch (TurnState)
        {
            case eTurnState.PreStart:
                if (TimeInState == 0)
                {

                }

                if (TimeInState > 2f)
                {
                    SwitchToState(eTurnState.StartOfTurn);
                }
                

                break;
            case eTurnState.StartOfTurn:
                if (TimeInState == 0)
                {
                    HandManager.Instance.DrawHand();
                }
                if (TimeInState > 2f)
                {
                    SwitchToState(eTurnState.InTurn);
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
                }
                if (TimeInState > 2.5f)
                {
                    EndTurn();
                    SwitchToState(eTurnState.PreEvent);
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
                if (TimeInState > 1f)
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

            default:
                break;
        }
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

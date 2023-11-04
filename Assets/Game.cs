using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    [HideInInspector] public float TimeInState = 0;
    public eTurnState TurnState = eTurnState.PreStart;

    public System.Random rand;



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
                    SwitchToState(eTurnState.InTurn);
                }


                break;
            case eTurnState.InTurn:
                if (TimeInState == 0)
                {

                }


                break;
            case eTurnState.EndTurn:
                if (TimeInState == 0)
                {
                    EndTurn();
                }
                if (TimeInState > 1f)
                {
                    SwitchToState(eTurnState.ResetTurn);
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
            default:
                break;
        }
    }

    private void SwitchToState(eTurnState newState)
    {
        TurnState = newState;
        TimeInState = 0;
        changedState = true;
    }


    public void EndTurn()
    {
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

}

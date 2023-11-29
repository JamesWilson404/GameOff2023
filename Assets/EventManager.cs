using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UIEventCard CurrentCard;
    public Camera GameCamera;

    public bool Dragging = false;
    public Vector3 returnPosition = Vector3.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            EventManager.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

    }




    internal void CardHovered(UIEventCard cardHovered)
    {
        if (CurrentCard != null || Dragging)
        {
            return;
        }
        else
        {
            CurrentCard = cardHovered;
        }
    }

    public void CardUnHovered(UIEventCard cardHovered)
    {
        if (CurrentCard == cardHovered && !Dragging)
        {
            CurrentCard = null;
        }
    }


    private void Update()
    {
        if (Game.Instance.TurnState != Game.eTurnState.InEvent)
        {
            return;
        }

        var mousePosition = GameCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        if (Dragging)
        {
            if (Input.GetMouseButton(0))
            {
                CurrentCard.transform.position = mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                CardDropped(mousePosition);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CurrentCard != null)
                {
                    Dragging = true;
                    returnPosition = CurrentCard.transform.position;
                    CurrentCard.StartDragging();
                }
            }
        }
    }

    private void CardDropped(Vector3 mousePos)
    {
        var Coords2D = BoardManager.Instance.GetNearestTile(mousePos);
        if (BoardManager.Instance.IsDeckPosition(Coords2D))
        {
            if (TryPurchaseCard())
            {
                AudioManager.Instance.PlaySound(SoundFX.CARD_BURN);
                CurrentCard = null;
            }
            else
            {
                CurrentCard.transform.position = returnPosition;
                AudioManager.Instance.PlaySound(SoundFX.CARD_DRAWN);
                CurrentCard.StopDragging();
            }
        }
        else
        {
            CurrentCard.transform.position = returnPosition;
            AudioManager.Instance.PlaySound(SoundFX.CARD_DRAWN);
            CurrentCard.StopDragging();
        }

        returnPosition = Vector3.zero;
        Dragging = false;
    }

    private bool TryPurchaseCard()
    {
        if (BoardManager.Instance.CurrentEvent == eEventType.BadCardPick || BoardManager.Instance.CurrentEvent == eEventType.GoodCardPick)
        {
            DeckManager.Instance.AddCardToDeck(CurrentCard.CurrentCard);
            Destroy(CurrentCard.gameObject);
            Debug.Log("ADDED");
            BoardManager.Instance.FinishEvent();
            return true;
        }


        if (Game.Instance.TryPurchase(CurrentCard.CurrentCard.polarity, CurrentCard.CurrentCard.cost))
        {
            DeckManager.Instance.AddCardToDeck(CurrentCard.CurrentCard);
            Destroy(CurrentCard.gameObject);
            Debug.Log("BOUGHT");
        }
        return false;
    }
}

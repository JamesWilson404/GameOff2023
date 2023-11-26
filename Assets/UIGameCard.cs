using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGameCard : UICard, IPointerEnterHandler, IPointerExitHandler
{
    public bool RequiresBloodTrail;
    [SerializeField] GameObject BloodTrailPrefab;
    public int NumberOfTrails;

    public void TrySpawnBloodTrail()
    {
        if (RequiresBloodTrail)
        {
            for (int i = 0; i < NumberOfTrails; i++)
            {
                var newBloodTrail = Instantiate(BloodTrailPrefab, transform.position, Quaternion.identity, BoardManager.Instance.BloodParent.transform);
                newBloodTrail.GetComponent<BloodTrail>().Init();
            }
            AudioManager.Instance.PlaySound(SoundFX.BLOOD_SPLAT);
        }
    }

    public void Placed()
    {
        Game.Instance.EventManager.OnCardPlayed(this, CurrentCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Game.Instance.GameTooltip.CardUnHovered();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Game.Instance.TurnState == Game.eTurnState.InTurn && !HandManager.Instance.Placing)
        {
            Game.Instance.GameTooltip.CardHovered(CurrentCard, gameObject);
        }

    }
}

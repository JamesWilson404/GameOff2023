using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameCard : UICard
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
        Game.Instance.EventManager.OnCardPlayed(CurrentCard);
    }
}

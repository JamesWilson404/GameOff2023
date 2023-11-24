using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardCard : MonoBehaviour
{
    
    public UIHandCard card;

    public void DestoryCard()
    {
        Destroy(this.gameObject);
    }

    public void DiscardAudio()
    {
        AudioManager.Instance.PlaySound(SoundFX.CARD_DRAWN);
    }

}

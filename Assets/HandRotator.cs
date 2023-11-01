using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotator : MonoBehaviour
{
    public float RotationPerCard;

    // Start is called before the first frame update
    void Start()
    {
        SetHandRotation();
    }

    public void SetHandRotation()
    {
        var cardsInHand = transform.childCount;
        var halfCount = Mathf.FloorToInt(cardsInHand / 2);
        for (int i = -halfCount; i < cardsInHand - halfCount; i++)
        {
            transform.GetChild(i + halfCount).transform.rotation = Quaternion.Euler(0, 0, i * RotationPerCard);
        }
    }
}

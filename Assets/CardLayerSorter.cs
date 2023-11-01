using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayerSorter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResolveCardOrdering();
    }

    public void ResolveCardOrdering()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var canvas = transform.GetChild(i).GetComponent<Canvas>();
            canvas.sortingOrder = i;
        }
    }


}

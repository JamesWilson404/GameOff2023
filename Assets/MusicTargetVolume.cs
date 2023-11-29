using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTargetVolume : MonoBehaviour
{
    public AudioSource Source;
    public float TargetVolume;
    public float LerpRate;



    private void Update()
    {
        Source.volume = Mathf.Lerp(Source.volume, TargetVolume, LerpRate);
    }

    internal void SetVolume(float v1, float v2)
    {
        TargetVolume = v1;
        LerpRate = v2;
    }
}

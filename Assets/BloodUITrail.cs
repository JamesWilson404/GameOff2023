using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodUITrail : MonoBehaviour
{

    public Vector3 Start;
    public Vector3 End;
    public Vector3 Half;

    public AnimationCurve VelocityCurve;

    public float t = 0;
    public bool travelling = false;

    public void StartPath(Vector3 start, Vector3 end)
    {
        Start = start;
        End = end;
        t = 0;
        transform.position = Start;
        gameObject.SetActive(true);
        travelling = true;
    }

    private void Update()
    {
        if (travelling)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, 1);
            transform.position = CalculateLocation();

            if (t == 1)
            {
                FinishPath();
            }
        }
    }

    private void FinishPath()
    {
        travelling = false;
        gameObject.SetActive(false);
        Game.Instance.AwardResource(eCardPolarity.Blood, BoardManager.Instance.BloodTiles.Count);
    }

    private Vector3 CalculateLocation()
    {
        var realT = VelocityCurve.Evaluate(t);


        var one = Vector3.Lerp(Start, Start + Half, realT);
        var two = Vector3.Lerp(Start + Half, End, realT);
        var three = Vector3.Lerp(one, two, realT);



        return three;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Start, 0.5f);
        Gizmos.DrawSphere(End, 0.5f);
    }
}

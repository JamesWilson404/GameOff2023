using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeManager : MonoBehaviour
{
    public Animator animator;

    float heldTime = 0;
    bool start = false;

    // Update is called once per frame
    void Update()
    {
        if (Game.Instance.TurnState == Game.eTurnState.Preamble)
        {






            if (!start && Input.anyKey)
            {
                start = true;
                VoiceManager.Instance.PlaySound(VoiceFX.INTRO1);
            }

            if (start)
            {
                heldTime += Time.deltaTime;
            }

            if (heldTime > 3f)
            {
                CloseEye();
            }
        
        }



    }


    public void CloseEye()
    {
        animator.SetTrigger("CloseEye");
        Game.Instance.StartGame();
    }

}

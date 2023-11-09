using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryTitle : MonoBehaviour
{

    [SerializeField] TMP_Text TitleText;
    int Progress = 0;
    string TextToSet;
    
    public void SetTitle(string title)
    {
        TextToSet = title;
        TitleText.text = "";
        TitleText.color = Color.white;
        Progress = 0;
        gameObject.SetActive(true);
        StartCoroutine(SetLetters());
    }

    public IEnumerator SetLetters()
    {

        while (Progress < TextToSet.Length)
        {
            yield return new WaitForSeconds(0.2f);
            AudioManager.Instance.PlaySound(SoundFX.WRITING);
            TitleText.text += TextToSet[Progress];
            Progress++;
        }

        FadeOut();
        yield break;
    }

    public void FadeOut()
    {
        GetComponent<Animator>().Play("StoryTitleFadeOut");
    }

}

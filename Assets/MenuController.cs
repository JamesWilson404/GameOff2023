using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject BloodParent;
    public GameObject BloodPrefab;

    public int numberOfTrails = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine( SpawnBlood() );
    }

    private IEnumerator SpawnBlood()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < numberOfTrails; i++)
        {
            var newBloodTrail = Instantiate(BloodPrefab, BloodParent.transform.position, Quaternion.identity, BloodParent.transform);
            newBloodTrail.GetComponent<BloodTrail>().Init();
        }
        AudioManager.Instance.PlaySound(SoundFX.BELL);
    }

    public void PlayPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

}

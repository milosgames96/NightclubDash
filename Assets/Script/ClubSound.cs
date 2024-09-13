using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubSound : MonoBehaviour
{
    [SerializeField] private AudioSource clubMusicSource;
    [SerializeField] private AudioClip sound1;
    [SerializeField] private AudioClip sound2;
    [SerializeField] private AudioClip sound3;
    [SerializeField] private AudioClip sound4;
    [SerializeField] private AudioClip sound5;
    [SerializeField] private AudioClip sound6;
    [SerializeField] private GirlScript girlScript;
    [SerializeField] private GameObject girl;
    private int enrageStack;
    public bool IsPlaying { get; set; }

    void Start()
    {
        IsPlaying = false;
    }

    void Update()
    {
        enrageStack = girlScript.EnrageStack;

        if (!IsPlaying && girl.activeSelf)
        {
            if (enrageStack <= 0)
                StartCoroutine(PlaySoundCorutine(sound1));
            else if (enrageStack == 1)
                StartCoroutine(PlaySoundCorutine(sound2));
            else if (enrageStack == 2)
                StartCoroutine(PlaySoundCorutine(sound3));
            else if (enrageStack == 3)
                StartCoroutine(PlaySoundCorutine(sound4));
            else if (enrageStack == 4)
                StartCoroutine(PlaySoundCorutine(sound5));
            else
                StartCoroutine(PlaySoundCorutine(sound6));
        }
    }
    
    IEnumerator PlaySoundCorutine(AudioClip clip)
    {
        IsPlaying = true;

        clubMusicSource.Stop();
        clubMusicSource.clip = clip;
        clubMusicSource.Play();

        yield return new WaitForSeconds(girlScript.requiredEnrageTime);

        IsPlaying = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlYellSound : MonoBehaviour
{
    AudioSource yellSoundSource;

    void Start()
    {
        yellSoundSource = GetComponent<AudioSource>();
    }

    //called from animation
    private void PlayYellSound()
    {
        yellSoundSource.PlayOneShot(yellSoundSource.clip);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager :MonoBehaviour {
    public void PlaySE(AudioClip clip){
        source.PlayOneShot(clip);
    }


    [SerializeField] AudioSource source;

    public static AudioManager Get() => GameObject.FindGameObjectsWithTag("Singletons").Single()
        .GetComponent<AudioManager>();
}

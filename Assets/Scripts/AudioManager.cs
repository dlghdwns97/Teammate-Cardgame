using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip bgmusic;
    public AudioSource audioSource;
    public AudioClip start;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.PlayOneShot(start);
        audioSource.clip = bgmusic;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

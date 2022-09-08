using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip bounceSound, blockDestroySound, ballReturnSound, specialHitSound;
    public static AudioSource audioSrc;
    private void Awake()
    {
        bounceSound = Resources.Load<AudioClip>("bounce");
        blockDestroySound = Resources.Load<AudioClip>("blockDestroy");
        ballReturnSound = Resources.Load<AudioClip>("ballReturn");
        specialHitSound = Resources.Load<AudioClip>("specialHit");
        audioSrc = GetComponent<AudioSource>();
    }
    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "bounce":
                audioSrc.PlayOneShot(bounceSound);
                break;
            case "blockDestroy":
                audioSrc.PlayOneShot(blockDestroySound);
                break;
            case "ballReturn":
                audioSrc.PlayOneShot(ballReturnSound);
                break;
            case "specialHit":
                audioSrc.PlayOneShot(specialHitSound);
                break;
        }
    }
}

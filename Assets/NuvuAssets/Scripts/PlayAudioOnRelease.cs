using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnRelease : HitTarget
{
    public AudioSource releaseAudioSource;
    public int playOnlyOnRelease = 0;

    int numReleases = 0;

    void Start()
    {
        if (releaseAudioSource == null)
            releaseAudioSource = GetComponent<AudioSource>();
    }

    bool ShouldPlay()
    {
        return playOnlyOnRelease == 0 || numReleases == playOnlyOnRelease;
    }

    override public void Release(TouchHitInfo hitInfo)
    {
        numReleases++;

        if (releaseAudioSource != null && ShouldPlay())
            releaseAudioSource.Play();
    }
}

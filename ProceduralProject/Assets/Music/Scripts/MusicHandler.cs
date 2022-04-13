using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicHandler : MonoBehaviour
{
    public AudioClip[] playlist;

    private AudioSource player;

    private int currentTrack;

    private void Start()
    {
        player = GetComponent<AudioSource>();
        PlayTrackRand();
    }

    public void PlayTrack(int t)
    {
        // Make sure the value falls into the bounds of the array
        if (t < 0 || t >= playlist.Length) return;

        // play the track at the array number passed in
        player.PlayOneShot(playlist[t]);

        currentTrack = t;
    }

    public void PlayTrackNext()
    {
        int track = currentTrack + 1;
        if (currentTrack >= playlist.Length) track = 0;
        PlayTrack(track);
    }

    public void PlayTrackRand()
    {
        PlayTrack(Random.Range(0, playlist.Length));
    }

    private void Update()
    {
        if (!player.isPlaying)
        {
            PlayTrackNext();
        }
    }
}

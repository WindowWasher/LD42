using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    public AudioSource source;

    public float timeDelayBetweenPlays = 0.5f;

    private float timeSincePlayed;

	// Use this for initialization
	void Start () {

        timeSincePlayed = Time.time;
	}

    public void PlaySound()
    {
        if (source && !source.isPlaying && timeSincePlayed + timeDelayBetweenPlays < Time.time)
        {
            source.pitch = Random.Range(0.8f, 1.35f);
            source.Play();
            timeSincePlayed = Time.time;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    public AudioSource source;

    public float timeDelayBetweenPlays = 1;

    private float timeSincePlayed;

	// Use this for initialization
	void Start () {

        timeSincePlayed = Time.time;
	}

    public void PlaySound()
    {
        if (!source.isPlaying && timeSincePlayed + timeDelayBetweenPlays > Time.time)
        {
            source.Play();
            timeSincePlayed = Time.time;
        }
    }
}

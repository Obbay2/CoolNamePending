using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour {

    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public int crossFadeTime = 5;

    public static int numberOfClips = 0;
    [SerializeField] private AudioClip[] audioClips = new AudioClip[numberOfClips];

    private int playingIndex;
    private AudioSource playingSource;
    private AudioSource waitingSource;
    private bool changing = false;

	// Use this for initialization
	void Start () {
        playingIndex = 0;
        audioSource1.clip = audioClips[playingIndex];
        audioSource1.volume = 0.25f;
        audioSource2.volume = 0.0f;
        playingSource = audioSource1;
        waitingSource = audioSource2;
        audioSource1.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        print(playingSource.time + "s" + " " + waitingSource.time + "s" + " " + changing + " " + playingSource.volume + " " + waitingSource.volume + " " + playingIndex);
        if (audioClips[playingIndex % audioClips.Length].length - playingSource.time < crossFadeTime && !changing)
        {    
            changing = true;
            playingIndex++;
            AudioSource temp = playingSource;
            playingSource = waitingSource;
            waitingSource = temp;

            playingSource.clip = audioClips[playingIndex % audioClips.Length];
            playingSource.Play();
            print("Starting: " + playingSource.clip.name + " on " + playingSource.name);
            
        }
        else
        {
            if (changing)
            {
                playingSource.volume = playingSource.volume += (0.25f / crossFadeTime) * Time.fixedDeltaTime;
                waitingSource.volume = waitingSource.volume -= (0.25f / crossFadeTime) * Time.fixedDeltaTime;
            }

            if (playingSource.volume > 0.25f - 0.025f)
            {
                playingSource.volume = 0.25f;
            }            

            if (waitingSource.clip != null && waitingSource.volume < 0.025f && waitingSource.isPlaying)
            {
                print("Stopping: " + waitingSource.clip.name + " on " + waitingSource.name);
                waitingSource.volume = 0;
                changing = false;
                waitingSource.Stop();
            }            
        }
    }
}

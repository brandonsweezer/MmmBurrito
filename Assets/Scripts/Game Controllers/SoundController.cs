using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour {

//    public float MasterVolume;
//    public float SoundEffectVolume;

	public Slider MasterVolume;
	public Slider SoundEffectVolume;

    public AudioClip pickup;
    public AudioClip fall;
    public AudioClip dash;
    public AudioClip dashAlt;
    public AudioClip bump;
    public AudioClip trash;
    public AudioClip death;
    public AudioClip mmmHi;
    public AudioClip mmmLo;
    public AudioClip mmmMed;
	public AudioClip dingding;
	public AudioClip lose;
    public AudioClip wrongSubmission;
    public AudioClip music;
    public AudioClip musicUrgent;
    public AudioClip musicExtraUrgent;
    public AudioClip ticking;
    public AudioClip urgentTicking;
    public AudioClip invincible;
    public AudioClip orderUp;
    public AudioClip spring;

	public AudioSource audSrc;
	public AudioSource audSrcMusic;

	// for playing music at different volumes
	public MusicUrgency musicUrgency;
	public enum MusicUrgency {
		Regular,
		Urgent,
		ExtraUrgent
	};

    public static SoundController instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
		musicUrgency = MusicUrgency.Regular;
		MasterVolume.value = 1f;
		SoundEffectVolume.value = 1f;
        pickup = (AudioClip) Resources.Load("Sound/pickup");
        fall = (AudioClip) Resources.Load("Sound/fall");
        bump = (AudioClip) Resources.Load("Sound/bump");
        trash = (AudioClip) Resources.Load("Sound/trash");
        dash = (AudioClip)Resources.Load("Sound/dash");
        death = (AudioClip)Resources.Load("Sound/death");
        mmmHi = (AudioClip)Resources.Load("Sound/mmm_Hi");
        mmmLo = (AudioClip)Resources.Load("Sound/mmm_Lo");
		mmmMed = (AudioClip)Resources.Load("Sound/mmm_Med");
		dingding = (AudioClip)Resources.Load("Sound/submit(right)");
		lose = (AudioClip)Resources.Load("Sound/lose");
        wrongSubmission = (AudioClip)Resources.Load("Sound/submit(wrong)");
        orderUp = (AudioClip)Resources.Load("Sound/orderup");
        music = (AudioClip)Resources.Load("Sound/music");
        musicUrgent = (AudioClip)Resources.Load("Sound/musicUrgent");
        musicExtraUrgent = (AudioClip)Resources.Load("Sound/musicExtraUrgent");
        ticking = (AudioClip)Resources.Load("Sound/30 sec");
        urgentTicking = (AudioClip)Resources.Load("Sound/10 sec");
        invincible = (AudioClip)Resources.Load("Sound/invincible");
        spring = (AudioClip)Resources.Load("Sound/dash2");

        //audSrc = gameObject.GetComponent<AudioSource>();

        audSrcMusic.clip = SoundController.instance.music;
		audSrcMusic.loop = true;
		audSrcMusic.Play();
    }

	public void ChangeMasterValue(float value)
	{
		MasterVolume.value=value;
		audSrcMusic.volume = value;
	}

	public void MuteMaster ()
	{
		if (MasterVolume.value > 0) {
			MasterVolume.value = 0;
			audSrcMusic.volume = 0;
		} else {
			MasterVolume.value = 1;
			audSrcMusic.volume = 1;
		}
	}

	public void ChangeFXValue(float value)
	{
		SoundEffectVolume.value=value;
		audSrc.volume = value;
	}

	public void MuteFX ()
	{
		if (SoundEffectVolume.value > 0) {
			SoundEffectVolume.value = 0;
			audSrc.volume = 0;
		} else {
			SoundEffectVolume.value = 1;
			audSrc.volume = 1;
		}
	}

	public void PlayMusic() {
		SoundController.instance.audSrcMusic.Stop();
		UpdateMusicClipUrgency ();
		SoundController.instance.audSrcMusic.Play();
	}

	public void SetMusicUrgency(MusicUrgency urgency) {
		musicUrgency = urgency;
	}

	private void UpdateMusicClipUrgency() {
		switch (musicUrgency) {
		case MusicUrgency.Regular:
			SoundController.instance.audSrcMusic.clip = SoundController.instance.music;
			break;
		case MusicUrgency.Urgent:
			SoundController.instance.audSrcMusic.clip = SoundController.instance.musicUrgent;
			break;
		case MusicUrgency.ExtraUrgent:
			SoundController.instance.audSrcMusic.clip = SoundController.instance.musicExtraUrgent;
			break;
		}
	}

	public void ChangePlayedMusicUrgencyIfNecessary(MusicUrgency urgency) {
		if (musicUrgency != urgency) {
			// play ticking noise
			if (urgency == MusicUrgency.Urgent) {
				audSrc.PlayOneShot(ticking,SoundEffectVolume.value);
			} else if (urgency == MusicUrgency.ExtraUrgent) {
				audSrc.PlayOneShot(urgentTicking,SoundEffectVolume.value);
			}
			// update music
			SetMusicUrgency (urgency);
			PlayMusic ();
		}
	}



	private void SetMusicBasedOnTime(float time) {
		if (GameController.instance.gamestate != GameController.GameState.Play && GameController.instance.gamestate != GameController.GameState.Pause) {
			SoundController.instance.ChangePlayedMusicUrgencyIfNecessary (SoundController.MusicUrgency.Regular);
			return;
		}

		if (time > 30 || time <= 0) {
			SoundController.instance.ChangePlayedMusicUrgencyIfNecessary (SoundController.MusicUrgency.Regular);
		} else if (time <= 30 && time > 10) {
			SoundController.instance.ChangePlayedMusicUrgencyIfNecessary (SoundController.MusicUrgency.Urgent);
		} else if (time <= 10) {
			SoundController.instance.ChangePlayedMusicUrgencyIfNecessary (SoundController.MusicUrgency.ExtraUrgent);
		}
	}


	// Update is called once per frame
	void Update () {
		SetMusicBasedOnTime (Timer.instance.getTime());
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public float MasterVolume;
    public float SoundEffectVolume;

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
    public AudioClip wrongSubmission;
    public AudioClip music;
    public AudioClip musicUrgent;
    public AudioClip musicExtraUrgent;
    public AudioClip ticking;
    public AudioClip urgentTicking;
    public AudioClip invincible;
    public AudioClip orderUp;

    public AudioSource audSrc;

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
        wrongSubmission = (AudioClip)Resources.Load("Sound/submit(wrong)");
        orderUp = (AudioClip)Resources.Load("Sound/orderup");
        music = (AudioClip)Resources.Load("Sound/music");
        musicUrgent = (AudioClip)Resources.Load("Sound/musicUrgent");
        musicExtraUrgent = (AudioClip)Resources.Load("Sound/musicExtraUrgent");
        ticking = (AudioClip)Resources.Load("Sound/30 sec");
        urgentTicking = (AudioClip)Resources.Load("Sound/10 sec");
        invincible = (AudioClip)Resources.Load("Sound/invincible");

        audSrc = gameObject.GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer theMixer;

    public static AudioClip nextLevel, bowRelease, bowDraw, arrowShoosh, balloonBurst, loseHP, balloonBonusItem;
    public static AudioClip mainMenu, inGame;

    public AudioSource BowSFXSource;
    public AudioSource BalloonSFXSource;
    public AudioSource BGMSource;
    public AudioSource UISFXSource;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    public static AudioManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            theMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            theMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            theMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
        }

        bowRelease = Resources.Load<AudioClip>("Sounds/SFX/BowRelease");
        bowDraw = Resources.Load<AudioClip>("Sounds/SFX/BowDraw");
        arrowShoosh = Resources.Load<AudioClip>("Sounds/SFX/ArrowShoosh");
        balloonBurst = Resources.Load<AudioClip>("Sounds/SFX/BalloonBurst");
        balloonBonusItem = Resources.Load<AudioClip>("Sounds/SFX/BonusSound");
        nextLevel = Resources.Load<AudioClip>("Sounds/SFX/NextLevel");
        loseHP = Resources.Load<AudioClip>("Sounds/SFX/LoseHP");

        mainMenu = Resources.Load<AudioClip>("Sounds/Music/MainMenu");
        inGame = Resources.Load<AudioClip>("Sounds/Music/InGame");

        BGMSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void bowPlaySFX(string clip)
    {
        switch (clip)
        {
            case "BalloonBurst":
                BowSFXSource.clip = balloonBurst;
                break;
            case "BowRelease":
                BowSFXSource.clip = bowRelease;
                break;
            case "BowDraw":
                BowSFXSource.clip = bowDraw;
                break;
            case "ArrowShoosh":
                BowSFXSource.clip = arrowShoosh;
                break;
        }
        BowSFXSource.Play();

    }

    public void balloonPlaySFX(string clip)
    {
        switch (clip)
        {
            case "BalloonBurst":
                BalloonSFXSource.clip = balloonBurst;
                break;
            case "BalloonBonusItem":
                BalloonSFXSource.clip = balloonBonusItem;
                break;
        }
        BalloonSFXSource.Play();

    }

    public void UIPlaySFX(string clip)
    {
        switch (clip)
        {
            case "LoseHP":
                UISFXSource.clip = loseHP;
                break;
            case "EndGame":
                UISFXSource.clip = arrowShoosh;
                break;
            case "NextLevel":
                UISFXSource.clip = loseHP;
                break;
        }
        UISFXSource.Play();

    }

    public void playMusic(string clip)
    {
        BGMSource.Stop();
        switch (clip)
        {
            case "MainMenu":
                BGMSource.clip = mainMenu;
                break;
            case "InGame":
                BGMSource.clip = inGame;
                break;
        }
        BGMSource.Play();
    }
}

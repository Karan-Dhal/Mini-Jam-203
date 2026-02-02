using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource uiTimeSource;
    [SerializeField] private AudioSource rewardSource;
    [SerializeField] private AudioSource environmentalSource;
    [SerializeField] private AudioSource actionSource;
    [SerializeField] private AudioSource footstepSource;

    [Header("Music Tracks")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusicStart;
    public AudioClip gameplayMusicToLoop;
    public AudioClip deathMusic;

    [Header("UI & Time Sounds")]
    public AudioClip pauseSFX;
    public AudioClip unpauseSFX;
    public AudioClip slowmoSFX;
    public AudioClip fastSFX;

    [Header("Reward Sounds")]
    public AudioClip collectibleSFX;
    public AudioClip powerupSFX;

    [Header("Environmental Sounds")]
    public AudioClip crushingWallsSFX;

    [Header("Action Sounds")]
    public AudioClip jumpSFX;
    public AudioClip leverSFX;
    public AudioClip punchSFX;
    public AudioClip reverseSFX;

    [Header("Footsteps")]
    public AudioClip[] footstepSounds = new AudioClip[6];
    
    private Coroutine footstepCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMenuMusic() => PlayMusic(menuMusic);
    public void PlayGameplayMusicStart() => uiTimeSource.PlayOneShot(gameplayMusicStart);
    public void PlayGameplayMusicLoop() => PlayMusic(gameplayMusicToLoop);
    public void PlayGameplayMusic() => StartCoroutine(PlayGamePlayMusicInOrder());
    public void PlayDeathMusic() => PlayMusic(deathMusic);

    public void PlayPause() => uiTimeSource.PlayOneShot(pauseSFX);
    public void PlayUnpause() => uiTimeSource.PlayOneShot(unpauseSFX);
    public void PlaySlowmo() => uiTimeSource.PlayOneShot(slowmoSFX);
    public void PlayFast() => uiTimeSource.PlayOneShot(fastSFX);

    public void PlayCollectible() => rewardSource.PlayOneShot(collectibleSFX);
    public void PlayPowerup() => rewardSource.PlayOneShot(powerupSFX);

    public void PlayCrushingWalls() => environmentalSource.PlayOneShot(crushingWallsSFX);

    public void PlayJump() => actionSource.PlayOneShot(jumpSFX);
    public void PlayLever() => actionSource.PlayOneShot(leverSFX);
    public void PlayPunch() => actionSource.PlayOneShot(punchSFX);
    public void PlayReverse() => actionSource.PlayOneShot(reverseSFX);
    public void PlayExample() => Debug.Log("Example sound called.");

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StartFootsteps()
    {
        if (footstepCoroutine == null)
        {
            footstepCoroutine = StartCoroutine(FootstepRoutine());
        }
    }

    public void StopFootsteps()
    {
        if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
            footstepSource.Stop();
        }
    }

    private IEnumerator FootstepRoutine()
    {
        while (true)
        {
            if (footstepSounds.Length > 0)
            {
                AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
                footstepSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length);
            }
            else
            {
                yield return null; 
            }
        }
    }

    private IEnumerator PlayGamePlayMusicInOrder()
    {
        PlayGameplayMusicStart();
        yield return new WaitForSeconds(gameplayMusicStart.length);
        PlayGameplayMusicLoop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        uiTimeSource.volume = volume;
        rewardSource.volume = volume;
        environmentalSource.volume = volume;
        actionSource.volume = volume;
        footstepSource.volume = volume;
    }
}
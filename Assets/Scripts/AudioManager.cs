using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<AudioManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip mainThemeBGM;

    [Header("SFX Clips")]
    public AudioClip shovelSFX;
    public AudioClip waterSFX;
    public AudioClip plantSFX;
    public List<AudioClip> plantSFXVariations = new List<AudioClip>();
    public AudioClip purifySFX;
    public AudioClip dialogueSFX;
    public AudioClip questCompleteSFX;
    public AudioClip footstepDirtSFX;
    public AudioClip footstepGrassSFX;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Auto-configure AudioSources
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            bgmSource.volume = 0.4f;
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.volume = 0.6f;
        }

        #if UNITY_EDITOR
        AutoLoadClips();
        #endif
    }

    private void Start()
    {
        if (mainThemeBGM != null)
        {
            PlayBGM(mainThemeBGM);
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        AutoLoadClips();
    }

    private void AutoLoadClips()
    {
        string audioDir = "Assets/Audio";
        if (!System.IO.Directory.Exists(audioDir)) return;

        if (mainThemeBGM == null) mainThemeBGM = LoadClip("bgm");
        if (shovelSFX == null) shovelSFX = LoadClip("shovel");
        if (waterSFX == null) waterSFX = LoadClip("splash");
        if (plantSFX == null) plantSFX = LoadClip("plant");
        if (purifySFX == null) purifySFX = LoadClip("purify");
        if (dialogueSFX == null) dialogueSFX = LoadClip("typing");
        if (questCompleteSFX == null) questCompleteSFX = LoadClip("victory");
        if (footstepDirtSFX == null) footstepDirtSFX = LoadClip("dirt_footstep");
        if (footstepGrassSFX == null) footstepGrassSFX = LoadClip("footstep_grass");

        // Autoload plant_0 to plant_9 variations
        plantSFXVariations.Clear();
        for (int idx = 0; idx < 10; idx++)
        {
            AudioClip clip = LoadClip($"plant_{idx}");
            if (clip != null)
            {
                plantSFXVariations.Add(clip);
            }
        }
    }

    private AudioClip LoadClip(string name)
    {
        string[] extensions = new string[] { ".wav", ".mp3", ".ogg" };
        foreach (var ext in extensions)
        {
            string path = $"Assets/Audio/{name}{ext}";
            if (System.IO.File.Exists(path))
            {
                var clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                if (clip != null) return clip;
            }
        }
        return null;
    }
    #endif

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1.0f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volumeScale);
    }

    // Play a specific time segment/slice of an audio clip in Unity
    public void PlaySFXSlice(AudioClip clip, float startTime, float endTime, float volumeScale = 1.0f)
    {
        if (clip == null) return;
        StartCoroutine(PlayClipSliceCoroutine(clip, startTime, endTime, volumeScale));
    }

    private System.Collections.IEnumerator PlayClipSliceCoroutine(AudioClip clip, float startTime, float endTime, float volume)
    {
        GameObject tempGo = new GameObject("TempSFXSource");
        AudioSource source = tempGo.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume * (sfxSource != null ? sfxSource.volume : 0.6f);
        source.loop = false;
        source.playOnAwake = false;

        // Start playback at specified time
        source.time = Mathf.Clamp(startTime, 0f, clip.length - 0.01f);
        source.Play();

        float playDuration = endTime - startTime;
        if (playDuration <= 0f) playDuration = clip.length - startTime;

        yield return new WaitForSeconds(playDuration);

        if (source != null)
        {
            source.Stop();
        }
        Destroy(tempGo);
    }

    // --- Helper shortcuts with custom duration-cuts ---
    public void PlayShovel() => PlaySFX(shovelSFX, 1.0f);
    public void PlayWater() => PlaySFXSlice(waterSFX, 0f, 1.5f, 0.9f); // Cut water splash to 0.0s - 1.5s
    
    public void PlayPlant()
    {
        if (plantSFXVariations != null && plantSFXVariations.Count > 0)
        {
            int randIndex = Random.Range(0, plantSFXVariations.Count);
            PlaySFX(plantSFXVariations[randIndex], 0.8f);
        }
        else if (plantSFX != null)
        {
            PlaySFXSlice(plantSFX, 3.0f, 4.0f, 0.8f); // Cut plant SFX to 3.0s - 4.0s
        }
    }

    public void PlayPurify() => PlaySFXSlice(purifySFX, 0f, 1.25f, 1.0f); // Cut purify chime to 0.0s - 1.25s
    public void PlayDialogue() => PlaySFX(dialogueSFX, 0.6f);
    public void PlayQuestComplete() => PlaySFX(questCompleteSFX, 1.0f);

    public void PlayFootstep(bool isOnGrass)
    {
        if (isOnGrass)
        {
            if (footstepGrassSFX != null) PlaySFX(footstepGrassSFX, 0.35f);
        }
        else
        {
            if (footstepDirtSFX != null) PlaySFX(footstepDirtSFX, 0.35f);
        }
    }
}

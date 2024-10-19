using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExampleRandomSoundPlayer : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private bool playOnAwake = false;

    [SerializeField] private bool playAsOneShot = false;
    [SerializeField] private bool playOnInterval = false;
    [SerializeField] private bool preventRepeat = true;
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float maxInterval = 7f;
    [SerializeField][Range(0f, 1f)] private float volume = 1f;
    [SerializeField] private bool randomizePitch = false;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private float minDelay = 1f;
    [SerializeField] private float maxDelay = 3f;

    [Header("3D Sound Settings")]
    [SerializeField][Range(0f, 1f)] private float spatialBlend = 0f;
    [SerializeField] private float maxDistance = 20f;

    private AudioSource audioSource;
    private float timer;
    private int lastPlayedIndex = -1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = spatialBlend;
        audioSource.maxDistance = maxDistance;
        audioSource.volume = volume;

        if (playOnAwake)
        {
            PlayRandomSound();
        }
    }

    private void Start()
    {
        timer = Random.Range(minInterval, maxInterval);
    }

    private void Update()
    {
        if (playOnInterval && minInterval > 0f && maxInterval > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                PlayRandomSound();
                timer = Random.Range(minInterval, maxInterval);
            }
        }
    }

    public void PlayRandomSound()
    {
        if (sounds.Length == 0)
        {
            Debug.LogWarning("No sounds assigned to RandomSoundPlayer.");
            return;
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, sounds.Length);
        } while (randomIndex == lastPlayedIndex && preventRepeat && sounds.Length > 1);

        lastPlayedIndex = randomIndex;

        if (!playAsOneShot)
        {

            audioSource.clip = sounds[randomIndex];

            if (randomizePitch)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }
            else
            {
                audioSource.pitch = 1f;
            }

            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(sounds[randomIndex], volume);

        }
    }

    public void PlayRandomSoundWithDelay()
    {
        if (sounds.Length == 0)
        {
            Debug.LogWarning("No sounds assigned to RandomSoundPlayer.");
            return;
        }

        float delay = Random.Range(minDelay, maxDelay);
        Invoke(nameof(PlayRandomSound), delay);
    }

    public void PlayOnTriggerEnter(Collider other)
    {
        PlayRandomSound();
    }
}

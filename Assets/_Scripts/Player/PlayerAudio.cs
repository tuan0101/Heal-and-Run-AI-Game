using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] songs;
    public GameObject singRange;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayASong()
    {
        audioSource.clip = songs[Random.Range(0, songs.Length)];
        audioSource.Play();
    }

    public void StopPlaySong()
    {
            audioSource.Stop();
    }

    public void DisableSingRange()
    {
        singRange.SetActive(false);
    }

    public void EnableSingRange()
    {
        singRange.SetActive(true);
    }
}

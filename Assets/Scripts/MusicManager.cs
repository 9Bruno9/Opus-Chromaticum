
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] AudioSource Soundtrack;
    [SerializeField] AudioSource Ambient;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public AudioClip soundtrack;
    public AudioClip ambient;
    public AudioClip bossclip;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
     
    }



    private void Start()
    {
        Soundtrack.clip = soundtrack;
        Soundtrack.Play();
        Ambient.clip = ambient;
        Ambient.Play();
    }

    public void changeSoundtrack(AudioClip newclip)
    {
        Soundtrack.clip = newclip;
        Soundtrack.Play();
    }

    public void riproduceSoundEffect(AudioClip sound)
    {
        Ambient.clip = sound;
        Ambient.Play();
    }


    /*
    public void PlaySoundEffect(AudioClip clip)
    {
        mbient.clip = clip;
        Ambient.Play();
    }*/
}

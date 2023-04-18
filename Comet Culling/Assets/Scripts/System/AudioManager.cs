// Control all audio, music, sfx etc across all scenes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Seperate components for sfx and music
    [SerializeField] AudioSource SfxSource;
    [SerializeField] AudioSource MusicSource;

    [SerializeField] AudioClip footsteps;
    [SerializeField] AudioClip tillingSoil;
    [SerializeField] AudioClip plantSeed;
    [SerializeField] AudioClip harvestCrop;
    [SerializeField] AudioClip waterCrop;
    [SerializeField] AudioClip generatorFeedCrops;
    [SerializeField] AudioClip generatorRecharge;

    // Create an instance of the class to allow to call its functions statically
    public static AudioManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the instance
        Instance = this;
    }

    public void PlayMusic()
    {
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    public void playFootsteps()
    {
        SfxSource.PlayOneShot(footsteps, 0.5f);
    }

    public void playTillingSoil()
    {
        SfxSource.PlayOneShot(tillingSoil, 0.5f);
    }

    public void playPlantSeed()
    {
        SfxSource.PlayOneShot(plantSeed, 0.5f);
    }

    public void playHarvestCrop()
    {
        SfxSource.PlayOneShot(harvestCrop, 0.5f);
    }

    public void playWaterCrop()
    {
        SfxSource.PlayOneShot(waterCrop, 0.5f);
    }
    public void playGeneratorFeedCrops()
    {
        SfxSource.PlayOneShot(generatorFeedCrops, 0.5f);
    }
    public void playGeneratorRecharge()
    {
        SfxSource.PlayOneShot(generatorRecharge, 0.5f);
    }
}
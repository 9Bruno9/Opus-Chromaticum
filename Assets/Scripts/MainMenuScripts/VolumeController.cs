using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MainMenuScripts
{
    public class VolumeController : MonoBehaviour
    {
        
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private AudioMixer mainMixer;

        private void Start()
        {
            if (!mainMixer.GetFloat("MusicVol", out _))
            {
                Debug.LogWarning("No exposed float \"MusicVol\" found");
            }
            if (!mainMixer.GetFloat("SFXVol", out _))
            {
                Debug.LogWarning("No exposed float \"SFXVol\" found");
            }

            if (!GameManager.instance.IsUnityNull())
            {
                musicSlider.value = GameManager.instance.musicVolume;
                sfxSlider.value = GameManager.instance.sfxVolume;
                SetMusicVolume(GameManager.instance.musicVolume);
                SetSfxVolume(GameManager.instance.sfxVolume);
            }
            else
            {
                musicSlider.value = .5f;
                sfxSlider.value = .5f;
                SetMusicVolume(.5f);
                SetSfxVolume(.5f);
            }
            
        }

        public void SetMusicVolume(float percent)
        {
            if (percent < 0.001)
            {
                mainMixer.SetFloat("MusicVol", -80);
            }
            else
            {
                mainMixer.SetFloat("MusicVol", Mathf.Log10(percent) * 20);
            }
            GameManager.instance.musicVolume = percent;
        }
    
        public void SetSfxVolume(float percent)
        {
            if (percent < 0.001)
            {
                mainMixer.SetFloat("SFXVol", -80);
            }
            else
            {
                mainMixer.SetFloat("SFXVol", Mathf.Log10(percent) * 20);
            }
            GameManager.instance.sfxVolume = percent;
        }
    }
}

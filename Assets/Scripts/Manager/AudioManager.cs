using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Manager {
    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance {get; private set; }

        [SerializeField] private AudioMixer mixers;

        public enum MixerGroups {
            MasterVolume,
            MusicVolume,
            SfxVolume
        }
        
        private void Awake() {
            if (Instance is not null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }
    
        private void OnDestroy() {
            if (Instance == this) Instance = null;
        }

        public void SetVolume(MixerGroups group, float value) {
            mixers.SetFloat(group.ToString(), Mathf.Log10(value) * 20);
        }

        public float SetSlider(MixerGroups group) {
            mixers.GetFloat(group.ToString(), out float value);
            return (float) Math.Pow(10, value / 20);
        }
    }
}
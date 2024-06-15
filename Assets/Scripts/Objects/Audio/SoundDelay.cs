using System;
using System.Collections;
using UnityEngine;

namespace Objects.Audio {
    [RequireComponent(typeof(AudioSource))]
    public class SoundDelay : MonoBehaviour {
        [SerializeField] private float delay;
        private AudioSource _source;
        
        private void Awake() {
            StartCoroutine(PlaySoundWithDelay());
        }

        private IEnumerator PlaySoundWithDelay() {
            _source.Play();
            yield return new WaitForSeconds(delay);
            StartCoroutine(PlaySoundWithDelay());
        }
    }
}
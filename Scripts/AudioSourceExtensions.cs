using System;
using System.Collections;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace UnityExtensions
{
    public static class AudioSourceExtensions
    {
        public static void FadeIn(this AudioSource self, float volume, float duration)
        {
            if (volume < 0.0f) volume = 0.0f;
            else if (volume > 1.0f) volume = 1.0f;
            DOTween.To(() => self.volume, x => self.volume = x, volume, duration).SetTarget(self);
        }
    
        public static IEnumerator FadeInAsCoroutine(this AudioSource self, float volume, float duration)
        {
            self.FadeIn(volume, duration);
            yield return new WaitForSeconds(duration);
        }
    
        public static void FadeOut(this AudioSource self, float duration)
        {
            DOTween.To(() => self.volume, x => self.volume = x, 0.0f, duration).SetTarget(self);
        }

        public static IEnumerator FadeOutAsCoroutine(this AudioSource self, float duration)
        {
            self.FadeOut(duration);
            yield return new WaitForSeconds(duration);
        }
    
        public static bool PlayWithFadeIn(this AudioSource self, AudioClip clip, float volume, float duration)
        {
            if (clip == null || volume <= 0.0f)
                return false;

            self.clip = clip;
            self.volume = 0.0f;
            self.Play();
            self.FadeIn(volume, duration);
            return true;
        }
        
        public static IObservable<Unit> PlayWithFadeInAsObservable(this AudioSource self, AudioClip clip, float volume, float duration)
        {
            if (self.PlayWithFadeIn(clip, volume, duration))
                return Observable.Timer(TimeSpan.FromSeconds(clip.length)).AsUnitObservable();
            else
                return Observable.Throw<Unit>(new ArgumentException());
        }
    }
}

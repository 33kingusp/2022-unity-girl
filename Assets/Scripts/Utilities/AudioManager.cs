using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Utilities
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { private set; get; }

        [SerializeField] private AudioSource _bgmAudioSource = default;
        [SerializeField] private AudioSource _seAudioSource = default;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void PlayBGM(AudioClip bgmClip, bool isLoop = true)
        {
            _bgmAudioSource.Stop();
            _bgmAudioSource.loop = isLoop;
            _bgmAudioSource.clip = bgmClip;
            _bgmAudioSource.Play();
        }

        public void StopBGM()
        {
            _bgmAudioSource.Stop();
        }

        public void PlaySE(AudioClip seClip)
        {
            _seAudioSource.PlayOneShot(seClip);
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="bgmClip">再生するBGM</param>
        /// <param name="fadeTime">フェードインする時間</param>
        /// <returns></returns>
        public IObservable<Unit> PlayBGMAsObservable(AudioClip bgmClip, float fadeTime = 0f, bool isLoop = true)
        {
            return Observable.FromCoroutine<Unit>(observer => PlayBGMCoroutine(observer, bgmClip, fadeTime, isLoop));
        }

        /// <summary>
        /// BGMを停止する
        /// </summary>
        /// <param name="fadeTime">フェードアウトする時間</param>
        /// <returns></returns>
        public IObservable<Unit> StopBGM(float fadeTime = 0f)
        {
            return Observable.FromCoroutine<Unit>(observer => StopBGMCoroutine(observer, fadeTime));
        }


        private IEnumerator PlayBGMCoroutine(IObserver<Unit> observer, AudioClip bgmClip, float fadeTime, bool isLoop = true)
        {
            float t = 0;

            _bgmAudioSource.volume = 0;
            _bgmAudioSource.loop = isLoop;
            _bgmAudioSource.clip = bgmClip;
            _bgmAudioSource.Play();

            while (t <= fadeTime)
            {
                _bgmAudioSource.volume = t / fadeTime;
                yield return null;
                t += Time.deltaTime;
            }

            _bgmAudioSource.volume = 1;

            observer.OnNext(Unit.Default);
            observer.OnCompleted();
        }

        private IEnumerator StopBGMCoroutine(IObserver<Unit> observer, float fadeTime)
        {
            float t = 0;

            _bgmAudioSource.volume = 1;

            while (t <= fadeTime)
            {
                _bgmAudioSource.volume = 1 - t / fadeTime;
                yield return null;
                t += Time.deltaTime;
            }

            _bgmAudioSource.Stop();
            _bgmAudioSource.volume = 1;

            observer.OnNext(Unit.Default);
            observer.OnCompleted();
        }
    }
}
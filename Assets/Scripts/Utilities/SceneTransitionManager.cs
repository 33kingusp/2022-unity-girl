using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;

namespace Utilities
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance { private set; get; } = default;

        [SerializeField] private CanvasGroup _transitionGroup = default;

        public IObservable<string> OnLoadedSceneAsObservable { get { return _onLoadedScene; } }
        private Subject<string> _onLoadedScene = default;

        public IObservable<string> OnFinishedFadeInAsObservable { get { return _onFinishedFadeIn; } }
        private Subject<string> _onFinishedFadeIn = default;

        public IObservable<string> OnFinishedFadeOutAsObservable { get { return _onFinishedFadeOut; } }
        private Subject<string> _onFinishedFadeOut = default;

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

        private void OnEnable()
        {
            _onLoadedScene = new Subject<string>();
            _onFinishedFadeIn = new Subject<string>();
            _onFinishedFadeOut = new Subject<string>();
        }

        private void OnDisable()
        {
            _onLoadedScene?.Dispose();
            _onFinishedFadeIn?.Dispose();
            _onFinishedFadeOut?.Dispose();
        }

        private void Start()
        {
            // デバッグ用
            _onLoadedScene.OnNext(SceneManager.GetActiveScene().name);
            _onFinishedFadeOut.OnNext(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// フェードイン・アウトと共にシーン遷移する
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="fadeTime"></param>
        public void LoadSceneWithFade(string sceneName, float fadeTime = 2.0f)
        {
            LoadSceneAsObservable(sceneName, fadeTime)
                .Subscribe()
                .AddTo(gameObject);
        }

        /// <summary>
        /// シーンを切り替える
        /// </summary>
        /// <param name="sceneName">読み込むシーン名</param>
        /// <param name="fadeTime">フェードイン・アウトにかかる時間</param>
        /// <returns></returns>
        private IObservable<Unit> LoadSceneAsObservable(string sceneName, float fadeTime = 2.0f)
        {
            return Observable.FromCoroutine<Unit>((observer) => LoadScneneCoroutine(observer, sceneName, fadeTime));
        }

        private IEnumerator LoadScneneCoroutine(IObserver<Unit> observer, string sceneName, float fadeTime)
        {
            yield return FadeIn(fadeTime);

            _onFinishedFadeIn.OnNext(sceneName);

            var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!async.isDone)
            {
                yield return null;
            }

            _onLoadedScene.OnNext(sceneName);

            yield return new WaitForSeconds(1f);                 

            yield return FadeOut(fadeTime);

            _onFinishedFadeOut.OnNext(sceneName);
            
            yield break;
        }

        /// <summary>
        /// トランジション用画像でフェードインする
        /// </summary>
        /// <param name="time">フェードにかかる時間</param>
        /// <returns></returns>
        private IEnumerator FadeIn(float time = 1.0f)
        {
            float t = 0;

            _transitionGroup.interactable = true;
            _transitionGroup.blocksRaycasts = true;

            while(t <= time)
            {
                _transitionGroup.alpha = t / time;

                yield return null;
                t += Time.deltaTime;
            }

            _transitionGroup.alpha = 1;
        }

        /// <summary>
        /// トランジション用画像でフェードアウトする
        /// </summary>
        /// <param name="time">フェードにかかる時間</param>
        /// <returns></returns>
        private IEnumerator FadeOut(float time = 1.0f)
        {
            float t = 0;

            _transitionGroup.interactable = true;
            _transitionGroup.blocksRaycasts = true;

            while (t <= time)
            {
                _transitionGroup.alpha = 1 - t / time;

                yield return null;
                t += Time.deltaTime;
            }

            _transitionGroup.alpha = 0;
            _transitionGroup.interactable = false;
            _transitionGroup.blocksRaycasts = false;
        }
    }
}

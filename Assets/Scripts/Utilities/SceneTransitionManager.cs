using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;

namespace Utilities
{
    // TODO:真面目にシングルトン化
    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance { private set; get; }

        [SerializeField] private CanvasGroup _transitionGroup = default;

        public IObservable<string> OnLoadedSceneAsObservable { get { return _onLoadedScene; } }
        private Subject<string> _onLoadedScene = default;

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
        }

        private void OnDisable()
        {
            _onLoadedScene.Dispose();
        }    

        /// <summary>
        /// フェードイン・アウトと共にシーン遷移します
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
        public IObservable<Unit> LoadSceneAsObservable(string sceneName, float fadeTime = 2.0f)
        {
            return Observable.FromCoroutine<Unit>((observer) => LoadScneneCoroutine(observer, sceneName, fadeTime));
        }

        private IEnumerator LoadScneneCoroutine(IObserver<Unit> observer, string sceneName, float fadeTime)
        {
            var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            yield return FadeIn(fadeTime);

            while (!async.isDone)
            {
                yield return null;                     
            }

            yield return new WaitForSeconds(1f);                 

            yield return FadeOut(fadeTime);

            _onLoadedScene.OnNext(sceneName);

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

            _transitionGroup.blocksRaycasts = true;

            while (t <= time)
            {
                _transitionGroup.alpha = 1 - t / time;

                yield return null;
                t += Time.deltaTime;
            }
            _transitionGroup.alpha = 0;

            _transitionGroup.blocksRaycasts = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;

namespace Utilities
{
    // TODO:�^�ʖڂɃV���O���g����
    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance { private set; get; }

        [SerializeField] private CanvasGroup transitionGroup = default;

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

        /// <summary>
        /// �V�[����؂�ւ���
        /// </summary>
        /// <param name="sceneName">�ǂݍ��ރV�[����</param>
        /// <param name="fadeTime">�t�F�[�h�C���E�A�E�g�ɂ����鎞��</param>
        /// <returns></returns>
        public IObservable<Unit> LoadScneAsObservable(string sceneName, float fadeTime = 2.0f)
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

            yield break;
        }

        /// <summary>
        /// �g�����W�V�����p�摜�Ńt�F�[�h�C������
        /// </summary>
        /// <param name="time">�t�F�[�h�ɂ����鎞��</param>
        /// <returns></returns>
        private IEnumerator FadeIn(float time = 1.0f)
        {
            float t = 0;
            transitionGroup.blocksRaycasts = true;

            while(t <= time)
            {
                transitionGroup.alpha = t / time;

                yield return null;
                t += Time.deltaTime;
            }

            transitionGroup.alpha = 1;
        }

        /// <summary>
        /// �g�����W�V�����p�摜�Ńt�F�[�h�A�E�g����
        /// </summary>
        /// <param name="time">�t�F�[�h�ɂ����鎞��</param>
        /// <returns></returns>
        private IEnumerator FadeOut(float time = 1.0f)
        {
            float t = 0;

            transitionGroup.blocksRaycasts = true;

            while (t <= time)
            {
                transitionGroup.alpha = 1 - t / time;

                yield return null;
                t += Time.deltaTime;
            }
            transitionGroup.alpha = 0;

            transitionGroup.blocksRaycasts = false;
        }
    }
}

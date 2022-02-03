using Logics;
using System.Collections;
using UniRx;
using UnityEngine;
using Utilities;
using Utilities.Audios;

namespace Homes
{
    public class HomeSceneController : MonoBehaviour
    {
        [SerializeField] private AudioClip _bgm = default;
        [SerializeField] private GameObject _backGround = default;
        [SerializeField] private float range = 20f;
        [SerializeField] private float speed = 0.3f;

        private void Awake()
        {
            SceneTransitionManager.Instance.OnFinishedFadeOutAsObservable
                .Subscribe(_ => StartCoroutine(OnLoadedScene()))
                .AddTo(gameObject);
        }

        private IEnumerator OnLoadedScene()
        {
            AudioManager.Instance.PlayBGM(_bgm, isLoop: false);

            float t = 0;
            var pos = _backGround.transform.position;

            while (t <= 1)
            {
                pos.y = t * range;
                _backGround.transform.position = pos;
                yield return null;
                t += Time.deltaTime * speed;
            }

            GameLogicManager.instance.NextPhase();
        }
    }
}
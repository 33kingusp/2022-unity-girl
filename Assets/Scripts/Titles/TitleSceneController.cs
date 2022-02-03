using Logics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Audios;

namespace Titles
{
    public class TitleSceneController : MonoBehaviour
    {
        [SerializeField] private Button _startButton = default;
        [SerializeField] private AudioClip _acceptSE = default;

        private void Awake()
        {
            _startButton.onClick.AsObservable()
                .Subscribe(_ => OnClickStart())
                .AddTo(gameObject);
        }

        private void OnClickStart()
        {
            AudioManager.Instance.PlaySE(_acceptSE);
            GameLogicManager.instance.StartGame();
        }
    }
}
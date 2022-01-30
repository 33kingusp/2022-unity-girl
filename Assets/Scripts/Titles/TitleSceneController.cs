using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Utilities;
using Logics;

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
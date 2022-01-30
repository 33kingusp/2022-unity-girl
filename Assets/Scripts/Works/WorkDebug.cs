using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Logics;

namespace Works
{
    public class WorkDebug : MonoBehaviour
    {
        [SerializeField] private Button _exitButton = default;

        private void Awake()
        {
            _exitButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    GameLogicManager.instance.NextPhase();
                })
                .AddTo(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Utilities;
using Logics;

namespace Results
{
    public class ResultSceneController : MonoBehaviour
    {
        [SerializeField] private Transform _imageParent = default;
        [SerializeField] private RawImage _rawImageBase = default;

        [SerializeField] private Image _blackImage = default;
        [SerializeField] private Button _nextButton = default;
        [SerializeField] private Text _messageText = default;

        [SerializeField] private EndingScriptableObject[] _endings = default;
        private EndingScriptableObject _currentEnding = default;

        private Queue<string> _messageQueue = default;

        private void Start()
        {
            _currentEnding = _endings[GameLogicManager.instance.CurrentEndingId];

            foreach (var sprites in _currentEnding.EndCardsprites)
            {
                var image = Instantiate(_rawImageBase, _imageParent);
                image.texture = sprites.texture;
            }

            _messageQueue = new Queue<string>(_currentEnding.Messages);
            _messageQueue.Enqueue($"{_currentEnding.Name}");

            _nextButton.onClick.AsObservable()
                .Subscribe(_ => NextMessage())
                .AddTo(gameObject);

            
            SceneTransitionManager.Instance.OnFinishedFadeOutAsObservable
                .First()
                .Subscribe(_ => StartCoroutine(EndingBeginFlow()))
                .AddTo(gameObject);

            _nextButton.interactable = false;

            //StartCoroutine(EndingBeginFlow());
        }

        private void NextMessage()
        {
            if (_messageQueue.Count > 0)
            {
                var message = _messageQueue.Dequeue();
                _messageText.text = message;
            }
            else
            {
                _messageText.text = "";
                StartCoroutine(EndingEndFlow());
            }
        }

        private IEnumerator EndingBeginFlow()
        {
            Debug.Log("EndingBeginFlow");

            if (_currentEnding.EndSound != null)
            {
                AudioManager.Instance.PlaySE(_currentEnding.EndSound);
            }
            yield return FadeIn();

            NextMessage();
            _nextButton.interactable = true;
        }

        private IEnumerator EndingEndFlow()
        {
            yield return FadeOut();
        }

        private IEnumerator FadeIn()
        {
            float t = 0;
            while(t < 1)
            {
                var c = _blackImage.color;
                c.a = t* 0.5f;
                _blackImage.color = c;
                yield return null;
                t += Time.deltaTime;
            }
        }

        private IEnumerator FadeOut()
        {
            float t = 1;
            var defA = _blackImage.color.a;
            while (t >= 0)
            {
                var c = _blackImage.color;
                c.a = t * defA;
                _blackImage.color = c;
                yield return null;
                t -= Time.deltaTime;
            }
        }
    }
}
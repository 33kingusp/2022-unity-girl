using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Audio;

namespace Utilities.Audios
{
    [DisallowMultipleComponent]
    public class VolumeWindow : MonoBehaviour
    {
        [SerializeField] private Button _openButton = default;
        [SerializeField] private VolumeSlider _bgmSlider = default;
        [SerializeField] private VolumeSlider _seSlider = default;

        [SerializeField] private AudioMixer _audioMixer = default;

        [SerializeField] private float _openTime = 0.5f;
        [SerializeField] private bool _isOpened = default;

        [SerializeField] private float _closedX = default;
        [SerializeField] private float _opendX = default;

        private Coroutine _currentCoroutine = default;

        private void Awake()
        {
            _openButton.onClick.AsObservable()
                .Do(_ =>
                {
                    if (_currentCoroutine != null)
                    {
                        StopCoroutine(_currentCoroutine);
                    }
                })
                .Subscribe(_ =>
                {
                    if (_isOpened)
                    {
                        _currentCoroutine = StartCoroutine(CloseCoroutine());
                    }
                    else
                    {
                        _currentCoroutine = StartCoroutine(OpenCoroutine());
            
                    }
                })
                .AddTo(gameObject);

            this.ObserveEveryValueChanged(_ => _bgmSlider.Volume)
                .Select(volume => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f))
                .Subscribe(volume => _audioMixer.SetFloat("BGM", volume))
                .AddTo(gameObject);

            this.ObserveEveryValueChanged(_ => _seSlider.Volume)
                .Select(volume => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f))
                .Subscribe(volume => _audioMixer.SetFloat("SE", volume))
                .AddTo(gameObject);
        }

        private void Start()
        {
            var rt = (RectTransform)transform;
            var pos = rt.localPosition;
            if (_isOpened)
            {
                pos.x = _opendX;
            }
            else
            {
                pos.x = _closedX;
            }
            rt.localPosition = pos;
        }

        private IEnumerator OpenCoroutine()
        {
            if (_isOpened)
            {
                yield break;
            }

            var rt = (RectTransform)transform;
            var pos = rt.localPosition;
            float t = 0;

            _isOpened = true;

            while (t <= 1)
            {
                pos.x = Mathf.Lerp(_closedX, _opendX, t);
                rt.localPosition = pos;
                yield return null;
                t += Time.deltaTime / _openTime;
            }
            pos.x = _opendX;
            rt.localPosition = pos;

            yield break;
        }

        private IEnumerator CloseCoroutine()
        {
            if (!_isOpened)
            {
                yield break;
            }

            var rt = (RectTransform)transform;
            var pos = rt.localPosition;
            float t = 0;

            _isOpened = false;

            while (t <= 1)
            {
                pos.x = Mathf.Lerp(_opendX, _closedX, t);
                rt.localPosition = pos;
                yield return null;
                t += Time.deltaTime / _openTime;
            }
            pos.x = _closedX;
            rt.localPosition = pos;

            yield break;
        }
    }
}
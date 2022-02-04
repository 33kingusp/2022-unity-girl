using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Utilities.Audios
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Button _speakerButton = default;
        [SerializeField] private Slider _volumeSlider = default;

        [SerializeField] Sprite[] _speakerSprites = default;

        public bool IsMute { private set; get; }
        public float Volume 
        { 
            get
            {
                if (IsMute)
                {
                    return 0f;
                }
                else
                {
                    return _volume;
                }
            }
        }

        private float _volume = 1f;


        private void Awake()
        {
            var muteChange = this.ObserveEveryValueChanged(_ => IsMute).Share();
            var sliderChange = this.ObserveEveryValueChanged(_ => _volumeSlider.value).Share();

            // スピーカーマークが押されるたびに、ミュート切り替え
            _speakerButton.onClick.AsObservable()
                .Subscribe(_ => IsMute = !IsMute)
                .AddTo(gameObject);

            // ミュートが変わったらスライダーを更新
            muteChange
                .Subscribe(_ => _volumeSlider.value = Volume)
                .AddTo(gameObject);

            // スライダーが変わったとき
            // ボリューム0でなければ、ミュート解除
            sliderChange
                .Where(volume => volume > 0)
                .Where(_ => IsMute)
                .Subscribe(_ => IsMute = false)
                .AddTo(gameObject);

            // スライダーが変わったとき
            // ミュートではなければ、ボリュームを更新
            sliderChange
                .Where(_ => !IsMute)
                .Subscribe(v => _volume = v)
                .AddTo(gameObject);

            // スライダーが変わったらUI更新
            sliderChange
                .Subscribe(_ => UpdateUI())
                .AddTo(gameObject);
        }

        private void Start()
        {
            // 初回更新
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (Volume <= 0.0f)
            {
                _speakerButton.image.sprite = _speakerSprites[0];
            }
            else if (Volume < 0.5f)
            {
                _speakerButton.image.sprite = _speakerSprites[1];
            }
            else if (Volume < 1.0f)
            {
                _speakerButton.image.sprite = _speakerSprites[2];
            }
            else
            {
                _speakerButton.image.sprite = _speakerSprites[3];
            }
        }
    }
}
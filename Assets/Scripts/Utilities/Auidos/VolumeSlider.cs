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

            // �X�s�[�J�[�}�[�N��������邽�тɁA�~���[�g�؂�ւ�
            _speakerButton.onClick.AsObservable()
                .Subscribe(_ => IsMute = !IsMute)
                .AddTo(gameObject);

            // �~���[�g���ς������X���C�_�[���X�V
            muteChange
                .Subscribe(_ => _volumeSlider.value = Volume)
                .AddTo(gameObject);

            // �X���C�_�[���ς�����Ƃ�
            // �{�����[��0�łȂ���΁A�~���[�g����
            sliderChange
                .Where(volume => volume > 0)
                .Where(_ => IsMute)
                .Subscribe(_ => IsMute = false)
                .AddTo(gameObject);

            // �X���C�_�[���ς�����Ƃ�
            // �~���[�g�ł͂Ȃ���΁A�{�����[�����X�V
            sliderChange
                .Where(_ => !IsMute)
                .Subscribe(v => _volume = v)
                .AddTo(gameObject);

            // �X���C�_�[���ς������UI�X�V
            sliderChange
                .Subscribe(_ => UpdateUI())
                .AddTo(gameObject);
        }

        private void Start()
        {
            // ����X�V
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using UniRx.Triggers;

namespace Data
{
    public class PlayerInfoController : MonoBehaviour
    {
        private const float OneHourAngle = 30.0f;
        private const float HalfHourAngle = 180.0f;

        [SerializeField]
        private Text gameTurnCount_;  //�Q�[���̃^�[����(�v���C���[�����H)
        [SerializeField]
        private RectTransform hourRect_;

        [SerializeField]
        private Slider stressSlider_;

        private int maxGageValue_;


        // Start is called before the first frame update
        void Start()
        {
            //UI�l�ύX����Canvas���UI���ύX�������s��

            PlayerInfoManager.instance.gameTurnCount.Subscribe(_ => gameTurnCount_.text = _.ToString());
            PlayerInfoManager.instance.currentTime.Subscribe(_=>MoveHandClock(_));
            PlayerInfoManager.instance.stressValue.Subscribe(_ => UpdateStressGage(_));
            maxGageValue_ = PlayerInfoManager.instance.stressValue.Value;
        }

        //�V�[���J�ڎ��Ƀv���C���[����������(�����p��)���s��
        public static void InitializeValue(int turn, int drunk, int money, int stress)
        {
            PlayerInfoManager.instance.gameTurnCount.Value = turn;
            PlayerInfoManager.instance.drunkValue.Value = drunk;
            PlayerInfoManager.instance.moneyValue.Value = money;
            PlayerInfoManager.instance.stressValue.Value = stress;
            PlayerInfoManager.instance.currentTime.Value = 18;
        }

        //���v�̐j�𓮂���
        void MoveHandClock(int value)
        {
            hourRect_.localEulerAngles = new Vector3(0, 0, HalfHourAngle - OneHourAngle * value);
        }

        //�X�g���X�Q�[�W���X�V
        void UpdateStressGage(int value)
        {
            float per = (float)value / (float)maxGageValue_;
            stressSlider_.value = per;
            if(value<=0)
            {
                stressSlider_.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
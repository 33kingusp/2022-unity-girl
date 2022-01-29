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
        [SerializeField]
        private Text gameTurnCount_;  //�Q�[���̃^�[����(�v���C���[�����H)
        [SerializeField]
        private Text drunkValue_;  //�����l
        [SerializeField]
        private Text stressValue_;    //�X�g���X�l
        [SerializeField]
        private Text moneyValue_;    //�g������z
        [SerializeField]
        private Text currentTime;    //���݂̎���


        // Start is called before the first frame update
        void Start()
        {
            PlayerInfoManager.instance.gameTurnCount.Subscribe(_ => gameTurnCount_.text = _.ToString());
            PlayerInfoManager.instance.drunkValue.Subscribe(_ => drunkValue_.text = _.ToString());
            PlayerInfoManager.instance.moneyValue.Subscribe(_ => moneyValue_.text = _.ToString());
            PlayerInfoManager.instance.stressValue.Subscribe(_ => stressValue_.text = _.ToString());
            PlayerInfoManager.instance.currentTime.Subscribe(_ => currentTime.text = _.ToString());
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

    }
}
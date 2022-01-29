using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Utilities;

namespace Data
{
    public class PlayerInfoManager : SingletonMonoBehaviour<PlayerInfoManager>
    {
        private IntReactiveProperty gameTurnCount_ = new IntReactiveProperty(0);  //�Q�[���̃^�[����(�v���C���[�����H)
        private IntReactiveProperty drunkValue_ = new IntReactiveProperty(0);  //�����l
        private IntReactiveProperty moneyValue_ = new IntReactiveProperty(10000);    //�\�Z
        private IntReactiveProperty stressValue_ = new IntReactiveProperty(30);    //�X�g���X

        private IntReactiveProperty actionCount_ = new IntReactiveProperty(0);    //���ݎ���
        public IntReactiveProperty gameTurnCount
        {
            get
            {
                return gameTurnCount_;
            }
            set
            {
                gameTurnCount_ = value;
            }
        }
        public IntReactiveProperty drunkValue
        {
            get
            {
                return drunkValue_;
            }
            set
            {
                drunkValue_ = value;
            }
        }
        public IntReactiveProperty moneyValue
        {
            get
            {
                return moneyValue_;
            }
            set
            {
                moneyValue_ = value;
            }
        }
        public IntReactiveProperty currentTime
        {
            get
            {
                return actionCount_;
            }
            set
            {
                actionCount_ = value;
            }
        }
        public IntReactiveProperty stressValue
        {
            get
            {
                return stressValue_;
            }
            set
            {
                stressValue_ = value;
            }
        }
        public void SetUp()
        {
            //PlayerInfo�̕\��
            if (this.transform.childCount == 0)
            {
                GameObject obj = Instantiate((GameObject)Resources.Load("UI/PlayerInfo"));
                obj.transform.parent = this.transform;
            }
        }


    }
}
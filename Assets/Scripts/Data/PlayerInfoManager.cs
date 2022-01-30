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
        private IntReactiveProperty stressValue_ = new IntReactiveProperty(30);    //���݃X�g���X
        private int maxStressValue_=30;                                          //�^�[�����Ƃ̃X�g���X�̍ő�l

        private IntReactiveProperty actionCount_ = new IntReactiveProperty(18);    //���ݎ���

        private List<int> drinkTypeList_ = new List<int>();
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
        public IntReactiveProperty actionCount
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

        public List<int>drinkTypeList
        {
            get
            {
                return drinkTypeList_;
            }
            set
            {
                drinkTypeList_ = value;
            }
        }
        public int maxStressValue
        {
            get
            {
                return maxStressValue_;
            }
            set
            {
                maxStressValue_ = value;
            }
        }
        public void SetUp()
        {
            //PlayerInfo�̕\��
            if (this.transform.childCount == 0)
            {
                GameObject obj = Instantiate((GameObject)Resources.Load(AssetDataPath.PlayerInfo));
                obj.transform.parent = this.transform;
            }
        }

        public void Clean()
        {
            Destroy(this.gameObject);
        }
    }
}
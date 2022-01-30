using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Utilities;

namespace Data
{
    public class PlayerInfoManager : SingletonMonoBehaviour<PlayerInfoManager>
    {
        private IntReactiveProperty gameTurnCount_ = new IntReactiveProperty(0);  //ゲームのターン数(プレイヤーが持つ？)
        private IntReactiveProperty drunkValue_ = new IntReactiveProperty(0);  //酔い値
        private IntReactiveProperty moneyValue_ = new IntReactiveProperty(10000);    //予算
        private IntReactiveProperty stressValue_ = new IntReactiveProperty(30);    //現在ストレス
        private int maxStressValue_=30;                                          //ターンごとのストレスの最大値

        private IntReactiveProperty actionCount_ = new IntReactiveProperty(18);    //現在時刻

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
            //PlayerInfoの表示
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
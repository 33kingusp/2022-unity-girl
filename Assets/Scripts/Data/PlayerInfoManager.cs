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
        private IntReactiveProperty stressValue_ = new IntReactiveProperty(30);    //ストレス

        private IntReactiveProperty actionCount_ = new IntReactiveProperty(0);    //現在時刻
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
            //PlayerInfoの表示
            if (this.transform.childCount == 0)
            {
                GameObject obj = Instantiate((GameObject)Resources.Load("UI/PlayerInfo"));
                obj.transform.parent = this.transform;
            }
        }


    }
}
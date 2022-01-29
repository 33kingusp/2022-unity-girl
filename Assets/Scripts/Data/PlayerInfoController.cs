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
        private Text gameTurnCount_;  //ゲームのターン数(プレイヤーが持つ？)
        [SerializeField]
        private Text drunkValue_;  //酔い値
        [SerializeField]
        private Text stressValue_;    //ストレス値
        [SerializeField]
        private Text moneyValue_;    //使える金額
        [SerializeField]
        private Text currentTime;    //現在の時刻


        // Start is called before the first frame update
        void Start()
        {
            PlayerInfoManager.instance.gameTurnCount.Subscribe(_ => gameTurnCount_.text = _.ToString());
            PlayerInfoManager.instance.drunkValue.Subscribe(_ => drunkValue_.text = _.ToString());
            PlayerInfoManager.instance.moneyValue.Subscribe(_ => moneyValue_.text = _.ToString());
            PlayerInfoManager.instance.stressValue.Subscribe(_ => stressValue_.text = _.ToString());
            PlayerInfoManager.instance.currentTime.Subscribe(_ => currentTime.text = _.ToString());
        }

        //シーン遷移時にプレイヤー情報を初期化(引き継ぎ)を行う
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
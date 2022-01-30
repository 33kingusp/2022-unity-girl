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
        private Text gameTurnCount_;  //ゲームのターン数(プレイヤーが持つ？)
        [SerializeField]
        private RectTransform hourRect_;

        [SerializeField]
        private Slider stressSlider_;



        // Start is called before the first frame update
        void Start()
        {
            //UI値変更時にCanvas上のUIも変更処理を行う

            PlayerInfoManager.instance.gameTurnCount.Subscribe(_ => gameTurnCount_.text = _.ToString());
            PlayerInfoManager.instance.actionCount.Subscribe(_=>MoveHandClock(_));
            PlayerInfoManager.instance.stressValue.Subscribe(_ => UpdateStressGage(_));
        }

        //シーン遷移時にプレイヤー情報を初期化(引き継ぎ)を行う
        public static void InitializeValue(int turn, int drunk, int money, int stress)
        {
            PlayerInfoManager.instance.gameTurnCount.Value = turn;
            PlayerInfoManager.instance.drunkValue.Value = drunk;
            PlayerInfoManager.instance.moneyValue.Value = money;
            PlayerInfoManager.instance.stressValue.Value = stress;
            PlayerInfoManager.instance.actionCount.Value = 18;
        }

        //時計の針を動かす
        void MoveHandClock(int value)
        {
            hourRect_.localEulerAngles = new Vector3(0, 0, - OneHourAngle * value);
        }

        //ストレスゲージを更新
        void UpdateStressGage(int value)
        {
            float per = (float)value / (float)PlayerInfoManager.instance.maxStressValue;
            stressSlider_.value = per;
        }
    }
}
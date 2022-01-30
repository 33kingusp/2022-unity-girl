using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using Data;
using Logics;
using Utilities;

namespace Bars
{
    public class BarController : MonoBehaviour
    {
        private const int CloseActionCount = 24;       //帰宅時間
        private const int GiveUpDrunkValue = 50;    //酔いの限界値

        [SerializeField]
        private GameObject buttonObj_;
        [SerializeField]
        private Text gameText_;
        BoolReactiveProperty exitButtonFlag_ = new BoolReactiveProperty(false);

        void Start()
        {
            CreateAlcoholButton();
        }

        // Update is called once per frame
        void Update()
        {

        }


        void CreateAlcoholButton()
        {
            //お酒の数だけボタンを生成
            for (int i = 0; i < (int)BaseAlcohol.AlcoholType.MAX; i++)
            {
                GameObject obj = Instantiate((GameObject)Resources.Load("UI/AlcoholButton"));
                obj.transform.parent = buttonObj_.transform;
                obj.transform.localScale = Vector3.one;
                //お酒の情報を登録する
                BaseAlcohol alcoholInfo = obj.GetComponent<BaseAlcohol>();
                alcoholInfo.SetInfo(AssetDataPath.AlcoholName[i], AssetDataPath.AlcoholDegree[i], AssetDataPath.AlcoholPrice[i], i);
                //押した際の処理を登録する
                Button button = obj.GetComponent<Button>();
                button.onClick.AddListener(() => { PushButton(alcoholInfo, button); });

                //退出ボタンは押せないようにする
                if (alcoholInfo.type == BaseAlcohol.AlcoholType.EXIT)
                {
                    //ボタンを押せない状態にする
                    exitButtonFlag_.Subscribe(flag =>
                    {
                        if (flag)
                        {
                            button.interactable = true;
                            button.transform.GetComponent<Image>().sprite= Utilities.LoadSpriteData.LoadSprite(AssetDataPath.BtnPush);
                        }
                        else
                        {
                            button.interactable = false;
                            button.transform.GetComponent<Image>().sprite = Utilities.LoadSpriteData.LoadSprite(AssetDataPath.BtnNotPush);

                        }
                    });
                }
            }
        }

        //お酒ボタンを押した際の処理
        public void PushButton(BaseAlcohol info, Button button)
        {
            //ボタンの選択解除する為の処理
            EventSystem.current.SetSelectedGameObject(null);
            //もし所持金が足りなかったら
            if (PlayerInfoManager.instance.moneyValue.Value - info.price < 0)
            {
                //ボタンクリックの処理は行わない
                return;
            }

            PlayerInfoManager.instance.moneyValue.Value -= info.price;
            PlayerInfoManager.instance.drunkValue.Value += info.alcoholDegree; ;
            PlayerInfoManager.instance.stressValue.Value -= info.alcoholDegree; ;
            PlayerInfoManager.instance.actionCount.Value++;

            //退出ボタン以外の情報は保持する
            if(info.type!=BaseAlcohol.AlcoholType.EXIT)
            {
                PlayerInfoManager.instance.drinkTypeList.Add((int)info.type);
            }

            //メッセージを更新する
            gameText_.text=ViewGameMessage(info);

            if (useExitButton(button))
            {
                exitButtonFlag_.Value = true;
            }
            //強制的にシーン遷移を行う処理
            if (CheckBarClose() || CheckDrunkValue())
            {
                GameLogicManager.instance.NextPhase();
                //シーン遷移を行い、家に帰る
            }

        }

        //退出ボタンを押せる状態かどうか
        private bool useExitButton(Button button)
        {
            if (PlayerInfoManager.instance.stressValue.Value > 0)
            {
                return false;
            }
            return true;
        }

        //帰宅時間かどうか
        private bool CheckBarClose()
        {
            if (PlayerInfoManager.instance.actionCount.Value < CloseActionCount)
            {
                return false;
            }
            return true;
        }

        //酔いが限界かどうか
        private bool CheckDrunkValue()
        {
            if (PlayerInfoManager.instance.drunkValue.Value < GiveUpDrunkValue)
            {
                return false;
            }
            return true;
        }

        //押したボタンに応じてメッセージを変更する
        string ViewGameMessage(BaseAlcohol alcohol)
        {
            return AssetDataPath.GameLog[(int)alcohol.type];
        }

    }
}
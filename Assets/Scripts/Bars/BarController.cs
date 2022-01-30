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
        enum DrunkMode
        {
            NORMAL,
            SOFT,
            HARD,
            MAX
        }

        private const int CloseActionCount = 24;       //帰宅時間
        private static readonly int[] GiveUpDrunkValue = { 0, 18, 36, 50 };    //酔いの限界値

        [SerializeField]
        private GameObject buttonObj_;
        [SerializeField]
        private Text gameText_;
        [SerializeField]
        private Sprite[] alcoholBtnSprite_;
        [SerializeField]
        private Sprite[] btnBackSprite_;
        [SerializeField]
        private Sprite[] charaSprite_;
        [SerializeField]
        private Image charaImg_;
        BoolReactiveProperty exitButtonFlag_ = new BoolReactiveProperty(false);
        private bool firstFlag_ = true;     

        private BaseAlcohol.AlcoholType prevType_=BaseAlcohol.AlcoholType.WATER;

        private float magDegree_;          //お酒の倍率
        void Start()
        {
            CreateAlcoholButton();

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
                alcoholInfo.SetInfo(AssetDataPath.AlcoholName[i], AssetDataPath.AlcoholDegree[i], AssetDataPath.AlcoholPrice[i], i, alcoholBtnSprite_[i]);
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
                            button.transform.GetComponent<Image>().sprite = btnBackSprite_[0];
                        }
                        else
                        {
                            if (PlayerInfoManager.instance.moneyValue.Value < AssetDataPath.AlcoholPrice[(int)BaseAlcohol.AlcoholType.BEER])
                            {
                                button.interactable = true;
                                button.transform.GetComponent<Image>().sprite = btnBackSprite_[0];
                            }
                            else
                            {
                                button.interactable = false;
                                button.transform.GetComponent<Image>().sprite = btnBackSprite_[1];
                            }

                        }
                    });
                }
                else
                {
                    //PushButtonCheck(PlayerInfoManager.instance.moneyValue.Value >= alcoholInfo.price, button);
                }
            }
        }

        //お酒ボタンを押した際の処理
        public void PushButton(BaseAlcohol info, Button button)
        {
            //ボタンの選択解除する為の処理
            EventSystem.current.SetSelectedGameObject(null);

            //メッセージを更新する
            gameText_.text = ViewGameMessage(info);
            //金額が足りない場合は処理をしない
            if (PlayerInfoManager.instance.moneyValue.Value - info.price < 0)
            {
                return;
            }
            //別のお酒を飲んだ場合の処理
            DrinkAncoholType(info);

            PlayerInfoManager.instance.moneyValue.Value -= info.price;
            if (prevType_ == BaseAlcohol.AlcoholType.WATER)
            {
                PlayerInfoManager.instance.drunkValue.Value += info.alcoholDegree;
            }
            else
            {
                PlayerInfoManager.instance.drunkValue.Value += (int)(info.alcoholDegree * magDegree_);
            }
            if (PlayerInfoManager.instance.stressValue.Value > 0)
            {
                PlayerInfoManager.instance.stressValue.Value -= info.alcoholDegree;
                //マイナスになったら0に戻す
                if (PlayerInfoManager.instance.stressValue.Value < 0)
                {
                    PlayerInfoManager.instance.stressValue.Value = 0;
                }
            }
            PlayerInfoManager.instance.actionCount.Value++;

            //PlayerInfo更新後の処理

            //キャラクターの画像の切り替え判定
            DrunkCharacterImage();


            //退出ボタン以外の情報は保持する
            if (info.type != BaseAlcohol.AlcoholType.EXIT)
            {
                PlayerInfoManager.instance.drinkTypeList.Add((int)info.type);
            }
            //退出ボタンの状態を切り替える
            if (useExitButton(button) || PlayerInfoManager.instance.moneyValue.Value < AssetDataPath.AlcoholPrice[(int)BaseAlcohol.AlcoholType.BEER])
            {
                exitButtonFlag_.Value = true;
            }
            //強制的にシーン遷移を行う処理
            if (CheckBarClose() || CheckDrunkValue() || info.type == BaseAlcohol.AlcoholType.EXIT)
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
            if (PlayerInfoManager.instance.drunkValue.Value < GiveUpDrunkValue[(int)DrunkMode.MAX])
            {
                return false;
            }
            return true;
        }

        //押したボタンに応じてメッセージを変更する
        string ViewGameMessage(BaseAlcohol alcohol)
        {
            if(PlayerInfoManager.instance.moneyValue.Value<alcohol.price)
            {
                return AssetDataPath.NotPushLog[(int)alcohol.type];

            }

          if(PlayerInfoManager.instance.moneyValue.Value < AssetDataPath.AlcoholPrice[(int)BaseAlcohol.AlcoholType.BEER])
            {
                if(alcohol.type==BaseAlcohol.AlcoholType.EXIT)
                {
                    return AssetDataPath.GameLog[(int)alcohol.type];
                }else
                {
                    return AssetDataPath.NotPushLog[(int)alcohol.type];

                }
            }
            return AssetDataPath.GameLog[(int)alcohol.type];
        }

        //酔い値に応じてキャラクターのイラストを切り替える
        void DrunkCharacterImage()
        {
            for (int i = 0; i < (int)DrunkMode.MAX; i++)
            {
                if (PlayerInfoManager.instance.drunkValue.Value > GiveUpDrunkValue[i])
                {
                    charaImg_.sprite = charaSprite_[i];
                }
                else
                {
                    break;
                }
            }
        }

        
        //前と飲んだお酒と同じ種類だったら
        void DrinkAncoholType(BaseAlcohol info)
        {
            if (firstFlag_)
            {
                firstFlag_ = false;
                magDegree_ = 1.0f;
                return;
            }
            if (info.type != prevType_)
            {
                magDegree_ = 1.5f;
                prevType_ = info.type;
            }
            magDegree_ = 1.0f;
        }
        //ボタンを押せるかどうか
        void PushButtonCheck(bool flag, Button button)
        {
            if (flag)
            {
                button.interactable = true;
                button.transform.GetComponent<Image>().sprite = btnBackSprite_[0];
            }
            else
            {
                button.interactable = false;
                button.transform.GetComponent<Image>().sprite = btnBackSprite_[1];
            }
        }
    }
    

}
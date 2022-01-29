using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
public class BarController : MonoBehaviour
{
    private const int CloseBarTime = 24;       //帰宅時間
    private const int GiveUpDrunkValue = 50;    //酔いの限界値

    [SerializeField]
    private GameObject buttonObj_;
    BoolReactiveProperty exitButtonFlag_ = new BoolReactiveProperty(false);

    // Start is called before the first frame update
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
            alcoholInfo.SetInfo(AlcoholData.AlcoholName[i], AlcoholData.AlcoholDegree[i], AlcoholData.AlcoholPrice[i], i);
            //押した際の処理を登録する
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => { PushButton(alcoholInfo, button); });

            //退出ボタンは押せないようにする
            if (alcoholInfo.type_ == BaseAlcohol.AlcoholType.EXIT)
            {
                //ボタンを押せない状態にする
                exitButtonFlag_.Subscribe(flag =>
                {
                    if (flag)
                    {
                        button.interactable = true;
                    }
                    else
                    {
                        button.interactable = false;

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
        if (PlayerInfoManager.instance.moneyValue.Value-info.price_<0)
        {
            //ボタンクリックの処理は行わない
            return;
        }

        PlayerInfoManager.instance.moneyValue.Value -= info.price_;
        PlayerInfoManager.instance.drunkValue.Value += info.alcoholDegree_; ;
        PlayerInfoManager.instance.stressValue.Value -= info.alcoholDegree_; ;
        PlayerInfoManager.instance.currentTime.Value++;


        if (useExitButton(button))
        {
            exitButtonFlag_.Value = true;
        }
        //強制的にシーン遷移を行う処理
        if(CheckBarClose()||CheckDrunkValue())
        {
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
        if (PlayerInfoManager.instance.currentTime.Value < CloseBarTime)
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class BarController : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonObj_;
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
            button.onClick.AddListener(() => { PushButton(alcoholInfo,button); });
        }
    }

    //お酒ボタンを押した際の処理
    public void PushButton(BaseAlcohol info,Button button)
    {
        PlayerInfoManager.instance.moneyValue.Value -= info.price_;
        PlayerInfoManager.instance.drunkValue.Value += info.alcoholDegree_; ;
        PlayerInfoManager.instance.stressValue.Value -= info.alcoholDegree_; ;
        PlayerInfoManager.instance.currentTime.Value++;
        //ボタンの選択解除する為の処理
        EventSystem.current.SetSelectedGameObject(null);
    }
}

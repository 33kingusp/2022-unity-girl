using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerInfoManager : SingletonMonoBehaviour<PlayerInfoManager>
{
    private ReactiveProperty<int> gameTurnCount_=new ReactiveProperty<int>(5);  //ゲームのターン数(プレイヤーが持つ？)
    private int emotionvalue_=50;  //ネガポジの値
    private IntReactiveProperty usingMoney_ =new IntReactiveProperty(5000);    //使える金額
    public ReactiveProperty<int> gameTurnCount
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
    public int emotionValue
    {
        get
        {
            return emotionvalue_;
        }
        set
        {
            emotionvalue_ = value;
        }
    }
    public IntReactiveProperty usingMoney
    {
        get
        {
            return usingMoney_;
        }
        set
        {
            usingMoney_ = value;
        }
    }

    public void SetUp()
    {
        //PlayerInfoの表示
        if(this.transform.childCount==0)
        {
            GameObject obj = Instantiate((GameObject)Resources.Load("UI/PlayerInfo"));
            obj.transform.parent = this.transform;
        }
    }


}

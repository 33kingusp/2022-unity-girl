using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : SingletonMonoBehaviour<PlayerInfoManager>
{
    private int gameTurnCount_=5;  //�Q�[���̃^�[����(�v���C���[�����H)
    private int emotionvalue_=50;  //�l�K�|�W�̒l
    private int usingMoney_=1000;    //�g������z
    public int gameTurnCount
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
    public int usingMoney
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
        //PlayerInfo�̕\��
        if(this.transform.childCount==0)
        {
            GameObject obj = Instantiate((GameObject)Resources.Load("UI/PlayerInfo"));
            obj.transform.parent = this.transform;
        }
    }


}

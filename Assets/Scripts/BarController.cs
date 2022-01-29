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
    private const int CloseBarTime = 24;       //�A���
    private const int GiveUpDrunkValue = 50;    //�����̌��E�l

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
        //�����̐������{�^���𐶐�
        for (int i = 0; i < (int)BaseAlcohol.AlcoholType.MAX; i++)
        {
            GameObject obj = Instantiate((GameObject)Resources.Load("UI/AlcoholButton"));
            obj.transform.parent = buttonObj_.transform;
            obj.transform.localScale = Vector3.one;
            //�����̏���o�^����
            BaseAlcohol alcoholInfo = obj.GetComponent<BaseAlcohol>();
            alcoholInfo.SetInfo(AlcoholData.AlcoholName[i], AlcoholData.AlcoholDegree[i], AlcoholData.AlcoholPrice[i], i);
            //�������ۂ̏�����o�^����
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => { PushButton(alcoholInfo, button); });

            //�ޏo�{�^���͉����Ȃ��悤�ɂ���
            if (alcoholInfo.type_ == BaseAlcohol.AlcoholType.EXIT)
            {
                //�{�^���������Ȃ���Ԃɂ���
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

    //�����{�^�����������ۂ̏���
    public void PushButton(BaseAlcohol info, Button button)
    {
        //�{�^���̑I����������ׂ̏���
        EventSystem.current.SetSelectedGameObject(null);
        //����������������Ȃ�������
        if (PlayerInfoManager.instance.moneyValue.Value-info.price_<0)
        {
            //�{�^���N���b�N�̏����͍s��Ȃ�
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
        //�����I�ɃV�[���J�ڂ��s������
        if(CheckBarClose()||CheckDrunkValue())
        {
            //�V�[���J�ڂ��s���A�ƂɋA��
        }

    }

    //�ޏo�{�^�����������Ԃ��ǂ���
    private bool useExitButton(Button button)
    {
        if (PlayerInfoManager.instance.stressValue.Value > 0)
        {
            return false;
        }
        return true;
    }

    //�A��Ԃ��ǂ���
    private bool CheckBarClose()
    {
        if (PlayerInfoManager.instance.currentTime.Value < CloseBarTime)
        {
            return false;
        }
        return true;
    }

    //���������E���ǂ���
    private bool CheckDrunkValue()
    {
        if (PlayerInfoManager.instance.drunkValue.Value < GiveUpDrunkValue)
        {
            return false;
        }
        return true;
    }
}

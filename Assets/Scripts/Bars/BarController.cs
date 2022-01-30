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
        private const int CloseActionCount = 24;       //�A���
        private const int GiveUpDrunkValue = 50;    //�����̌��E�l

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
            //�����̐������{�^���𐶐�
            for (int i = 0; i < (int)BaseAlcohol.AlcoholType.MAX; i++)
            {
                GameObject obj = Instantiate((GameObject)Resources.Load("UI/AlcoholButton"));
                obj.transform.parent = buttonObj_.transform;
                obj.transform.localScale = Vector3.one;
                //�����̏���o�^����
                BaseAlcohol alcoholInfo = obj.GetComponent<BaseAlcohol>();
                alcoholInfo.SetInfo(AssetDataPath.AlcoholName[i], AssetDataPath.AlcoholDegree[i], AssetDataPath.AlcoholPrice[i], i);
                //�������ۂ̏�����o�^����
                Button button = obj.GetComponent<Button>();
                button.onClick.AddListener(() => { PushButton(alcoholInfo, button); });

                //�ޏo�{�^���͉����Ȃ��悤�ɂ���
                if (alcoholInfo.type == BaseAlcohol.AlcoholType.EXIT)
                {
                    //�{�^���������Ȃ���Ԃɂ���
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

        //�����{�^�����������ۂ̏���
        public void PushButton(BaseAlcohol info, Button button)
        {
            //�{�^���̑I����������ׂ̏���
            EventSystem.current.SetSelectedGameObject(null);
            //����������������Ȃ�������
            if (PlayerInfoManager.instance.moneyValue.Value - info.price < 0)
            {
                //�{�^���N���b�N�̏����͍s��Ȃ�
                return;
            }

            PlayerInfoManager.instance.moneyValue.Value -= info.price;
            PlayerInfoManager.instance.drunkValue.Value += info.alcoholDegree; ;
            PlayerInfoManager.instance.stressValue.Value -= info.alcoholDegree; ;
            PlayerInfoManager.instance.actionCount.Value++;

            //�ޏo�{�^���ȊO�̏��͕ێ�����
            if(info.type!=BaseAlcohol.AlcoholType.EXIT)
            {
                PlayerInfoManager.instance.drinkTypeList.Add((int)info.type);
            }

            //���b�Z�[�W���X�V����
            gameText_.text=ViewGameMessage(info);

            if (useExitButton(button))
            {
                exitButtonFlag_.Value = true;
            }
            //�����I�ɃV�[���J�ڂ��s������
            if (CheckBarClose() || CheckDrunkValue())
            {
                GameLogicManager.instance.NextPhase();
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
            if (PlayerInfoManager.instance.actionCount.Value < CloseActionCount)
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

        //�������{�^���ɉ����ă��b�Z�[�W��ύX����
        string ViewGameMessage(BaseAlcohol alcohol)
        {
            return AssetDataPath.GameLog[(int)alcohol.type];
        }

    }
}
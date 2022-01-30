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

        private const int CloseActionCount = 24;       //�A���
        private static readonly int[] GiveUpDrunkValue = { 0, 18, 36, 50 };    //�����̌��E�l

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

        private float magDegree_;          //�����̔{��
        void Start()
        {
            CreateAlcoholButton();

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
                alcoholInfo.SetInfo(AssetDataPath.AlcoholName[i], AssetDataPath.AlcoholDegree[i], AssetDataPath.AlcoholPrice[i], i, alcoholBtnSprite_[i]);
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

        //�����{�^�����������ۂ̏���
        public void PushButton(BaseAlcohol info, Button button)
        {
            //�{�^���̑I����������ׂ̏���
            EventSystem.current.SetSelectedGameObject(null);

            //���b�Z�[�W���X�V����
            gameText_.text = ViewGameMessage(info);
            //���z������Ȃ��ꍇ�͏��������Ȃ�
            if (PlayerInfoManager.instance.moneyValue.Value - info.price < 0)
            {
                return;
            }
            //�ʂ̂��������񂾏ꍇ�̏���
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
                //�}�C�i�X�ɂȂ�����0�ɖ߂�
                if (PlayerInfoManager.instance.stressValue.Value < 0)
                {
                    PlayerInfoManager.instance.stressValue.Value = 0;
                }
            }
            PlayerInfoManager.instance.actionCount.Value++;

            //PlayerInfo�X�V��̏���

            //�L�����N�^�[�̉摜�̐؂�ւ�����
            DrunkCharacterImage();


            //�ޏo�{�^���ȊO�̏��͕ێ�����
            if (info.type != BaseAlcohol.AlcoholType.EXIT)
            {
                PlayerInfoManager.instance.drinkTypeList.Add((int)info.type);
            }
            //�ޏo�{�^���̏�Ԃ�؂�ւ���
            if (useExitButton(button) || PlayerInfoManager.instance.moneyValue.Value < AssetDataPath.AlcoholPrice[(int)BaseAlcohol.AlcoholType.BEER])
            {
                exitButtonFlag_.Value = true;
            }
            //�����I�ɃV�[���J�ڂ��s������
            if (CheckBarClose() || CheckDrunkValue() || info.type == BaseAlcohol.AlcoholType.EXIT)
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
            if (PlayerInfoManager.instance.drunkValue.Value < GiveUpDrunkValue[(int)DrunkMode.MAX])
            {
                return false;
            }
            return true;
        }

        //�������{�^���ɉ����ă��b�Z�[�W��ύX����
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

        //�����l�ɉ����ăL�����N�^�[�̃C���X�g��؂�ւ���
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

        
        //�O�ƈ��񂾂����Ɠ�����ނ�������
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
        //�{�^���������邩�ǂ���
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
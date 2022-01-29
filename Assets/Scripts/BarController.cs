using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
            button.onClick.AddListener(() => { PushButton(alcoholInfo,button); });
        }
    }

    //�����{�^�����������ۂ̏���
    public void PushButton(BaseAlcohol info,Button button)
    {
        PlayerInfoManager.instance.moneyValue.Value -= info.price_;
        PlayerInfoManager.instance.drunkValue.Value += info.alcoholDegree_; ;
        PlayerInfoManager.instance.stressValue.Value -= info.alcoholDegree_; ;
        PlayerInfoManager.instance.currentTime.Value++;
    }
}

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
        //‚¨ğ‚Ì”‚¾‚¯ƒ{ƒ^ƒ“‚ğ¶¬
        for (int i = 0; i < (int)BaseAlcohol.AlcoholType.MAX; i++)
        {
            GameObject obj = Instantiate((GameObject)Resources.Load("UI/AlcoholButton"));
            obj.transform.parent = buttonObj_.transform;
            obj.transform.localScale = Vector3.one;
            //‚¨ğ‚Ìî•ñ‚ğ“o˜^‚·‚é
            BaseAlcohol alcoholInfo = obj.GetComponent<BaseAlcohol>();
            alcoholInfo.SetInfo(AlcoholData.AlcoholName[i], AlcoholData.AlcoholDegree[i], AlcoholData.AlcoholPrice[i], i);
            //‰Ÿ‚µ‚½Û‚Ìˆ—‚ğ“o˜^‚·‚é
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => { PushButton(alcoholInfo,button); });
        }
    }

    //‚¨ğƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚½Û‚Ìˆ—
    public void PushButton(BaseAlcohol info,Button button)
    {
        PlayerInfoManager.instance.moneyValue.Value -= info.price_;
        PlayerInfoManager.instance.drunkValue.Value += info.alcoholDegree_; ;
        PlayerInfoManager.instance.stressValue.Value -= info.alcoholDegree_; ;
        PlayerInfoManager.instance.currentTime.Value++;
    }
}

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
            alcoholInfo.SetInfo("‚¨ğ", 10, 1000, i);
            //‰Ÿ‚µ‚½Û‚Ìˆ—‚ğ“o˜^‚·‚é
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(()=> { PushButton(alcoholInfo); });
        }
        
    }

    public void PushButton(BaseAlcohol info)
    {
        PlayerInfoManager.instance.usingMoney.Value-=info.price_;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
    [SerializeField]
    private Text gameTurnCount_;  //ゲームのターン数(プレイヤーが持つ？)
    [SerializeField]
    private Text emotionvalue_ ;  //ネガポジの値
    [SerializeField]
    private Text usingMoney_;    //使える金額
    [SerializeField]
    private Text jobTime_ ;    //ノルマ(勤務時間)
    [SerializeField]
    private Text alcoholTime_;    //経過時間(入店後の時間)
    [SerializeField]
    private Text overWork_ ;    //残業時間


    // Start is called before the first frame update
    void Start()
    {
        gameTurnCount_.text = PlayerInfoManager.instance.gameTurnCount.ToString();
        emotionvalue_.text = PlayerInfoManager.instance.emotionValue.ToString();
        usingMoney_.text = PlayerInfoManager.instance.usingMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

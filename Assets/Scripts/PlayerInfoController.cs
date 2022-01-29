using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
    [SerializeField]
    private Text gameTurnCount_;  //�Q�[���̃^�[����(�v���C���[�����H)
    [SerializeField]
    private Text emotionvalue_ ;  //�l�K�|�W�̒l
    [SerializeField]
    private Text usingMoney_;    //�g������z
    [SerializeField]
    private Text jobTime_ ;    //�m���}(�Ζ�����)
    [SerializeField]
    private Text alcoholTime_;    //�o�ߎ���(���X��̎���)
    [SerializeField]
    private Text overWork_ ;    //�c�Ǝ���


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDrag()
    {
        //Cube�̈ʒu�����[���h���W����X�N���[�����W�ɕϊ����āAobjectPoint�Ɋi�[
        Vector2 objectPoint = Camera.main.WorldToScreenPoint(transform.position);

        //Cube�̌��݈ʒu(�}�E�X�ʒu)���ApointScreen�Ɋi�[
        Vector2 pointScreen = new Vector2(Input.mousePosition.x,
                                          Input.mousePosition.y);

        //Cube�̌��݈ʒu���A�X�N���[�����W���烏�[���h���W�ɕϊ����āApointWorld�Ɋi�[
        Vector2 pointWorld = Camera.main.ScreenToWorldPoint(pointScreen);

        //Cube�̈ʒu���ApointWorld�ɂ���
        transform.position = pointWorld;
    }
}

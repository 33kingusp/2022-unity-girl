using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Paper : MonoBehaviour
{
    private GameObject clickedDownGameObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // �N���b�N�_�E���� - �N���b�N�����I�u�W�F�N�g���̕ێ�
        if (Input.GetMouseButtonDown(0))
        {
            clickedDownGameObject = null;   // �I�u�W�F�N�g������
            // ���C���΂��ăI�u�W�F�N�g�擾
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            // �I�u�W�F�N�g�擾�ł����ꍇ
            if (hit2d)
            {
                clickedDownGameObject = hit2d.collider.gameObject;  // �I�u�W�F�N�g���i�[
                int Paper = 8;  // Paper layer number
                // Paper�̂݃I�u�W�F�N�g����ێ�����
                if (clickedDownGameObject.layer != Paper)
                {
                    clickedDownGameObject = null;
                }
            }
        }


        // �N���b�N�A�b�v�� - �X�R�A�ϓ�
        if (Input.GetMouseButtonUp(0))
        {
            // �N���b�N�_�E�����̏�񂪕ێ�����Ă��鎞
            if (clickedDownGameObject)
            {
                // �}�E�X���W�擾
                Vector2 touchPosition = Input.mousePosition;
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);
                // ���C���΂��Ď擾�����S�ẴI�u�W�F�N�g��ԗ�
                foreach (RaycastHit2D hit in Physics2D.RaycastAll(worldPoint, Vector2.zero))
                {
                    //�I�u�W�F�N�g�����������Ƃ��̏���
                    if (hit)
                    {
                        // �N���b�N�_�E�����Ɠ����Ȃ珈�����΂�
                        if (hit.collider.gameObject == clickedDownGameObject) { continue; }
                        GameObject clickedGameObject = hit.collider.gameObject; // �I�u�W�F�N�g���擾
                        GameObject Game = GameObject.Find("Game");  // Game �I�u�W�F�N�g�擾
                        Main main = Game.GetComponent<Main>();      // Game�I�u�W�F�N�g��Main�X�N���v�g�擾
                        // �N���b�N�_�E�����̃^�O�ƁA�N���b�N�A�b�v���̃^�O����v����Ƃ�
                        if (clickedGameObject.tag == clickedDownGameObject.tag)
                        {
                            main.changeScore(1);    // �X�R�A�A�b�v
                        }
                        else
                        {
                            main.changeScore(-1);   // �X�R�A�_�E��
                        }
                        Destroy(clickedDownGameObject); // Paper�I�u�W�F�N�g�폜
                        break;
                    }
                }
                clickedDownGameObject = null;   // �N���b�N�_�E��������null��
            }
        }
    }
    // �}�E�X�h���b�O
    void OnMouseDrag()
    {
        //object�̈ʒu�����[���h���W����X�N���[�����W�ɕϊ����āAobjectPoint�Ɋi�[
        Vector2 objectPoint = Camera.main.WorldToScreenPoint(transform.position);

        //object�̌��݈ʒu(�}�E�X�ʒu)���ApointScreen�Ɋi�[
        Vector2 pointScreen = new Vector2(Input.mousePosition.x,
                                          Input.mousePosition.y);

        //object�̌��݈ʒu���A�X�N���[�����W���烏�[���h���W�ɕϊ����āApointWorld�Ɋi�[
        Vector2 pointWorld = Camera.main.ScreenToWorldPoint(pointScreen);

        //object�̈ʒu���ApointWorld�ɂ���
        transform.position = pointWorld;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Works
{
    public class MiniGameManager : MonoBehaviour
    {
        private int score = 0;
        [SerializeField] private GameObject[] _paperInfo; // �����z��
        private GameObject _paper;
        private GameObject _paper2;
        private GameObject clickedDownGameObject;   // �N���b�N�_�E�����I�u�W�F�N�g

        // Start is called before the first frame update
        void Start()
        {
            score = 0;
            // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // �e�L�X�g�̕\�������ւ���
            score_text.text = "Score : 0";
            _paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            _paper.transform.position = new Vector3(7.0f, 1.5f, -1);
            _paper2 = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            _paper2.transform.position = new Vector3(7.0f, -2.5f, -1);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void changeScore(int val)
        {
            score += val;
            // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // �e�L�X�g�̕\�������ւ���
            score_text.text = "Score : " + score;
        }
    }
}
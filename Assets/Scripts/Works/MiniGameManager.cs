using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Logics;

namespace Works
{
    public class MiniGameManager : MonoBehaviour
    {
        private int score = 0;
        [SerializeField] private GameObject[] _paperInfo; // �����z��
        private GameObject _paper;
        private GameObject _paper2;
        private GameObject clickedDownGameObject;   // �N���b�N�_�E�����I�u�W�F�N�g
        private int[] _paperCnt;    // ���̃J�E���g
        private float time; // �o�ߎ��Ԃ��i�[����ϐ�
        private Text timerText;
        private const float timeUp = 30.0f;
        private bool beingMeasured; // �v�����ł��邱�Ƃ�\���ϐ�
        private bool gameOver = false; // �Q�[���I�[�o�[�t���O

        // Start is called before the first frame update
        void Start()
        {
            score = 0;
            // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // �e�L�X�g�̕\�������ւ���
            score_text.text = "Score : 0";
            // ���z�u
            GameObject paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = new Vector3(7.0f, 1.5f, -1);
            paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = new Vector3(7.0f, -2.5f, -1);
            // ���Ԍv��
            time = 0.0f;
            GameObject time_object = GameObject.Find("Time");
            timerText = time_object.GetComponent<Text>();
            timerText.text = "Time : 30";
            beingMeasured = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonUp(0)) { beingMeasured = true; }
            if (!beingMeasured) { return; }
            // ���Ԍv��
            time += Time.deltaTime;
            float text_time = Mathf.Max(0, timeUp - time);
            timerText.text = "Time : " + text_time.ToString("0");
            // ���Ԑ؂ꁨ�V�[���J��
            if(text_time <= 0 && !gameOver)
            {
                gameOver = true;
                GameLogicManager.instance.NextPhase();
            }
        }
        
        // �X�R�A�ϓ� (+ val)
        public void changeScore(int val)
        {
            score += val;
            score = Mathf.Max(0, score);
            // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // �e�L�X�g�̕\�������ւ���
            score_text.text = "Score : " + score;
        }

        // ������
        public void createPaper(Vector3 pos)
        {
            GameObject paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = pos;
        }

        // score GET
        public int getScore()
        {
            return score;
        }
    }
}
using Data;
using Logics;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Audios;

namespace Works
{
    public class MiniGameManager : MonoBehaviour
    {
        private int score = 0;
        [SerializeField] private GameObject[] _paperInfo; // �����z��
        private GameObject clickedDownGameObject;   // �N���b�N�_�E�����I�u�W�F�N�g
        private int[] _paperCnt;    // ���̃J�E���g
        private float time;         // �o�ߎ��Ԃ��i�[����ϐ�
        private float clockTime;    // �o�ߎ��Ԃ��i�[����ϐ�(���v�p)
        private float stressTime;   // �o�ߎ��Ԃ��i�[����ϐ�(�X�g���X�p)
        private Text timerText;
        private const float timeUp = 30.0f;
        private bool beingMeasured;     // �v�����ł��邱�Ƃ�\���ϐ�
        private bool gameOver = false;  // �Q�[���I�[�o�[�t���O
        private Vector3 defaultPos;     // �������W
        private Vector3 defaultPos2;    // �������W2
        private const int red = 0;      // ��
        private const int green = 1;    // ��
        private const int blue = 2;     // ��
        private const int black = 3;    // ��
        private const int white = 4;    // ��
        private const int purple = 5;   // �p�[�v��
        private const int shredder = 6; // �V�����b�_�[
        // �V�����b�_�[�֘A
        private float shredderTime = 0.0f; // �V�����b�_�[���Ԍv��
        private bool shredderAnimF = false; // �V�����b�_�[�A�j���[�V�����t���O
        private const float shredderAnimTime = 1.0f;  // �V�����b�_�[�A�j���[�V��������
        private GameObject shredder_child;  // �V�����b�_�[�q�I�u�W�F�N�g
        private GameObject shredder_child2; // �V�����b�_�[�q�I�u�W�F�N�g2

        [SerializeField] private AudioClip _bgm = default;
        [SerializeField] private AudioClip _putDownPaper = default;
        [SerializeField] private AudioClip _shred = default;

        // Start is called before the first frame update
        void Start()
        {
            defaultPos = new Vector3(4.0f, 0.5f, -1.0f);
            defaultPos2 = new Vector3(4.0f, -3.0f, -1.0f);
            score = 0;
            // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // �e�L�X�g�̕\�������ւ���
            score_text.text = "Score : 0";
            // ���z�u
            GameObject paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = defaultPos;
            paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = defaultPos2;
            // ���Ԍv��
            time = stressTime = clockTime = 0.0f;
            GameObject time_object = GameObject.Find("Time");
            timerText = time_object.GetComponent<Text>();
            timerText.text = "Time : " + timeUp;
            beingMeasured = false;
            // ������������
            _paperCnt = new int[_paperInfo.Length];
            for (int i = 0; i < _paperInfo.Length; i++) _paperCnt[i] = 0;
            // �V�����b�_�[�֘A
            GameObject shredder_obj = GameObject.Find("tray_shredder");
            shredder_child = shredder_obj.transform.GetChild(0).gameObject;
            shredder_child2 = shredder_obj.transform.GetChild(1).gameObject;

            AudioManager.Instance.PlayBGM(_bgm);
        }

        // Update is called once per frame
        void Update()
        {
            // �V�����b�_�[�v�����Ԃ��A�j���[�V�������Ԃ������Ȃ��ꍇ�A�A�j���[�V�������s��
            if (shredderAnimF)
            {
                shredderTime -= Time.deltaTime;
                // �V�����b�_�[�v�����Ԃ��A�j���[�V�������Ԃ̔����ȉ��ɂȂ�ƁA2�̉摜���\����
                if (shredderTime <= shredderAnimTime / 2)
                {
                    shredder_child.SetActive(false);
                }
                // �V�����b�_�[�v�����Ԃ��A�j���[�V�������Ԉȉ��ɂȂ�ƁA1�̉摜���\����
                if (shredderTime <= 0.0f)
                {
                    shredder_child2.SetActive(false);
                    shredderAnimF = false;
                }
            }
            if (Input.GetMouseButtonDown(0)) { beingMeasured = true; }
            if (gameOver) { return; }
            if (!beingMeasured) { return; }
            // ���Ԍv��
            time += Time.deltaTime;
            clockTime += Time.deltaTime;
            stressTime += Time.deltaTime;
            // 1�b���ƂɃX�g���X�l��1�グ��
            if( stressTime >= 1.0f)
            {
                PlayerInfoManager.instance.stressValue.Value++;
                stressTime = 0.0f;
            }
            // (�Q�[�����ԁ��Ɩ�����)�b���ƂɎ��Ԃ�1�i�߂�
            if( clockTime >= timeUp / 9 )
            {
                PlayerInfoManager.instance.actionCount.Value++;
                clockTime = 0.0f;
            }
            float text_time = Mathf.Max(0, timeUp - time);
            timerText.text = "Time : " + text_time.ToString("0");
            // ���Ԑ؂ꁨ�V�[���J��
            if(text_time <= 0 && !gameOver)
            {
                gameOver = true;
                GameLogicManager.instance.SetGameScore(score);
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

        // gameOver GET
        public bool getGameOver()
        {
            return gameOver;
        }

        public void paperAnim(string tagName)
        {
            int paperChild1 = 1;        // �q�I�u�W�F�N�g1��\�����鎞�̒l
            int paperChild2 = 3;        // �q�I�u�W�F�N�g2��\�����鎞�̒l
            int paperChild3 = 5;        // �q�I�u�W�F�N�g3��\�����鎞�̒l
            int paperChildNum1 = 1;     // �q�I�u�W�F�N�g1�̊K�w
            int paperChildNum2 = 2;     // �q�I�u�W�F�N�g2�̊K�w 
            int paperChildNum3 = 3;     // �q�I�u�W�F�N�g3�̊K�w
            switch (tagName)
            {
                case "red":
                    _paperCnt[red]++;
                    if (_paperCnt[red] == paperChild1)
                    {
                        GameObject tray = GameObject.Find("tray_red");
                        GameObject child = tray.transform.GetChild(paperChildNum1).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[red] == paperChild2)
                    {
                        GameObject tray = GameObject.Find("tray_red");
                        GameObject child = tray.transform.GetChild(paperChildNum2).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[red] == paperChild3)
                    {
                        GameObject tray = GameObject.Find("tray_red");
                        GameObject child = tray.transform.GetChild(paperChildNum3).gameObject;
                        child.SetActive(true);
                    }
                    AudioManager.Instance.PlaySE(_putDownPaper);
                    break;
                case "green":
                    _paperCnt[green]++;
                    if (_paperCnt[green] == paperChild1)
                    {
                        GameObject tray = GameObject.Find("tray_" + "green");
                        GameObject child = tray.transform.GetChild(paperChildNum1).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[green] == paperChild2)
                    {
                        GameObject tray = GameObject.Find("tray_" + "green");
                        GameObject child = tray.transform.GetChild(paperChildNum2).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[green] == paperChild3)
                    {
                        GameObject tray = GameObject.Find("tray_" + "green");
                        GameObject child = tray.transform.GetChild(paperChildNum3).gameObject;
                        child.SetActive(true);
                    }
                    AudioManager.Instance.PlaySE(_putDownPaper);
                    break;
                case "blue":
                    _paperCnt[blue]++;
                    if (_paperCnt[blue] == paperChild1)
                    {
                        GameObject tray = GameObject.Find("tray_" + "blue");
                        GameObject child = tray.transform.GetChild(paperChildNum1).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[blue] == paperChild2)
                    {
                        GameObject tray = GameObject.Find("tray_" + "blue");
                        GameObject child = tray.transform.GetChild(paperChildNum2).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[blue] == paperChild3)
                    {
                        GameObject tray = GameObject.Find("tray_" + "blue");
                        GameObject child = tray.transform.GetChild(paperChildNum3).gameObject;
                        child.SetActive(true);
                    }
                    AudioManager.Instance.PlaySE(_putDownPaper);
                    break;
                case "black":
                    _paperCnt[black]++;
                    if (_paperCnt[black] == paperChild1)
                    {
                        GameObject tray = GameObject.Find("tray_" + "black");
                        GameObject child = tray.transform.GetChild(paperChildNum1).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[black] == paperChild2)
                    {
                        GameObject tray = GameObject.Find("tray_" + "black");
                        GameObject child = tray.transform.GetChild(paperChildNum2).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[black] == paperChild3)
                    {
                        GameObject tray = GameObject.Find("tray_" + "black");
                        GameObject child = tray.transform.GetChild(paperChildNum3).gameObject;
                        child.SetActive(true);
                    }
                    AudioManager.Instance.PlaySE(_putDownPaper);
                    break;
                case "white":
                    _paperCnt[white]++;
                    if (_paperCnt[white] == paperChild1)
                    {
                        GameObject tray = GameObject.Find("tray_" + "white");
                        GameObject child = tray.transform.GetChild(paperChildNum1).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[white] == paperChild2)
                    {
                        GameObject tray = GameObject.Find("tray_" + "white");
                        GameObject child = tray.transform.GetChild(paperChildNum2).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[white] == paperChild3)
                    {
                        GameObject tray = GameObject.Find("tray_" + "white");
                        GameObject child = tray.transform.GetChild(paperChildNum3).gameObject;
                        child.SetActive(true);
                    }
                    AudioManager.Instance.PlaySE(_putDownPaper);
                    break;
                case "purple":
                    _paperCnt[purple]++;
                    if (_paperCnt[purple] == paperChild1)
                    {
                        GameObject tray = GameObject.Find("tray_" + "purple");
                        GameObject child = tray.transform.GetChild(paperChildNum1).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[purple] == paperChild2)
                    {
                        GameObject tray = GameObject.Find("tray_" + "purple");
                        GameObject child = tray.transform.GetChild(paperChildNum2).gameObject;
                        child.SetActive(true);
                    }
                    if (_paperCnt[purple] == paperChild3)
                    {
                        GameObject tray = GameObject.Find("tray_" + "purple");
                        GameObject child = tray.transform.GetChild(paperChildNum3).gameObject;
                        child.SetActive(true);
                    }
                    AudioManager.Instance.PlaySE(_putDownPaper);
                    break;
                case "shredder":
                    shredder_child.SetActive(true);     // �V�����b�_�[�q�I�u�W�F�N�g�\��
                    shredder_child2.SetActive(true);    // �V�����b�_�[�q�I�u�W�F�N�g2�\��
                    shredderAnimF = true;       // �A�j���[�V�����J�n
                    shredderTime = shredderAnimTime;    // �V�����b�_�[�v�����ԏ�����
                    AudioManager.Instance.PlaySE(_shred);
                    break;
            }
        }
    }
}
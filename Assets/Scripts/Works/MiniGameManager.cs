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
        [SerializeField] private GameObject[] _paperInfo; // 紙情報配列
        private GameObject _paper;
        private GameObject _paper2;
        private GameObject clickedDownGameObject;   // クリックダウン時オブジェクト
        private int[] _paperCnt;    // 紙のカウント
        private float time; // 経過時間を格納する変数
        private Text timerText;
        private const float timeUp = 30.0f;

        // Start is called before the first frame update
        void Start()
        {
            score = 0;
            // オブジェクトからTextコンポーネントを取得
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // テキストの表示を入れ替える
            score_text.text = "Score : 0";
            // 紙配置
            GameObject paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = new Vector3(7.0f, 1.5f, -1);
            paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = new Vector3(7.0f, -2.5f, -1);
            // 時間計測
            time = 0.0f;
            GameObject time_object = GameObject.Find("Time");
            timerText = time_object.GetComponent<Text>();
            timerText.text = "Time : 0";
        }

        // Update is called once per frame
        void Update()
        {
            // 時間計測
            time += Time.deltaTime;
            float text_time = Mathf.Max(0, timeUp - time);
            timerText.text = "Time : " + text_time.ToString("0");
            // 時間切れ→シーン遷移
            if(timerText < 0)
            {
                GameLogicManager.instance.NextPhase();
            }
        }

        public void changeScore(int val)
        {
            score += val;
            score = Mathf.Max(0, score);
            // オブジェクトからTextコンポーネントを取得
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // テキストの表示を入れ替える
            score_text.text = "Score : " + score;
        }

        public void createPaper(Vector3 pos)
        {
            GameObject paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = pos;
        }
    }
}
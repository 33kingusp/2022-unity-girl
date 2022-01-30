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
        private bool beingMeasured; // 計測中であることを表す変数
        private bool gameOver = false; // ゲームオーバーフラグ

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
            timerText.text = "Time : 30";
            beingMeasured = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonUp(0)) { beingMeasured = true; }
            if (!beingMeasured) { return; }
            // 時間計測
            time += Time.deltaTime;
            float text_time = Mathf.Max(0, timeUp - time);
            timerText.text = "Time : " + text_time.ToString("0");
            // 時間切れ→シーン遷移
            if(text_time <= 0 && !gameOver)
            {
                gameOver = true;
                GameLogicManager.instance.NextPhase();
            }
        }
        
        // スコア変動 (+ val)
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

        // 紙生成
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
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
        private GameObject clickedDownGameObject;   // クリックダウン時オブジェクト
        private int[] _paperCnt;    // 紙のカウント
        private float time;         // 経過時間を格納する変数
        private Text timerText;
        private const float timeUp = 30.0f;
        private bool beingMeasured;     // 計測中であることを表す変数
        private bool gameOver = false;  // ゲームオーバーフラグ
        private Vector3 defaultPos;     // 初期座標
        private Vector3 defaultPos2;    // 初期座標2
        private const int red = 0;      // 赤
        private const int green = 1;    // 緑
        private const int blue = 2;     // 青
        private const int black = 3;    // 黒
        private const int white = 4;    // 白
        private const int purple = 5;   // パープル
        private const int shredder = 6; // シュレッダー
        // シュレッダー関連
        private float shredderTime = 0.0f; // シュレッダー時間計測
        private bool shredderAnimF = false; // シュレッダーアニメーションフラグ
        private const float shredderAnimTime = 1.0f;  // シュレッダーアニメーション時間
        private GameObject shredder_child;  // シュレッダー子オブジェクト
        private GameObject shredder_child2; // シュレッダー子オブジェクト2

        // Start is called before the first frame update
        void Start()
        {
            defaultPos = new Vector3(4.5f, 0.5f, -1.0f);
            defaultPos2 = new Vector3(4.5f, -3.0f, -1.0f);
            score = 0;
            // オブジェクトからTextコンポーネントを取得
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // テキストの表示を入れ替える
            score_text.text = "Score : 0";
            // 紙配置
            GameObject paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = defaultPos;
            paper = Instantiate(_paperInfo[(int)Random.Range(0, _paperInfo.Length)]);
            paper.transform.position = defaultPos2;
            // 時間計測
            time = 0.0f;
            GameObject time_object = GameObject.Find("Time");
            timerText = time_object.GetComponent<Text>();
            timerText.text = "Time : " + timeUp;
            beingMeasured = false;
            // 紙枚数初期化
            _paperCnt = new int[_paperInfo.Length];
            for (int i = 0; i < _paperInfo.Length; i++) _paperCnt[i] = 0;
            // シュレッダー関連
            GameObject shredder_obj = GameObject.Find("tray_shredder");
            shredder_child = shredder_obj.transform.GetChild(0).gameObject;
            shredder_child2 = shredder_obj.transform.GetChild(1).gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            // シュレッダー計測時間がアニメーション時間よりも少ない場合、アニメーションを行う
            if (shredderAnimF)
            {
                shredderTime -= Time.deltaTime;
                // シュレッダー計測時間がアニメーション時間の半分以下になると、2の画像を非表示に
                if (shredderTime <= shredderAnimTime / 2)
                {
                    shredder_child.SetActive(false);
                }
                // シュレッダー計測時間がアニメーション時間以下になると、1の画像を非表示に
                if (shredderTime <= 0.0f)
                {
                    shredder_child2.SetActive(false);
                    shredderAnimF = false;
                }
            }
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

        // gameOver GET
        public bool getGameOver()
        {
            return gameOver;
        }

        public void paperAnim(string tagName)
        {
            int paperChild1 = 1;        // 子オブジェクト1を表示する時の値
            int paperChild2 = 3;        // 子オブジェクト2を表示する時の値
            int paperChild3 = 5;        // 子オブジェクト3を表示する時の値
            int paperChildNum1 = 1;     // 子オブジェクト1の階層
            int paperChildNum2 = 2;     // 子オブジェクト2の階層 
            int paperChildNum3 = 3;     // 子オブジェクト3の階層
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
                    break;
                case "shredder":
                    shredder_child.SetActive(true);     // シュレッダー子オブジェクト表示
                    shredder_child2.SetActive(true);    // シュレッダー子オブジェクト2表示
                    shredderAnimF = true;       // アニメーション開始
                    shredderTime = shredderAnimTime;    // シュレッダー計測時間初期化
                    break;
            }
        }
    }
}
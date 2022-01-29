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
        [SerializeField] private GameObject[] _paperInfo; // 紙情報配列
        private GameObject _paper;
        private GameObject _paper2;
        private GameObject clickedDownGameObject;   // クリックダウン時オブジェクト

        // Start is called before the first frame update
        void Start()
        {
            score = 0;
            // オブジェクトからTextコンポーネントを取得
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // テキストの表示を入れ替える
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
            // オブジェクトからTextコンポーネントを取得
            GameObject score_object = GameObject.Find("Score");
            Text score_text = score_object.GetComponent<Text>();
            // テキストの表示を入れ替える
            score_text.text = "Score : " + score;
        }
    }
}
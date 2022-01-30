using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Works
{
    public class Paper : MonoBehaviour
    {
        private GameObject clickedDownGameObject;   // クリックダウン時オブジェクト
        [SerializeField] private GameObject[] paperInfo; // 紙情報配列
        private Vector3 defaultPos = new Vector3(4.5f,  0.5f, -1.0f);
        private Vector3 defaultPos2 = new Vector3(4.5f, -3.0f, -1.0f);
        private Vector3 tmpPos = new Vector3(7.0f, 1.5f, -1.0f);           // 座標保存用
        private MiniGameManager main;
        private bool gameOver = false;

        // Start is called before the first frame update
        void Start()
        {
            tmpPos = defaultPos;
            GameObject Game = GameObject.Find("Game");  // Game オブジェクト取得
            main = Game.GetComponent<MiniGameManager>();      // GameオブジェクトのMainスクリプト取得
        }

        // Update is called once per frame
        void Update()
        {
            gameOver = main.getGameOver();
            if(gameOver) { return; }    // ゲームオーバーなら処理を終える
            // クリックダウン時 - クリックしたオブジェクト情報の保持
            if (Input.GetMouseButtonDown(0))
            {
                clickedDownGameObject = null;   // オブジェクト初期化
                // レイを飛ばしてオブジェクト取得
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
                // オブジェクト取得できた場合
                if (hit2d)
                {
                    if(hit2d.collider.gameObject == this.gameObject)
                    {
                        clickedDownGameObject = hit2d.collider.gameObject;  // オブジェクト情報格納
                        if (defaultPos == clickedDownGameObject.transform.position
                         || defaultPos2 == clickedDownGameObject.transform.position)
                            tmpPos = clickedDownGameObject.transform.position;
                    }
                }
            }


            // クリックアップ時 - スコア変動
            if (Input.GetMouseButtonUp(0))
            {
                // クリックダウン時の情報が保持されている時
                if (clickedDownGameObject)
                {
                    // マウス座標取得
                    Vector2 touchPosition = Input.mousePosition;
                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);
                    // レイを飛ばして取得した全てのオブジェクトを網羅
                    foreach (RaycastHit2D hit in Physics2D.RaycastAll(worldPoint, Vector2.zero))
                    {
                        //オブジェクトが見つかったときの処理
                        if (hit)
                        {
                            // クリックダウン時と同じオブジェクトなら処理を飛ばす
                            if (hit.collider.gameObject == clickedDownGameObject) { continue; }
                            // レイヤーがトレー以外なら処理を飛ばす
                            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Tray")) { continue; }
                            GameObject clickedGameObject = hit.collider.gameObject; // オブジェクト情報取得

                            // クリックダウン時のタグと、クリックアップ時のタグが一致するとき
                            if (clickedGameObject.tag == clickedDownGameObject.tag)
                            {
                                main.changeScore(1);    // スコアアップ
                            }
                            // 一致しない時
                            else
                            {
                                main.changeScore(-1);   // スコアダウン
                            }
                            main.paperAnim(clickedGameObject.tag);    // スコアアップ
                            main.createPaper(tmpPos);   // 新しい紙を生成
                            tmpPos = defaultPos;
                            Destroy(clickedDownGameObject); // Paperオブジェクト削除
                            break;
                        }
                    }
                    clickedDownGameObject = null;   // クリックダウン時情報をnullに
                }
            }
        }
        // マウスドラッグ
        void OnMouseDrag()
        {
            //objectの位置をワールド座標からスクリーン座標に変換して、objectPointに格納
            Vector2 objectPoint = Camera.main.WorldToScreenPoint(transform.position);

            //objectの現在位置(マウス位置)を、pointScreenに格納
            Vector2 pointScreen = new Vector2(Input.mousePosition.x,
                                              Input.mousePosition.y);

            //objectの現在位置を、スクリーン座標からワールド座標に変換して、pointWorldに格納
            Vector2 pointWorld = Camera.main.ScreenToWorldPoint(pointScreen);

            //objectの位置を、pointWorldにする
            transform.position = pointWorld;
        }
    }
}
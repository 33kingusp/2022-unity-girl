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
                clickedDownGameObject = hit2d.collider.gameObject;  // オブジェクト情報格納
                int Paper = 8;  // Paper layer number
                // Paperのみオブジェクト情報を保持する
                if (clickedDownGameObject.layer != Paper)
                {
                    clickedDownGameObject = null;
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
                        // クリックダウン時と同じなら処理を飛ばす
                        if (hit.collider.gameObject == clickedDownGameObject) { continue; }
                        GameObject clickedGameObject = hit.collider.gameObject; // オブジェクト情報取得
                        GameObject Game = GameObject.Find("Game");  // Game オブジェクト取得
                        Main main = Game.GetComponent<Main>();      // GameオブジェクトのMainスクリプト取得
                        // クリックダウン時のタグと、クリックアップ時のタグが一致するとき
                        if (clickedGameObject.tag == clickedDownGameObject.tag)
                        {
                            main.changeScore(1);    // スコアアップ
                        }
                        else
                        {
                            main.changeScore(-1);   // スコアダウン
                        }
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

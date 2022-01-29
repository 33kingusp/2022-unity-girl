using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDrag()
    {
        //Cubeの位置をワールド座標からスクリーン座標に変換して、objectPointに格納
        Vector2 objectPoint = Camera.main.WorldToScreenPoint(transform.position);

        //Cubeの現在位置(マウス位置)を、pointScreenに格納
        Vector2 pointScreen = new Vector2(Input.mousePosition.x,
                                          Input.mousePosition.y);

        //Cubeの現在位置を、スクリーン座標からワールド座標に変換して、pointWorldに格納
        Vector2 pointWorld = Camera.main.ScreenToWorldPoint(pointScreen);

        //Cubeの位置を、pointWorldにする
        transform.position = pointWorld;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class AssetDataPath
    {
        public static readonly string[] AlcoholName = { "水", "ビール", "ワイン", "日本酒", "カクテル", "退出" };
        public static readonly int[] AlcoholDegree = { 0, 5, 12, 15, 20, 0 };
        public static readonly int[] AlcoholPrice = { 0, 500, 600, 800, 800, 0 };

        public static readonly string[] AlcoholBtnSprite =
            {
                "btn_water.png",
                "btn_beer.png",
                "btn_wine.png",
                "btn_sake.png",
                "btn_cocktail.png",
                "btn_exit.png",
            };
        public static readonly string[] CharacterViewSprite =
            {
                "bar_girl1_normal.png",
                "bar_girl2_normal.png",
                "bar_girl3_normal.png",
            };

        public static readonly string BtnNotPush = "btn_bg_blue_forbidden.png";
        public static readonly string BtnPush = "btn_bg_blue.png";

        //Resourceフォルダからの呼出し
        public static readonly string PlayerInfo = "UI/PlayerInfo";

        public static readonly string[] GameLog ={
            //"今日もがんばるよ〜♪",
            //"お仕事ちゃんとできた！",
            //"失敗しちゃったぁ......",
            //"捨てちゃおーっと♪",
            "お水お水ぅ！",
            "ビールグビグビー！",
            "ワイン最高！！",
            "日本酒はいいねぇ〜",
            "カクテルは大人の味♡",
            "今日も疲れた〜〜",
            //"お酒おいしいなぁ",
            //"もう.......だめ....."
        };
    }
}
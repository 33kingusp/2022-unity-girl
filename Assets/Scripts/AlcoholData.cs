using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcoholData
{
    public static readonly string[] AlcoholName = { "水","ビール", "ワイン", "日本酒", "カクテル","退出"};
    public static readonly int[] AlcoholDegree = {0, 5,12,15,20,0};
    public static readonly int[] AlcoholPrice = {0,500,600,800,800,0};

    public static readonly string[] SpritePath = {
        "btn_water.png",
        "btn_beer.png",
        "btn_wine.png",
        "btn_sake.png",
        "btn_cocktail.png",
        "btn_exit.png",
    };
}

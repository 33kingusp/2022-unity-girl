using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAlcohol : MonoBehaviour
{
    public enum AlcoholType
    {
        BEER,     //ビール
        WINE,     //ワイン
        SHOCHU,   //焼酎
        SAKE,     //日本酒
        MAX
    }

    [SerializeField]
    private Text textData_;
    public string alcoholName_;
    public int alcoholDegree_;   //アルコール度数
    public int capacity_;       //容量
    public int price_;          //価格
    public AlcoholType type_;

    public void SetInfo(string name,int degree,int price,int type)
    {
        alcoholName_ = name;
        textData_.text = name;
        alcoholDegree_ = degree;
        price_ = price;
        type_ = (AlcoholType)type;
    }
}

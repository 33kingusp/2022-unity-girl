using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Utilities;

namespace Data
{
    public class BaseAlcohol : MonoBehaviour
    {
        public enum AlcoholType
        {
            WATER,
            BEER,     //ビール
            WINE,     //ワイン
            SHOCHU,   //焼酎
            SAKE,     //日本酒
            EXIT,      //退出
            MAX
        }

        [SerializeField]
        private Text textData_;
        [SerializeField]
        private Image icon_;
        private string alcoholName_;
        private int alcoholDegree_;   //アルコール度数
        private int price_;          //価格
        private AlcoholType type_;

        public string alcoholName
        {
            get
            {
                return alcoholName_;
            }
            set
            {
                alcoholName_ = value;
            }
        }
        public int alcoholDegree
        {
            get
            {
                return alcoholDegree_;
            }
            set
            {
                alcoholDegree_ = value;
            }
        }
        public int price
        {
            get
            {
                return price_;
            }
            set
            {
                price_ = value;
            }
        }
        public AlcoholType type
        {
            get
            {
                return type_;
            }
            set
            {
                type_ = value;
            }
        }
        public void SetInfo(string name, int degree, int price, int type)
        {
            alcoholName_ = name;
            textData_.text = name;
            alcoholDegree_ = degree;
            price_ = price;
            type_ = (AlcoholType)type;
            icon_.sprite = LoadSpriteData.LoadSprite(AssetDataPath.AlcoholBtnSprite[type]);

        }
    }
}
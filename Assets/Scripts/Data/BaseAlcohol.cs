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
        public string alcoholName_;
        public int alcoholDegree_;   //アルコール度数
        public int capacity_;       //容量
        public int price_;          //価格
        public AlcoholType type_;

        public void SetInfo(string name, int degree, int price, int type)
        {
            alcoholName_ = name;
            textData_.text = name;
            alcoholDegree_ = degree;
            price_ = price;
            type_ = (AlcoholType)type;
            string pathName = Application.dataPath + "/Textures/" + AssetDataPath.SpritePath[type];
            icon_.sprite = LoadSpriteData.LoadSprite(pathName);

        }
    }
}
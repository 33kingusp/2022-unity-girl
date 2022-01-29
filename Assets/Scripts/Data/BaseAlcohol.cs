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
            BEER,     //�r�[��
            WINE,     //���C��
            SHOCHU,   //�Ē�
            SAKE,     //���{��
            EXIT,      //�ޏo
            MAX
        }



        [SerializeField]
        private Text textData_;
        [SerializeField]
        private Image icon_;
        public string alcoholName_;
        public int alcoholDegree_;   //�A���R�[���x��
        public int capacity_;       //�e��
        public int price_;          //���i
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
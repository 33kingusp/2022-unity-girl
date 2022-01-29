using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAlcohol : MonoBehaviour
{
    public enum AlcoholType
    {
        BEER,     //�r�[��
        WINE,     //���C��
        SHOCHU,   //�Ē�
        SAKE,     //���{��
        MAX
    }

    [SerializeField]
    private Text textData_;
    public string alcoholName_;
    public int alcoholDegree_;   //�A���R�[���x��
    public int capacity_;       //�e��
    public int price_;          //���i
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

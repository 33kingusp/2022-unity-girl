using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public string alcoholName_;
    public int alcoholDegree_;   //�A���R�[���x��
    public int capacity_;       //�e��
    public int price_;          //���i
    public AlcoholType type_;

    public void SetInfo(string name,int degree,int price,int type)
    {
        alcoholName_ = name;
        alcoholDegree_ = degree;
        price_ = price;
        type_ = (AlcoholType)type;
    }
}

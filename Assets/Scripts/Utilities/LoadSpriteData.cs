using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


namespace Utilities
{
    public class LoadSpriteData : MonoBehaviour
    {

        public static string TexturesFile = Application.dataPath + "/Textures/";
        //public static Sprite LoadSprite(string path)
        //{
        //    try
        //    {
        //        var rawData = File.ReadAllBytes(TexturesFile+path);
        //        Texture2D texture2D = new Texture2D(0, 0);
        //        texture2D.LoadImage(rawData);
        //        var sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        //        return sprite;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e);
        //        return null;
        //    }
        //}
    }
}
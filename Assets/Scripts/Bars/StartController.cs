using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Bars
{
    public class StartController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //Debug�p(�Q�[���J�n����Setup�͌Ă΂��)
            PlayerInfoManager.instance.SetUp();
        }
    }
}
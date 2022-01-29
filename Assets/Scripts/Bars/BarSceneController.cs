using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using UniRx;

namespace Bars
{
    public class BarSceneController : MonoBehaviour
    {
        [SerializeField] private AudioClip _bgm = default;

        private void Start()
        {
            AudioManager.Instance.PlayBGM(_bgm);
        }
    }
}
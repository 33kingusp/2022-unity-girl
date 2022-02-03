using UnityEngine;
using Utilities.Audios;

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
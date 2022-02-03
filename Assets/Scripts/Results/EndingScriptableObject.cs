using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Results
{
    [CreateAssetMenu(fileName = "Ending", menuName = "Results/EndingScriptableObject")]
    public class EndingScriptableObject : ScriptableObject
    {
        [SerializeField] private int _id = default;
        [SerializeField] private string _endName = default;
        [SerializeField] private Sprite[] _endCardsprites = default;
        [SerializeField] private AudioClip _endSound = default;
        [SerializeField] private string[] _messages = default;

        public int Id { get { return _id; } }
        public string Name { get { return _endName; } }
        public Sprite[] EndCardsprites { get { return _endCardsprites; } }
        public AudioClip EndSound { get { return _endSound; } }
        public string[] Messages { get { return _messages; } } 
    }
}
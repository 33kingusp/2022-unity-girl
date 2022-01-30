using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Titles
{
    public class PressToStartObject : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _moveCurve = default;
        [SerializeField] private float _moveRange = 100f;
        [SerializeField] private float _speed = 0.5f;

        private IEnumerator Start()
        {
            var startPos = transform.position;
            float t = 0;

            while (true)
            {
                var pos = transform.position;
                pos.y = startPos.y + _moveCurve.Evaluate(t) * _moveRange;
                transform.position = pos;
                yield return null;
                t = (t + Time.deltaTime * _speed) % 1;
            }
        }
    }
}
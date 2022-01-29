using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Utilities;
using Data;

namespace Logics
{
    public class GameLogicManager : SingletonMonoBehaviour<GameLogicManager>
    {
        private const int AttendanceTime = 9; // 出社時間
        private const int LeaveTime = 18; // 退社時間
        private const float AlcoholDecompositionPerHour = 3.75f; // 1時間当たりのアルコール分解能

        public int CurrentTurn { private set; get; } = 0;
        public GamePhase CurrentPhase { private set; get; } = GamePhase.Home;

        /// <summary>
        /// フェイズの遷移を通知
        /// </summary>
        public IObservable<GamePhase> OnChangeCurrentPhaseAsObservable { get { return _onChangeCurrentPhase; } }
        private Subject<GamePhase> _onChangeCurrentPhase = default;

        private void OnEnable()
        {
            _onChangeCurrentPhase = new Subject<GamePhase>();
        }

        private void OnDisable()
        {
            _onChangeCurrentPhase.Dispose();
        }

        public void StartGame()
        {
            CurrentTurn = 0;
            CurrentPhase = GamePhase.Work;
            NextTurn();
        }

        /// <summary>
        /// 次のターンへ遷移
        /// </summary>
        public void NextTurn()
        {
            CurrentTurn++;
            CurrentPhase = GamePhase.Work;
            OnBeginWorkPhase();
            _onChangeCurrentPhase.OnNext(CurrentPhase);
            AudioManager.Instance.StopBGM(1.5f);
            SceneTransitionManager.Instance.LoadSceneWithFade("MiniGameScene", 2f);
        }

        /// <summary>
        /// 次のフェイズへ遷移
        /// </summary>
        public void NextPhase()
        {
            if (CurrentPhase == GamePhase.Work)
            {
                CurrentPhase = GamePhase.Bar;
                OnBeginBarPhase();
                _onChangeCurrentPhase.OnNext(CurrentPhase);
                AudioManager.Instance.StopBGM(1.5f);
                SceneTransitionManager.Instance.LoadSceneWithFade("BarScene", 2f);
            }
            else if (CurrentPhase == GamePhase.Bar)
            {
                CurrentPhase = GamePhase.Home;
                OnBeginHomePhase();
                _onChangeCurrentPhase.OnNext(CurrentPhase);
                AudioManager.Instance.StopBGM(1.5f);
                SceneTransitionManager.Instance.LoadSceneWithFade("HomeScene", 2f);
            }
            else if (CurrentPhase == GamePhase.Home)
            {
                NextTurn();
            }
        }

        /// <summary>
        /// 仕事フェイズの開始時処理
        /// </summary>
        private void OnBeginWorkPhase()
        {
            // 時間を出社時間に設定
            PlayerInfoManager.instance.currentTime.Value = AttendanceTime;
        }

        /// <summary>
        /// バーフェイズの開始時処理
        /// </summary>
        private void OnBeginBarPhase()
        {
            // 時間を退社時間に設定
            PlayerInfoManager.instance.currentTime.Value = LeaveTime;
        }

        /// <summary>
        /// 家フェイズの開始時処理
        /// </summary>
        private void OnBeginHomePhase()
        {
            // 早めに帰った際に得られる時間
            int addSleepTime = 24 - PlayerInfoManager.instance.currentTime.Value;
            // 出社までの時間
            int sleepTime = AttendanceTime + addSleepTime;
            // アルコールは1時間あたり3.75%抜ける
            float ad = AlcoholDecompositionPerHour * sleepTime;
            float drunk = Mathf.Max(PlayerInfoManager.instance.drunkValue.Value - ad, 0);
            PlayerInfoManager.instance.drunkValue.Value = (int)drunk;
        }
    }
}
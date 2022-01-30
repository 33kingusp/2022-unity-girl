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
        private const int AttendanceTime = 9; // �o�Ў���
        private const int LeaveTime = 18; // �ގЎ���
        private const float AlcoholDecompositionPerHour = 3.75f; // 1���ԓ�����̃A���R�[������\

        public int CurrentTurn { private set; get; } = 0;
        public GamePhase CurrentPhase { private set; get; } = GamePhase.Home;
        public int CurrentEndingId { private set; get; } = 0;
        public int CurrentGameScore { private set; get; } = 0;

        /// <summary>
        /// �t�F�C�Y�̑J�ڂ�ʒm
        /// </summary>
        public IObservable<GamePhase> OnChangeCurrentPhaseAsObservable { get { return _onChangeCurrentPhase; } }
        private Subject<GamePhase> _onChangeCurrentPhase = default;

        private void OnEnable()
        {
            _onChangeCurrentPhase = new Subject<GamePhase>();

            this.ObserveEveryValueChanged(_ => CurrentTurn)
                .Where(_ => PlayerInfoManager.instance != null)
                .Subscribe(_ => PlayerInfoManager.instance.gameTurnCount.Value = CurrentTurn)
                .AddTo(gameObject);
        }

        private void OnDisable()
        {
            _onChangeCurrentPhase.Dispose();
        }

        public void StartGame()
        {
            SceneTransitionManager.Instance.OnFinishedFadeInAsObservable
                .First()
                .Subscribe(_ => PlayerInfoManager.instance.SetUp())
                .AddTo(gameObject);

            CurrentTurn = 0;
            CurrentEndingId = 0;
            CurrentPhase = GamePhase.Work;
            NextTurn();
        }

        public void EndGame()
        {
            SceneTransitionManager.Instance.LoadSceneWithFade("TitleScene", 3f);
            SceneTransitionManager.Instance.OnFinishedFadeInAsObservable
                .Where(scene => scene == "TitleScene")
                .First()
                .Subscribe(_ => 
                {
                    CurrentTurn = 0;
                    CurrentEndingId = 0;
                    CurrentPhase = GamePhase.Work;
                }
                ).AddTo(gameObject);            
        }

        public void SetGameScore(int score)
        {
            CurrentGameScore = score;
        }

        /// <summary>
        /// ���̃^�[���֑J��
        /// </summary>
        public void NextTurn()
        {
            AudioManager.Instance.StopBGMAsObservable(1.5f);
            SceneTransitionManager.Instance.LoadSceneWithFade("MiniGameScene", 2f);

            SceneTransitionManager.Instance.OnFinishedFadeInAsObservable
                .Where(scene => scene == "MiniGameScene")
                .First()
                .Subscribe(_ =>
                {
                    CurrentTurn++;
                    CurrentPhase = GamePhase.Work;
                    OnBeginWorkPhase();
                    _onChangeCurrentPhase.OnNext(CurrentPhase);
                })
                .AddTo(gameObject);
        }

        /// <summary>
        /// ���̃t�F�C�Y�֑J��
        /// </summary>
        public void NextPhase()
        {
            if (CurrentPhase == GamePhase.Work)
            {
                AudioManager.Instance.StopBGM(1.5f);
                SceneTransitionManager.Instance.LoadSceneWithFade("BarScene", 2f);

                SceneTransitionManager.Instance.OnFinishedFadeInAsObservable
                    .Where(scene => scene == "BarScene")
                    .First()
                    .Subscribe(_ =>
                    {
                        CurrentPhase = GamePhase.Bar;
                        OnBeginBarPhase();
                        _onChangeCurrentPhase.OnNext(CurrentPhase);
                    })
                    .AddTo(gameObject);
            }
            else if (CurrentPhase == GamePhase.Bar)
            {
                AudioManager.Instance.StopBGM(1.5f);

                if (IsFinishedGame())
                {
                    //�I������
                    SceneTransitionManager.Instance.LoadSceneWithFade("ResultScene", 2f);
                }
                else
                {
                    SceneTransitionManager.Instance.LoadSceneWithFade("HomeScene", 2f);

                    SceneTransitionManager.Instance.OnFinishedFadeInAsObservable
                        .Where(scene => scene == "HomeScene")
                        .First()
                        .Subscribe(_ =>
                        {
                            CurrentPhase = GamePhase.Home;
                            OnBeginHomePhase();
                            _onChangeCurrentPhase.OnNext(CurrentPhase);
                        })
                        .AddTo(gameObject);
                }
            }
            else if (CurrentPhase == GamePhase.Home)
            {
                NextTurn();
            }
        }

        /// <summary>
        /// �d���t�F�C�Y�̊J�n������
        /// </summary>
        private void OnBeginWorkPhase()
        {
            // ���Ԃ��o�Ў��Ԃɐݒ�
            PlayerInfoManager.instance.actionCount.Value = AttendanceTime;
        }

        /// <summary>
        /// �o�[�t�F�C�Y�̊J�n������
        /// </summary>
        private void OnBeginBarPhase()
        {
            // TODO:�d���̌��ʂ������p��            
            // ���Ԃ�ގЎ��Ԃɐݒ�
            PlayerInfoManager.instance.actionCount.Value = LeaveTime;
            PlayerInfoManager.instance.drunkValue.Value = 0;
            PlayerInfoManager.instance.stressValue.Value += 30;
            // 1�_�ɂ�200�~�g���܂�
            PlayerInfoManager.instance.moneyValue.Value = CurrentGameScore * 200;
            Debug.Log($"���ԁF{PlayerInfoManager.instance.actionCount.Value}\n�����l�F{ PlayerInfoManager.instance.drunkValue.Value }\n�X�g���X�F{PlayerInfoManager.instance.stressValue.Value}\n�������F{PlayerInfoManager.instance.moneyValue.Value}");
        }

        /// <summary>
        /// �ƃt�F�C�Y�̊J�n������
        /// </summary>
        private void OnBeginHomePhase()
        {
            // ���߂ɋA�����ۂɓ����鎞��
            int addSleepTime = 24 - PlayerInfoManager.instance.actionCount.Value;
            // �o�Ђ܂ł̎���
            int sleepTime = AttendanceTime + addSleepTime;
            // �A���R�[����1���Ԃ�����3.75%������
            float ad = AlcoholDecompositionPerHour * sleepTime;
            float drunk = Mathf.Max(PlayerInfoManager.instance.drunkValue.Value - ad, 0);
            PlayerInfoManager.instance.drunkValue.Value = (int)drunk;
        }
    
        private bool IsFinishedGame()
        {
            if (PlayerInfoManager.instance.drunkValue.Value >= 60)
            {
                // �����x����΂�
                CurrentEndingId = 0;
                return true;
            }
            else if (CurrentTurn >= 7 && PlayerInfoManager.instance.stressValue.Value < 5)
            {
                // 7�A�Ό�ɃX�g���X5�ȉ�
                CurrentEndingId = 1;
                return true;
            }
            else if (PlayerInfoManager.instance.stressValue.Value >= 100)
            {
                // �X�g���X����΂�
                CurrentEndingId = 2;
                return true;
            }

            return false;
        }
    }
}
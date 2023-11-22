using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Base;
using Game.Scripts.Container;
using Game.Scripts.Controller;
using Game.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Manager
{
    public class LevelCanvasController : ComponentContainerBehaviour
    {
        [SerializeField] private GameObject rankingBoard;
        [SerializeField] private TextMeshProUGUI[] rankingTexts;
        private CoroutineManager _coroutineManager;

        private LevelRanking _levelRanking;
        private OpponentsController _opponentsController;
        private PlayerController _playerController;

        public override void ContainerOnAwake()
        {
            _coroutineManager = MainContainer.GetContainerComponent(nameof(CoroutineManager)) as CoroutineManager;
            _opponentsController =
                MainContainer.GetContainerComponent(nameof(OpponentsController)) as OpponentsController;
            _playerController = MainContainer.GetContainerComponent(nameof(PlayerController)) as PlayerController;
           
            if (rankingBoard.activeSelf) rankingBoard.SetActive(false);
        }

        private void Start()
        {
            StartRankingRoutine();
        }

        private void StartRankingRoutine()
        {
            List<CharacterBase> opponents = _opponentsController.GetAllOpponents.ConvertAll(x=>x as CharacterBase);
            opponents.Add(_playerController.GetPlayer);
            _levelRanking = new LevelRanking(opponents, ConstantsVariables.PlayerName);
            _coroutineManager.HStopCoroutine("RankingRoutine");
            _coroutineManager.HStartCoroutine("RankingRoutine", SetRankingTexts());
        }

        public override void ContainerRemoving()
        {
            _coroutineManager!.HStopCoroutine("RankingRoutine");
        }


        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            FinishLineController.FinisLineAction += SetRankingBoard;
        }

        protected override void UnRegisterEvents()
        {
            base.UnRegisterEvents();
            FinishLineController.FinisLineAction -= SetRankingBoard;
        }

        private IEnumerator SetRankingTexts()
        {
            yield return new WaitWhile(() => rankingBoard.activeInHierarchy);

            while (rankingBoard.activeInHierarchy)
            {
                var rankingCharacter = _levelRanking.CharacterRanking();

                for (var i = 0; i < 4; i++)
                {
                    var (characterName, rank) = rankingCharacter[i];

                    rankingTexts[i].text = $"{rank}. {characterName}";
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void SetRankingBoard(bool val)
        {
            rankingBoard.SetActive(!rankingBoard.activeInHierarchy);
        }
    }
}
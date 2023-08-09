

using GameFoundation.Scripts.Utilities.LogService;
using TheOneStudio.HyperCasual.Blueprints;
using TheOneStudio.HyperCasual.Others;

namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates
{
    using TheOneStudio.UITemplate.UITemplate.Scenes.Leaderboard;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.LeaderBoard;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using TheOneStudio.UITemplate.UITemplate.Scenes.Utils;
    using TheOneStudio.UITemplate.UITemplate.Services;
    using TheOneStudio.UITemplate.UITemplate.Services.CountryFlags.CountryFlags.Scripts;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;

    public class LeaderboardScreenView : BaseView
    {
        public LeaderboardAdapter Adapter;
        public Button             CloseButton;
        public Transform          YourRankerParentTransform;
        public CountryFlags       CountryFlags;
        public TMP_Text           BetterThanText;
        public int                MaxLevel    = 100;
        public int                LowestRank  = 68365;
        public int                HighestRank = 1;
        public int                RankRange => this.LowestRank - this.HighestRank;
        public AudioSource        audioSource;
    }
    
    public class LeaderboardScreenModel
    {
        public Action OnOkClicked;
    }

    [ScreenInfo(nameof(LeaderboardScreenView))]
    public class LeaderBoardScreenPresenter : UITemplateBaseScreenPresenter<LeaderboardScreenView, LeaderboardScreenModel>
    {
        private const string SFXLeaderboard = "sfx_leaderboard";
        
        private readonly MiscParamBlueprint miscParamBlueprint;

        #region inject

        private readonly DiContainer diContainer;
        private readonly UITemplateLevelDataController uiTemplateLevelDataController;
        private readonly UITemplateSoundServices uiTemplateSoundServices;

        #endregion

        private GameObject yourClone;
        private CancellationTokenSource animationCancelTokenSource;
        private List<Tween> animationTweenList = new();

        public LeaderBoardScreenPresenter(SignalBus signalBus, DiContainer diContainer,
            UITemplateLevelDataController uiTemplateLevelDataController,
            UITemplateSoundServices uiTemplateSoundServices, ILogService logger, MiscParamBlueprint miscParamBlueprint) : base(signalBus, logger)
        {
            this.diContainer = diContainer;
            this.uiTemplateLevelDataController = uiTemplateLevelDataController;
            this.uiTemplateSoundServices = uiTemplateSoundServices;
            this.miscParamBlueprint = miscParamBlueprint;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CloseButton.onClick.AddListener(this.OnOkClicked);
        }

        private int GetRankWithLevel(int level) => (int)(this.View.LowestRank -
                                                         Mathf.Sqrt(Mathf.Sqrt(level * 1f / this.View.MaxLevel)) *
                                                         this.View.RankRange);

        private void OnOkClicked()
        {
            this.View.audioSource.Stop();
            this.Model.OnOkClicked?.Invoke();
        }
        

        private async UniTask DoAnimation()
        {
            var indexPadding   = 4;
            var scrollDuration = 3;
            var scaleTime      = 1f;

            var TestList = new List<LeaderboardItemModel>();

            var currentLevel = this.uiTemplateLevelDataController.GetCurrentLevelData.Level;
            var oldRank      = this.GetRankWithLevel(currentLevel - 1);
            var newRank      = 5;
            var newIndex     = indexPadding;
            var oldIndex     = (oldRank - newRank - indexPadding);
            var currentRankingScore = this.miscParamBlueprint.RankingScore;
            for (var i = newRank - indexPadding; i < oldRank + indexPadding; i++)
            {
                TestList.Add(new LeaderboardItemModel(i, this.View.CountryFlags.GetRandomFlag(), NVJOBNameGen.GiveAName(Random.Range(1, 8)), false, "$ " + currentRankingScore.ToFormattedRewardCash()));
                currentRankingScore -= Random.Range(1, 10) * 123_123;
            }

            TestList[newIndex].IsYou = true;
            TestList[oldIndex].IsYou = true;
            TestList[oldIndex].CountryFlag = this.View.CountryFlags.GetLocalDeviceFlagByDeviceLang();
            TestList[oldIndex].Name = "You";
            TestList[oldIndex].RankingScore = TestList[newIndex].RankingScore;
            
            //Setup view
            await this.View.Adapter.InitItemAdapter(TestList, this.diContainer);
            this.View.Adapter.ScrollTo(oldIndex - indexPadding);

            //Create your clone
            this.yourClone = Object.Instantiate(this.View.Adapter.GetItemViewsHolderIfVisible(oldIndex).root.gameObject,
                this.View.YourRankerParentTransform);
            this.yourClone.GetComponent<CanvasGroup>().alpha = 1;
            var cloneView = this.yourClone.GetComponent<LeaderboardItemView>();
            this.View.BetterThanText.text = this.GetBetterThanText(oldRank);
            

            this.animationCancelTokenSource = new CancellationTokenSource();
            //Do animation
            //Do scale up
            this.animationTweenList.Clear();
            this.animationTweenList.Add(this.yourClone.transform.DOScale(Vector3.one * 1.1f, scaleTime).SetEase(Ease.InOutBack));
            await UniTask.Delay(TimeSpan.FromSeconds(scaleTime), cancellationToken: this.animationCancelTokenSource.Token);
            //Do move to new rank
            cloneView.ShowRankUP();
            this.animationTweenList.Add(DOTween.To(() => 0, setValue => cloneView.SetRankUp(setValue), oldRank - newRank, scrollDuration));
            this.animationTweenList.Add(DOTween.To(() => oldRank, setValue =>
            {
                cloneView.SetRank(setValue);
            }, newRank, scrollDuration));
            this.View.Adapter.SmoothScrollTo(newIndex - indexPadding, scrollDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(scrollDuration), cancellationToken: this.animationCancelTokenSource.Token);

            this.yourClone.GetComponent<LeaderboardItemView>().ScaleAnim();

        }


        private string GetBetterThanText(int currentRank) =>
            $"you are better than <color=#2DF2FF><size=120%>{(this.View.LowestRank * 1.5f - currentRank) / (this.View.LowestRank * 1.5f) * 100:F2}%</size ></color > of people";

        public override void Dispose()
        {
            base.Dispose();
            this.animationCancelTokenSource.Cancel();
            this.animationCancelTokenSource.Dispose();
            this.View.Adapter.StopScrollingIfAny();
            foreach (var tween in this.animationTweenList)
            {
                tween.Kill();
            }

            Object.Destroy(this.yourClone);
        }

        public override UniTask BindData(LeaderboardScreenModel popupModel)
        {
            this.View.audioSource.Play();
            _ = this.DoAnimation();
            return UniTask.CompletedTask;
        }
    }
}
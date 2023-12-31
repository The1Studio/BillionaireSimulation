﻿namespace TheOneStudio.HyperCasual.Scenes.Main.UI.LeaderBoard
{
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class LeaderboardItemModel
    {
        public int    Rank;
        public Sprite CountryFlag;
        public string Name;
        public bool   IsYou;
        public string RankingScore;

        public LeaderboardItemModel(int rank, Sprite countryFlag, string name, bool isYou, string rankingScore)
        {
            this.Rank        = rank;
            this.CountryFlag = countryFlag;
            this.Name        = name;
            this.IsYou       = isYou;
            this.RankingScore = rankingScore;
        }
    }

    public class LeaderboardItemView : TViewMono
    {
        public TMP_Text   RankText;
        public TMP_Text   NameText;
        public Image      FlagImage;
        public Image      BackGround;
        public TMP_Text   RankUpText;
        public GameObject RankUpObject;

        public Sprite OtherSpriteBg;
        public Sprite YourSpriteBg;
        public Sprite Rank1;
        public Sprite Rank2;
        public Sprite Rank3;
        public TMP_Text RankingScore;

        public void SetRank(int rank) { this.RankText.text = $"#{rank}"; }
        public void SetRankUp(int rankUp) { this.RankUpText.text = rankUp.ToString(); }
        public void ShowRankUP()
        {
            this.RankUpObject.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }

        public void ScaleAnim()
        {
            this.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        }
    }

    public class LeaderboardItemPresenter : BaseUIItemPresenter<LeaderboardItemView, LeaderboardItemModel>
    {
        public LeaderboardItemPresenter(IGameAssets gameAssets) : base(gameAssets) { }

        public override void BindData(LeaderboardItemModel param)
        {
            this.View.SetRank(param.Rank);
            this.View.NameText.text     = param.Name;
            this.View.NameText.fontSize = param.IsYou ? 50 : 30;
            this.View.FlagImage.sprite  = param.CountryFlag;
            this.View.RankUpObject.gameObject.SetActive(param.IsYou);
            this.View.RankUpObject.transform.localScale = Vector3.zero;
            this.View.RankingScore.text = param.RankingScore;
            this.View.RankText.enabled = true;
            if (param.Rank <= 3)
            {
                this.View.RankText.enabled = false;
            }

            this.View.BackGround.sprite = param.IsYou ? this.View.YourSpriteBg : this.View.OtherSpriteBg;
            switch(param.Rank) 
            {
                case 1:
                    this.View.BackGround.sprite = this.View.Rank1;
                    break;
                case 2:
                    this.View.BackGround.sprite = this.View.Rank2;
                    break;
                case 3:
                    this.View.BackGround.sprite = this.View.Rank3;
                    break;
            }

            this.View.GetComponent<CanvasGroup>().alpha = param.IsYou ? 0 : 1;
        }
    }
}
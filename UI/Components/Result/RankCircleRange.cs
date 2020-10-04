using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Rulesets.Maps;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Result
{
    public class RankCircleRange : UguiSprite {

        private UguiSprite[] rangeSprites;


        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private ResultModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            SpriteName = "circle-320";
            Color = Color.black;

            rangeSprites = new UguiSprite[Model.GetRankRangeTypes().Count()];
            for (int i = 0; i < rangeSprites.Length; i++)
            {
                var sprite = rangeSprites[i] = CreateChild<UguiSprite>($"range{i}");
                sprite.Anchor = AnchorType.Fill;
                sprite.Offset = new Offset(4f);
                sprite.SpriteName = "outline-circle-296-4";
                sprite.ImageType = Image.Type.Filled;
                sprite.SetRadial360Fill(Image.Origin360.Top);
                sprite.Active = false;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Map.BindAndTrigger(OnMapChanged);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Map.Unbind(OnMapChanged);
        }

        /// <summary>
        /// Sets up range sprites for current map.
        /// </summary>
        private void SetupRanges()
        {
            var scoreProcessor = Model.GetScoreProcessor();
            var rankRangeTypes = Model.GetRankRangeTypes().ToList();
            for (int i = 0; i < rankRangeTypes.Count; i++)
            {
                var rank = rankRangeTypes[i];
                float accFrom = scoreProcessor.GetRankAccuracy(rank);
                float accTo = (
                    i == rankRangeTypes.Count - 1 ?
                    1f :
                    scoreProcessor.GetRankAccuracy(rankRangeTypes[i + 1])
                );
                SetRangeSprite(rangeSprites[i], ColorPreset.GetRankColor(rank), accFrom, accTo);
            }
        }

        /// <summary>
        /// Sets up range sprite based on specified accuracy range.
        /// </summary>
        private void SetRangeSprite(UguiSprite sprite, Color color, float accFrom, float accTo)
        {
            float offsetAngle = 1f;
            sprite.Active = true;
            sprite.Color = color;
            sprite.RotationZ = accFrom * -360f - offsetAngle;
            sprite.FillAmount = (accTo - accFrom) - (offsetAngle * 2f / 360f);
        }

        /// <summary>
        /// Event called when the current map instance has changed.
        /// </summary>
        private void OnMapChanged(IPlayableMap map)
        {
            if (map == null)
            {
                foreach(var sprite in rangeSprites)
                    sprite.Active = false;
            }
            else
                SetupRanges();
        }
    }
}
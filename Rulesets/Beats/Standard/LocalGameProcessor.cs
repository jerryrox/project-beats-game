using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;

namespace PBGame.Rulesets.Beats.Standard
{
    public class LocalGameProcessor : GameProcessor {

        public override float CurrentTime => MusicController.CurrentTime;


        public override void JudgePassive(float curTime, HitObjectView hitObjectView)
        {
            // TODO:
        }
    }
}
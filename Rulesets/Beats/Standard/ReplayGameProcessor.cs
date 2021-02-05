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
    public class ReplayGameProcessor : GameProcessor
    {
        // TODO: When implemented ReplayFrame class, hold the reference to data stream reader with generic type of ReplayFrame.

        // TODO: Return the last replayed frame's time.
        public override float CurrentTime => 0;


        public override void JudgePassive(float curTime, HitObjectView hitObjectView)
        {
            // TODO:
            throw new NotImplementedException();
        }
    }
}
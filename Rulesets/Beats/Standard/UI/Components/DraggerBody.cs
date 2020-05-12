using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class DraggerBody : UguiObject<CurvedLineDrawable>
    {
        private Dragger dragger;


        [ReceivesDependency]
        private PlayAreaContainer PlayArea { get; set; }



        [InitWithDependency]
        private void Init()
        {
            component.CurveAngle = 10f;
            component.UseSmoothEnds = true;
            component.color = new Color(1f, 1f, 1f, 0.3334f);

            var effect = AddEffect(new DefaultShaderEffect());
            effect.StencilOperation = StencilOp.IncrementSaturate;
            effect.CompareFunction = CompareFunction.Equal;
        }

        /// <summary>
        /// Sets the dragger object to draw the body from.
        /// </summary>
        public void SetDragger(Dragger dragger)
        {
            this.dragger = dragger;
            component.CurveRadius = dragger.Radius;
        }

        /// <summary>
        /// Renders body path.
        /// </summary>
        public void RenderPath()
        {
            // Property value caching for performance.
            float distPerTime = PlayArea.DistancePerTime;
            float interval = PlayArea.DraggerBodyInterval;
            float startTime = dragger.StartTime;
            float endTime = dragger.EndTime;
            float duration = dragger.Duration;

            // Start building line.
            float lastTimeSpent = 0f;
            Vector2 lastPathPos = dragger.GetPosition(lastTimeSpent);
            float nextTimeSpent = lastTimeSpent;
            Vector2 nextPathPos = lastPathPos;
            for (float t = startTime; t < endTime; t += interval)
            {
                nextTimeSpent = Math.Min(lastTimeSpent + interval, duration);
                nextPathPos = dragger.GetPosition(nextTimeSpent / duration);
                var line = new Line(
                    new Vector2(lastPathPos.x, lastTimeSpent * distPerTime),
                    new Vector2(nextPathPos.x, nextTimeSpent * distPerTime)
                );
                lastTimeSpent = nextTimeSpent;
                lastPathPos = nextPathPos;

                component.AddLine(line);
            }
        }

        /// <summary>
        /// Clears path data.
        /// </summary>
        public void ClearPath()
        {
            component.ClearLines();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Graphics
{
    public class RootMain : UguiRoot, IRootMain {

        /// <summary>
        /// Creates a new instance of the RootMain and returns it.
        /// </summary>
        public static new RootMain Create(IDependencyContainer dependency)
        {
            var root = new GameObject("RootMain").AddComponent<RootMain>();
            if (dependency != null)
            {
                dependency.Inject(root);
            }
            return root;
        }

        protected override void Awake()
        {
            base.Awake();

            Resolution = new Vector2(1280f, 720f);
            SetOverlayRender(1);
        }
    }
}
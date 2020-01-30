using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Graphics
{
    public class Root3D : UguiRoot, IRoot3D {

        /// <summary>
        /// Creates a new instance of the RootMain and returns it.
        /// </summary>
        public static new Root3D Create(IDependencyContainer dependency)
        {
            var root = new GameObject("Root3D").AddComponent<Root3D>();
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
            SetCameraRender(Camera.main);
        }
    }
}
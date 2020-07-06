using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class QuitModel : BaseModel {

        [ReceivesDependency]
        private IGame Game { get; set; }


        /// <summary>
        /// Forcibly quits the game.
        /// </summary>
        public void Quit() => Game.ForceQuit();
    }
}
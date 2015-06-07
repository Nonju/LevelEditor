using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace LevelEditor.BlocksFolder {
    class DirtBlock : Blocks {

        public DirtBlock(Texture2D texture, Vector2 pos, float width, float height, string blockName) : base(texture, pos, width, height, blockName) { }

        public override void Update() {
            base.Update();
        }
        public override void OnClick() {
            base.OnClick();
        }

    }
}

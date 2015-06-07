using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace LevelEditor {
    class GameObjects {

        protected Texture2D texture;
        protected Vector2 pos;
        protected float width, height;
        protected Rectangle rec;

        public GameObjects(Texture2D texture, Vector2 pos, float width, float height) {
            this.texture = texture;
            this.pos = pos;
            this.width = width;
            this.height = height;

            rec = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
        }

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rec, Color.White);
        }

        public virtual bool OnHover(MouseState mState) { //if mouse is on top of object
            if (mState.X > pos.X && mState.X < (pos.X + width)) {
                if (mState.Y > pos.Y && mState.Y < (pos.Y + height)) {
                    return true;
                }
            }
            return false;
        }

        bool objectIsClicked = false;
        public virtual bool IsClicked(MouseState mState) {
            if (mState.LeftButton == ButtonState.Pressed && !objectIsClicked) {
                objectIsClicked = true;
                if (mState.X > pos.X && mState.X < (pos.X + width)) {
                    if (mState.Y > pos.Y && mState.Y < (pos.Y + height)) {
                        return true; //object is clicked!
                    }

                }
            }
            else if (mState.LeftButton == ButtonState.Released) { objectIsClicked = false; }
            return false; // == not Clicked
        }

        public virtual void OnClick() { } //if object is clicked

        //Properties
        public Vector2 Pos { get { return pos; } set { pos = value; } }
        public float Width { get { return width; } }
        public float Height { get { return height; } }

    }//end GameObjects

    class PhysicalObjects : GameObjects {

        protected bool isAlive = true;
        protected Rectangle intersectRec;

        public PhysicalObjects(Texture2D texture, Vector2 pos, float width, float height) : base(texture, pos, width, height) { }

        public bool CheckCollision(Rectangle otherRec) { //checks if this rec intersects some other physical object
            if (intersectRec.Intersects(otherRec)) { return true; }
            else { return false; }
        }
        

    }//end PhysicalObjects

    class MovingObjects : PhysicalObjects{
        
        protected Vector2 speed;

        public MovingObjects(Texture2D texture, Vector2 pos, float width, float height, Vector2 speed) : base(texture, pos, width, height) {
            this.speed = speed;
        }

    }//end MovingObjects

    class Blocks : PhysicalObjects {

        protected string blockName;

        public Blocks(Texture2D texture, Vector2 pos, float width, float height, string blockName) : base(texture, pos, width, height) {
            if (blockName == null) { this.blockName = "EmptyBlock"; }
            else { this.blockName = blockName; }
        }

        //Properties
        public string BlockName { get { return blockName; } }

    }//end Blocks

    class Buttons : GameObjects {

        protected int nrToAdd;
        protected Color btnColor;

        public Buttons(Texture2D texture, Vector2 pos, float width, float height) : base(texture, pos, width, height) { }

        public int NrToAdd { get { return nrToAdd; } set { nrToAdd = value; } } //Nr to add with when clicking button
        public virtual void OnClick(Blocks[,] lvlArray, string lvlName) { } //for saving levels

        public virtual void Draw(SpriteBatch spriteBatch, Color btnHoverColor, MouseState mState) {
            if (OnHover(mState)) { btnColor = btnHoverColor; }
            else { btnColor = Color.White; }
            spriteBatch.Draw(texture, rec, btnColor);
        }

    }//end Buttons

}

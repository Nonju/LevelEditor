using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace LevelEditor {
    class SetNrTool {

        int counter, maxNr, minNr;
        string counterMessage;
        Vector2 basePos, tempVector, counterPos;
        float width, height, btnWidth, btnHeight, nrAreaWidth, nrAreaHeight;
        Buttons[] btnList;
        Buttons btnAddOne, btnAddFive, btnReduceOne, btnReduceFive;
        SpriteFont sf;


        public SetNrTool(ContentManager content ,Vector2 basePos, float width, float height, string counterMessage, int maxNr, int minNr) {
            counter = 1;
            this.counterMessage = counterMessage;
            this.maxNr = maxNr;
            this.minNr = minNr;
            btnList = new Buttons[4]; //four buttons
            this.basePos = basePos;
            this.width = width;
            this.height = height;
            counterPos = new Vector2(basePos.X + (width * 0.5f), basePos.Y + (height * 0.5f));
            sf = content.Load<SpriteFont>(@"Images/SpriteFont1");
            //button measurements
            btnWidth = width * 0.15f;
            btnHeight = height * 0.25f;
            //size and pos of area where current NR is showing
            nrAreaWidth = width * 0.3f;
            nrAreaHeight = height * 0.3f;

            //create new buttons
            tempVector = new Vector2(basePos.X /*+ (width * 0.1f)*/, basePos.Y + (height * 0.2f));
            btnAddOne = new ButtonsFolder.CounterBtn(content.Load<Texture2D>(@"Images/BtnImages/BtnAddOne.png"), tempVector, btnWidth, btnHeight);
            btnAddOne.NrToAdd = 1;

            tempVector = new Vector2(basePos.X + (width * 0.8f), basePos.Y + (height * 0.2f));
            btnAddFive = new ButtonsFolder.CounterBtn(content.Load<Texture2D>(@"Images/BtnImages/BtnAddFive.png"), tempVector, btnWidth, btnHeight);
            btnAddFive.NrToAdd = 5;

            tempVector = new Vector2(basePos.X /*+ (width * 0.1f)*/, basePos.Y + (height * 0.8f));
            btnReduceOne = new ButtonsFolder.CounterBtn(content.Load<Texture2D>(@"Images/BtnImages/BtnReduceOne.png"), tempVector, btnWidth, btnHeight);
            btnReduceOne.NrToAdd = -1;

            tempVector = new Vector2(basePos.X + (width * 0.8f), basePos.Y + (height * 0.8f));
            btnReduceFive = new ButtonsFolder.CounterBtn(content.Load<Texture2D>(@"Images/BtnImages/BtnReduceFive.png"), tempVector, btnWidth, btnHeight);
            btnReduceFive.NrToAdd = -5;

            //add buttons to btnList
            btnList[0] = btnAddOne;
            btnList[1] = btnAddFive;
            btnList[2] = btnReduceOne;
            btnList[3] = btnReduceFive;
        }
        MouseState mState;
        public int SetNr() {
            mState = Mouse.GetState();
            foreach (Buttons btn in btnList) {
                if (btn.IsClicked(mState)) {
                    counter += btn.NrToAdd;
                    if (counter < minNr) { counter = 1; } //to prevent editor from trying to create a negative amount of blocks
                    if (counter > maxNr) { counter = maxNr; }
                }
            }
            return counter;
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Buttons btn in btnList) { btn.Draw(spriteBatch); }
            spriteBatch.DrawString(sf,counterMessage + counter.ToString(), counterPos, Color.Red);
        }

    }
}

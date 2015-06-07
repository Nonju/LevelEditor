using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;


namespace LevelEditor {
    class DisplayFiles {

        string[] files;
        string[] fileNames;
        Buttons[] selectFileBtns;
        Texture2D selectFileBtnTexture;
        Vector2 selectFileBtnPos;
        float selectFileBtnWidth, selectFileBtnHeight;
        Buttons[] deleteFileBtns;
        Texture2D deleteFileBtnTexture;
        Vector2 deleteFileBtnPos;
        float deleteFileBtnWidth, deleteFileBtnHeight;

        string filePath;
        SpriteFont sf;
        Vector2 spritePos;
        float moveDownByX; //floatNr that pushes down

        public DisplayFiles(ContentManager content, GameWindow window, string filePath, Vector2 textBasePos, float moveDownByX) {
            sf = content.Load<SpriteFont>(@"Images/SpriteFont1");
            this.filePath = filePath;
            this.spritePos = textBasePos;
            this.moveDownByX = moveDownByX;

            selectFileBtnTexture = content.Load<Texture2D>(@"Images/BtnImages/Temp/TempSelectFileBtn.bmp");
            selectFileBtnWidth = window.ClientBounds.Width * 0.2f;
            selectFileBtnHeight = window.ClientBounds.Height * 0.1f;
            selectFileBtnPos = new Vector2((window.ClientBounds.Width - selectFileBtnWidth), textBasePos.Y);

            deleteFileBtnTexture = content.Load<Texture2D>(@"Images/BtnImages/Temp/TempDeleteFileBtn.bmp");
            deleteFileBtnWidth = window.ClientBounds.Width * 0.15f;
            deleteFileBtnHeight = selectFileBtnHeight;
            deleteFileBtnPos = new Vector2((selectFileBtnPos.X - (deleteFileBtnWidth * 1.3f)), textBasePos.Y);

            LoadFiles();

        }

        bool loadedOnce = false;
        public void LoadFiles() {
            files = Directory.GetFiles(filePath); //recives files from directory
            fileNames = new string[files.Length];
            selectFileBtns = new Buttons[files.Length];
            deleteFileBtns = new Buttons[files.Length];

            FileDisplay(); //prepare new file-list
        }


        string[] tempFileString;
        public void FileDisplay() {
            try {
                for (int i = 0; i < files.Length; i++) {
                    tempFileString = files[i].Split('/');
                    fileNames[i] = tempFileString[tempFileString.Length - 1]; //adds the last array location to fileNames (It removes the remaining filePath)
                    if (!loadedOnce) { //only loads the buttons ONCE!
                        Buttons tempBtn = new ButtonsFolder.BasicBtn(selectFileBtnTexture, selectFileBtnPos, selectFileBtnWidth, selectFileBtnHeight);
                        selectFileBtnPos.Y = (i + 1) * selectFileBtnHeight; //same as with the filenameSpritefont below
                        selectFileBtns[i] = tempBtn;

                        Buttons stempBtn = new ButtonsFolder.BasicBtn(deleteFileBtnTexture, deleteFileBtnPos, deleteFileBtnWidth, deleteFileBtnHeight);
                        deleteFileBtnPos.Y = selectFileBtnPos.Y;
                        deleteFileBtns[i] = stempBtn;
                    }
                }
                loadedOnce = true;
            }
            catch { }
        }

        MouseState mState;
        public bool GetFileName(ref string fileName) {
            mState = Mouse.GetState();
            if (loadedOnce) {
                for (int i = 0; i < fileNames.Length; i++) {
                    tempFileString = fileNames[i].Split('.'); //removes the fileEnding (.xml)
                    if (selectFileBtns[i].IsClicked(mState)) {
                        fileName = tempFileString[0];
                        return true;
                    }
                    if (deleteFileBtns[i].IsClicked(mState)) { //allowing the user to delete existing lvls from the menu
                        //code to delete file...
                    }
                }
            }
            //btn was not clicked
            return false;
        }

        Color fileColor;
        public void Draw(SpriteBatch spriteBatch) {
            if (loadedOnce) {
                for (int i = 0; i < fileNames.Length; i++) { 
                    if (i % 2 == 0) { fileColor = Color.Black; } //meant to be used for different colored backgrounds (not added yet!!)
                    else { fileColor = Color.DarkRed; }

                    spritePos.Y = i * selectFileBtnHeight;
                    selectFileBtns[i].Draw(spriteBatch, Color.LightGreen, mState);
                    deleteFileBtns[i].Draw(spriteBatch, Color.Pink, mState);
                    spriteBatch.DrawString(sf, (i + 1) + ":/" + fileNames[i], spritePos, Color.White);
                }
            }
        }

        //Properties
        public bool LoadedOnce { get { return loadedOnce; } set { loadedOnce = value; } }
        public float SelectFileBtnPosY { set { selectFileBtnPos.Y = value; } }

    }//end class-DisplayLvlFiles

    
}

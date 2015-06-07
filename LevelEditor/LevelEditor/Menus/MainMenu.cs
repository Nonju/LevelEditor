using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace LevelEditor.Menus {
    static class MainMenu {
        //MenuStates
        public enum State { Menu, Editor, Options, Info, Quit };
        public static State currentState;

        static MenuTemplate menu;
        static float menuBtnWidth, menuBtnHeight;

        //Options
        static Buttons mainMnuBtn;
        static Vector2 mainMnuBtnPos;
        static float optionsMnuBtnWidth, optionsMnuBtnHeight;

        static int windResX, windResY; //for when resChanging is implemented

        //Info
        static ReadFromTxtFile RFTF;
        static Buttons nxtPage, prevPage;
        static Vector2 nxtPagePos, prevPagePos;
        static float pageBtnsWidth, pageBtnsHeight;


        public static void Load(ContentManager content, GameWindow window, Game1 game) { 
            //MenuBtnSizes
            menuBtnWidth = window.ClientBounds.Width * 0.3f;
            menuBtnHeight = window.ClientBounds.Height * 0.2f;
            //MenuAdd
            menu = new MenuTemplate((int)State.Menu);
            //change images to real ones after making them!!!!
            menu.AddItem(window, content.Load<Texture2D>(@"Images/BtnImages/Temp/TempEditorBtn.bmp"), menuBtnWidth, menuBtnHeight, (int)State.Editor);
            menu.AddItem(window, content.Load<Texture2D>(@"Images/BtnImages/Temp/TempOptionsBtn.bmp"), menuBtnWidth, menuBtnHeight, (int)State.Options);
            menu.AddItem(window, content.Load<Texture2D>(@"Images/BtnImages/Temp/TempInfoBtn.bmp"), menuBtnWidth, menuBtnHeight, (int)State.Info);
            menu.AddItem(window, content.Load<Texture2D>(@"Images/BtnImages/Temp/TempQuitBtn.bmp"), menuBtnWidth, menuBtnHeight, (int)State.Quit);

            //EditorMenu
            Menus.EditorMenu.currentState = EditorMenu.State.Menu;
            Menus.EditorMenu.Load(content, window, game);

            //Options
            optionsMnuBtnWidth = (window.ClientBounds.Width * 0.3f);
            optionsMnuBtnHeight = (window.ClientBounds.Height * 0.14f);
            mainMnuBtnPos = new Vector2((window.ClientBounds.Width * 0.5f) - (optionsMnuBtnWidth / 2), window.ClientBounds.Height * 0.8f);
            mainMnuBtn = new ButtonsFolder.BasicBtn(content.Load<Texture2D>(@"Images/BtnImages/Temp/TempMainMenubtn.bmp"), mainMnuBtnPos, optionsMnuBtnWidth, optionsMnuBtnHeight);

            //Info
            pageBtnsWidth = window.ClientBounds.Width * 0.05f;
            pageBtnsHeight = window.ClientBounds.Height * 0.05f;
            float pageBtnsHeightPos = (window.ClientBounds.Height * 0.5f) - (pageBtnsHeight / 2);
            prevPagePos = new Vector2(0, pageBtnsHeightPos);
            nxtPagePos = new Vector2((window.ClientBounds.Width - pageBtnsWidth), pageBtnsHeightPos);
            
            nxtPage = new ButtonsFolder.BasicBtn(content.Load<Texture2D>(@"Images/BtnImages/Temp/TempBtnRight"), nxtPagePos, pageBtnsWidth, pageBtnsHeight);
            prevPage = new ButtonsFolder.BasicBtn(content.Load<Texture2D>(@"Images/BtnImages/Temp/TempBtnLeft"), prevPagePos, pageBtnsWidth, pageBtnsHeight);

            RFTF = new ReadFromTxtFile(content, window, new Vector2((pageBtnsWidth + pageBtnsWidth * 0.2f),0));


        }

        public static State MenuUpdate(GameTime gameTime) {
            return (State)menu.Update(gameTime);
        }
        public static void MenuDraw(SpriteBatch spriteBatch) {
            menu.Draw(spriteBatch);
        }

        public static State EditorUpdate(GameTime gameTime, ContentManager content) {
            switch (EditorMenu.currentState) { 
                case Menus.EditorMenu.State.NewEditor:
                    Menus.EditorMenu.currentState = EditorMenu.NewEditorUpdate(content);
                    break;
                case Menus.EditorMenu.State.LoadEditor:
                    Menus.EditorMenu.currentState = EditorMenu.LoadEditorUpdate(content);
                    break;
                case EditorMenu.State.MainMenu:
                    EditorMenu.currentState = EditorMenu.State.Menu;
                    EditorMenu.menu.Selected = 0; //Resets ingame-menu when leaving it
                    return Menus.MainMenu.currentState = Menus.MainMenu.MenuUpdate(gameTime);
                case EditorMenu.State.Menu:
                    EditorMenu.currentState = EditorMenu.EditorMenuUpdate(gameTime);
                    break;
                default: //menu
                    EditorMenu.currentState = EditorMenu.EditorMenuUpdate(gameTime);
                    break;
            }
            
            //stay in EditorUpdate
            return State.Editor;
        }
        public static void EditorDraw(SpriteBatch spriteBatch) {
            switch (EditorMenu.currentState) { 
                case EditorMenu.State.NewEditor:
                    EditorMenu.NewEditorDraw(spriteBatch);
                    break;
                case EditorMenu.State.LoadEditor:
                    EditorMenu.LoadEditorDraw(spriteBatch);
                    break;
                default: //Menu
                    EditorMenu.EditorMenuDraw(spriteBatch);
                    break;
            }
        }


        static MouseState mState;
        public static State OptionsUpdate() {
            mState = Mouse.GetState();


            if (mainMnuBtn.IsClicked(mState)) { return State.Menu; } //user wants back to mainMenu
            //stay in OptionsUpdate
            return State.Options;
        }
        public static void OptionsDraw(SpriteBatch spriteBatch) {
            mainMnuBtn.Draw(spriteBatch, Color.LightBlue, mState);
        }


        static string filePath;
        static int currentInfoPage = 0;
        static string[] pageFilePaths;
        public static State InfoUpdate() {
            mState = Mouse.GetState();
            pageFilePaths = RetriveFiles.retrive(@"../XmlDocuments/Info/");

            if (nxtPage.IsClicked(mState)) { //next info-page
                if (currentInfoPage > pageFilePaths.Length - 1) { currentInfoPage = pageFilePaths.Length - 1; }
                else { currentInfoPage++; }
            }
            if (prevPage.IsClicked(mState)) { //previous info-page
                if (currentInfoPage < 0) { currentInfoPage = 0; }
                else { currentInfoPage--; }

            }

            for (int i = 0; i < pageFilePaths.Length; i++) {
                if (i == currentInfoPage) { filePath = pageFilePaths[i]; }
            }

            RFTF.FileReader(filePath);

            if (mainMnuBtn.IsClicked(mState)) { return State.Menu; } //user wants back to mainMenu
            //stay in InfoUpdate
            return State.Info;
        }
        public static void InfoDraw(SpriteBatch spriteBatch) {
            nxtPage.Draw(spriteBatch, Color.LightGray, mState);
            prevPage.Draw(spriteBatch, Color.LightGray, mState);
            RFTF.Draw(spriteBatch);
            mainMnuBtn.Draw(spriteBatch, Color.LightBlue, mState);
        }


    }

}

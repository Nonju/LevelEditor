using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace LevelEditor.Menus {
    static class EditorMenu {

        //Menustates
        public enum State { Menu, NewEditor, LoadEditor, MainMenu };
        public static State currentState;

        //Menu
        public static MenuTemplate menu;
        static float menuBtnWidth, menuBtnHeight;

        //Editor
        static Editor editor;
        
        static SetNrTool setWidth, setHeight;
        static float setNrWidth, setNrHeight;
        static Buttons madeSizeSelection;
        static float sizeSelectWidth, sizeSelectHeight;

        static DisplayFiles displayFiles;


        //Testmode
        static bool isTestmodeOn;

        public static void Load(ContentManager content, GameWindow window, Game1 game) {
            //MenuBtnSizes
            menuBtnWidth = window.ClientBounds.Width * 0.3f;
            menuBtnHeight = window.ClientBounds.Height * 0.2f;
            //MenuAdd
            menu = new MenuTemplate((int)State.Menu);
            menu.BtnPressed = true; //prevents the game from instantly selecting the first menuoption it lands on in this menu
            //change images to real ones after making them!!!!
            menu.AddItem(window, content.Load<Texture2D>(@"Images/BtnImages/Temp/TempEditorBtn.bmp"), menuBtnWidth, menuBtnHeight, (int)State.NewEditor);
            menu.AddItem(window, content.Load<Texture2D>(@"Images/BtnImages/Temp/TempLoadEditorBtn.bmp"), menuBtnWidth, menuBtnHeight, (int)State.LoadEditor);
            menu.AddItem(window, content.Load<Texture2D>(@"Images/BtnImages/Temp/TempMainMenuBtn.bmp"), menuBtnWidth, menuBtnHeight, (int)State.MainMenu);

            //Editor
            editor = new Editor(content, window, game);
            
            setNrWidth = (window.ClientBounds.Width * 0.4f);
            setNrHeight = (window.ClientBounds.Height * 0.4f);
            setWidth = new SetNrTool(content, new Vector2((window.ClientBounds.Width * 0.05f), (window.ClientBounds.Height * 0.4f)), setNrWidth, setNrHeight, "X: ", 25, 1);
            setHeight = new SetNrTool(content, new Vector2((window.ClientBounds.Width * 0.6f), (window.ClientBounds.Height * 0.4f)), setNrWidth, setNrHeight, "Y: ", 15, 1);
            //buttons for selecting editorSize on newEditor-loadup
            sizeSelectWidth = (window.ClientBounds.Width * 0.2f);
            sizeSelectHeight = (window.ClientBounds.Height * 0.1f);
            madeSizeSelection = new ButtonsFolder.BasicBtn(content.Load<Texture2D>(@"Images/BtnImages/Temp/TempLoadEditorBtn.bmp"), new Vector2((window.ClientBounds.Width * 0.5f) - (sizeSelectWidth / 2), window.ClientBounds.Height * 0.85f), sizeSelectWidth, sizeSelectHeight);
            //allows the player to select an existing file and load it into the editor
            displayFiles = new DisplayFiles(content, window, @"../XmlDocuments/Lvls/", Vector2.Zero, window.ClientBounds.Height * 0.1f);

            //Testmode
            isTestmodeOn = false; //sets "testmode" to false on first load

        }

        public static State EditorMenuUpdate(GameTime gameTime) {
            return (State)menu.Update(gameTime);
        }
        public static void EditorMenuDraw(SpriteBatch spriteBatch) {
            menu.Draw(spriteBatch);
        }

        static bool editorSizeSelected = false;
        static bool newEditorLoaded = false;
        static int eWidth, eHeight;
        static MouseState mState;
        public static State NewEditorUpdate(ContentManager content) {
            mState = Mouse.GetState();
            if (editorSizeSelected) {
                if (newEditorLoaded) { //editorUpdate-Stuff
                    editor.Update(content);
                    if (editor.BackToMenu) { //allows the user to return back to the editorMenu (though without saving)
                        //reset bool's before exiting editor
                        editorSizeSelected = false;
                        newEditorLoaded = false;
                        editor.BackToMenu = false;

                        displayFiles.LoadedOnce = false;
                        displayFiles.SelectFileBtnPosY = 0; //resets the y-axis for selectFileBtn's pos
                        displayFiles.LoadFiles();
                        return State.Menu;
                    }
                }
                else {
                    //loads the editor ONE TIME before continuing
                    editor.CreateEmptyEditor(content, eWidth, eHeight);
                    newEditorLoaded = true; //makes it so editor only creates ONE new empty editor
                }
            }
            else {
                //Code for selecting editor measurements
                eWidth = setWidth.SetNr();
                eHeight = setHeight.SetNr();
                if (madeSizeSelection.IsClicked(mState)) { editorSizeSelected = true; } //if true, load editor
            }

            //stay in NewEditorUpdate
            return State.NewEditor;
        }
        public static void NewEditorDraw(SpriteBatch spriteBatch) {
            if (editorSizeSelected && newEditorLoaded) {
                editor.Draw(spriteBatch);
                //TestModeDraw
                if (isTestmodeOn) { TestmodeDraw(spriteBatch); }
            }
            else {
                setWidth.Draw(spriteBatch);
                setHeight.Draw(spriteBatch);
                madeSizeSelection.Draw(spriteBatch, Color.LightCyan, mState);
            }

        }

        static bool fileSelected = false;
        static bool existingEditorLoaded = false;
        static string lvlName;
        public static State LoadEditorUpdate(ContentManager content) {
            mState = Mouse.GetState();
            if (fileSelected) {
                if (existingEditorLoaded) { //editorUpdate-Stuff
                    editor.Update(content);
                    if (editor.BackToMenu) { //allows the user to return back to the editorMenu (though without saving)
                        //reset bool's before exiting editor
                        editor.BackToMenu = false;
                        fileSelected = false;
                        existingEditorLoaded = false;

                        displayFiles.LoadedOnce = false;
                        displayFiles.SelectFileBtnPosY = 0; //resets the y-axis for selectFileBtn's pos
                        displayFiles.LoadFiles();
                        return State.Menu; 
                    }
                }
                else {
                    //loads the editor ONE TIME before continuing
                    editor.LoadExistingEditor(content, lvlName);
                    existingEditorLoaded = true; //makes it so editor only creates ONE new empty editor
                }
            }
            else {
                displayFiles.FileDisplay();
                fileSelected = displayFiles.GetFileName(ref lvlName);
            }

            //Testmode
            if (isTestmodeOn) { TestmodeUpdate(); }

            //stay in LoadEditorUpdate
            return State.LoadEditor;
        }
        public static void LoadEditorDraw(SpriteBatch spriteBatch) {
            if (fileSelected && existingEditorLoaded) {
                editor.Draw(spriteBatch);
                //TestModeDraw
                if (isTestmodeOn) { TestmodeDraw(spriteBatch); }
            }
            else {
                displayFiles.Draw(spriteBatch);
            }
        }

        public static void TestmodeUpdate() { } //Adds a testPlayer that allows the user to testrun the lvl (bonus goal, added if time!!)
        public static void TestmodeDraw(SpriteBatch spriteBatch) { }


    }
}

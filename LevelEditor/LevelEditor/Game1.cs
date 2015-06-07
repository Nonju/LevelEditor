#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace LevelEditor {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
            : base() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize() {
            //show mousecursor
            this.IsMouseVisible = true;

            base.Initialize();
        }


        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //MainMenu
            Menus.MainMenu.currentState = Menus.MainMenu.State.Menu;
            Menus.MainMenu.Load(Content, Window, this);
            
        }


        protected override void UnloadContent() { }


        protected override void Update(GameTime gameTime) {

            //MainMenu
            switch (Menus.MainMenu.currentState) { 
                case Menus.MainMenu.State.Editor:
                    Menus.MainMenu.currentState = Menus.MainMenu.EditorUpdate(gameTime, Content);
                    break;
                case Menus.MainMenu.State.Options:
                    Menus.MainMenu.currentState = Menus.MainMenu.OptionsUpdate();
                    break;
                case Menus.MainMenu.State.Info:
                    Menus.MainMenu.currentState = Menus.MainMenu.InfoUpdate();
                    break;
                case Menus.MainMenu.State.Quit:
                    this.Exit();
                    break;
                default: //Menu
                    Menus.MainMenu.currentState = Menus.MainMenu.MenuUpdate(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            switch (Menus.MainMenu.currentState) { 
                case Menus.MainMenu.State.Editor:
                    Menus.MainMenu.EditorDraw(spriteBatch);
                    break;
                case Menus.MainMenu.State.Options:
                    Menus.MainMenu.OptionsDraw(spriteBatch);
                    break;
                case Menus.MainMenu.State.Info:
                    Menus.MainMenu.InfoDraw(spriteBatch);
                    break;
                default: //Menu
                    Menus.MainMenu.MenuDraw(spriteBatch);
                    break;
            }
            
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}

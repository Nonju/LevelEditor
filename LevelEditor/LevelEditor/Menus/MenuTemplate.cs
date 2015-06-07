using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace LevelEditor.Menus {
    class MenuItem {
        Texture2D texture;
        Vector2 pos;
        int currentState;
        Rectangle rec;

        public MenuItem(Texture2D texture, Vector2 pos, float textureWidth, float textureHeight, int currentState) {
            this.texture = texture;
            this.pos = pos;
            this.currentState = currentState;

            rec = new Rectangle((int)pos.X, (int)pos.Y, (int)textureWidth, (int)textureHeight);
        }

        //Properties
        public Rectangle Rec { get { return rec; } }
        public Texture2D Texture { get { return texture; } }
        public Vector2 Pos { get { return pos; } } //Probably not needed since menu can't be moved
        public int State { get { return currentState; } }
        
    }//end class-MenuItem

    class MenuTemplate {
        List<MenuItem> menu;
        int selected = 0; //First menuselection is always on location "0" in menuList

        float currentHeight = 0; //menuItems differenet heights
        double lastChange = 0; //time since last menuSelection
        int defaultMenuState;

        public MenuTemplate(int defaultMenuState) {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
        }

        public void AddItem(GameWindow window, Texture2D itemTexture, float textureWidth, float textureHeight, int state) { 
            //itemHeights
            float X = 0;
            float Y = 0 + currentHeight;
            currentHeight += textureHeight + (window.ClientBounds.Height * 0.04f);

            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), textureWidth, textureHeight, state);
            menu.Add(temp);

        }

        bool btnPressed = false;
        KeyboardState kState;
        public int Update(GameTime gameTime) {
            kState = Keyboard.GetState();

            if (lastChange + 110 < gameTime.TotalGameTime.TotalMilliseconds) { //add so that the buttons can be clicked aswell
                if (kState.IsKeyDown(Keys.Down)) { //moving down the menu
                    selected++;
                    if (selected > menu.Count - 1) { selected = 0; } //"selected" is bigger than menuList
                }
                if (kState.IsKeyDown(Keys.Up)) {  //moving up the menu
                    selected--;
                    if (selected < 0) { selected = menu.Count - 1; } //"selected" is smaller than menuList
                }
                //lastChange == now
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (kState.IsKeyDown(Keys.Enter) && !btnPressed) { //acknowledges the first buttonpress only
                btnPressed = true;
                return menu[selected].State; //userSelection
            }
            if (kState.IsKeyUp(Keys.Enter)) { btnPressed = false; } //resets btnPressed when releasing button

            //if no selection has been made, stay in the menu
            return defaultMenuState;
        }

        public void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < menu.Count; i++) {
                if (i == selected) { //selected menuOption
                    spriteBatch.Draw(menu[i].Texture, menu[i].Rec, Color.RosyBrown);
                }
                else { //others
                    spriteBatch.Draw(menu[i].Texture, menu[i].Rec, Color.White);
                }
            }
        }

        //Properties
        public int Selected { get { return selected; } set { selected = value; } }
        public bool BtnPressed { get { return btnPressed; } set { btnPressed = value; } }

    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace UserTextInputTest {
    class TextInput {

        //userTextInput-variables
        Keys[] pressedKeys;
        Keys[] lastPressedKeys;
        string inputText = string.Empty;
        SpriteFont sf;

        public TextInput(ContentManager content) {
            sf = content.Load<SpriteFont>(@"Images/SpriteFont1");
        }

        public void Update(GameTime gameTime) {
            readKeyBoard(gameTime);
        }

        float pressedKeysX = 0;
        float lastPressedKeysX = 0;
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(sf, inputText, Vector2.Zero, Color.White); //output

            pressedKeysX = 0;
            foreach (Keys key in pressedKeys) {
                spriteBatch.DrawString(sf, key.ToString(), new Vector2(pressedKeysX, 100), Color.White); //pressedKeysArray
                pressedKeysX += 15;
            }
            lastPressedKeysX = 0;

            foreach (Keys key in lastPressedKeys) {
                spriteBatch.DrawString(sf, key.ToString(), new Vector2(lastPressedKeysX, 200), Color.White); //lastPressedKeysArray
                lastPressedKeysX += 15;
            }

        }

        KeyboardState kState;
        bool keyFound;
        float lastTime;
        private void readKeyBoard(GameTime gameTime) {
            kState = Keyboard.GetState();

            pressedKeys = kState.GetPressedKeys();
            foreach (Keys key in pressedKeys) {
                keyFound = false;
                foreach (Keys lastKey in lastPressedKeys) {
                    if (key != lastKey) { keyFound = true; }
                }
                if (!keyFound) { KeyExceptions(key); }
            }
            lastPressedKeys = pressedKeys;
        }

        //inputText += key.ToString();
        private void KeyExceptions(Keys key) {
            if (key != Keys.LeftControl || key != Keys.LeftShift || key != Keys.Enter || key != Keys.LeftAlt || key != Keys.Tab || key != Keys.Escape) {
                switch (key) {
                    case Keys.Space:
                        inputText += "_";
                        break;
                    case Keys.Back:
                        if (inputText.Length <= 0) { break; }
                        else { inputText = inputText.Remove(inputText.Length - 1); }
                        break;
                    default:
                        inputText += key.ToString();
                        break;
                }
            }
        }

    }
}

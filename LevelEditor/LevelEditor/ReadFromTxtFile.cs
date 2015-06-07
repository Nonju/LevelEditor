using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
//Addons
using System.IO;


namespace LevelEditor {
    class ReadFromTxtFile {

        Vector2 spritePos;
        float moveDownByY;
        List<string> lines;
        SpriteFont sf;

        public ReadFromTxtFile(ContentManager content, GameWindow window, Vector2 basePos) {
            lines = new List<string>();
            this.spritePos = basePos;

            moveDownByY = window.ClientBounds.Height * 0.05f;
            sf = content.Load<SpriteFont>("SpriteFont2");

        }

        public void FileReader(string filePath) {
            StreamReader reader = new StreamReader(@"" + filePath, System.Text.Encoding.UTF8);
            lines.Clear();
            string line;
            while ((line = reader.ReadLine()) != null) {
                lines.Add(line);
            }
            reader.Close();
        }

        public void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < lines.Count; i++) {
                spritePos.Y = (i +1) * moveDownByY;
                spriteBatch.DrawString(sf, lines[i], spritePos, Color.White);
            }
            
        }

    }
}

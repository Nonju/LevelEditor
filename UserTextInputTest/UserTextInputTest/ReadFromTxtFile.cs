using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using System.IO;


namespace UserTextInputTest {
    class ReadFromTxtFile {

        Vector2 spritePos;
        float moveDownByY;
        List<string> lines;
        SpriteFont sf;


        public ReadFromTxtFile(ContentManager content, GameWindow window) {
            lines = new List<string>();

            spritePos = new Vector2(0, 0);
            moveDownByY = window.ClientBounds.Height * 0.05f;
            sf = content.Load<SpriteFont>(@"Images/SpriteFont1");

        }

        public void FileReader() {
            //StreamReader reader = new StreamReader(@"../XmlDocuments/TextFile.txt");
            StreamReader reader = new StreamReader(@"../XmlDocuments/Info.txt", System.Text.Encoding.UTF8);
            lines.Clear();
            string line;
            while ((line = reader.ReadLine()) != null) {
                lines.Add(line);
            }
            reader.Close();

        }


        public void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < lines.Count; i++) {
                spritePos.Y = 0 + (i * moveDownByY);
                spriteBatch.DrawString(sf, lines[i], spritePos, Color.White);
            }
        }


    }
}

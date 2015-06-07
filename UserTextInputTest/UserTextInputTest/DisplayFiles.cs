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
    class DisplayFiles {

        string[] files;
        string[] fileNames;
        //List<string> fileNames;
        SpriteFont sf;
        Vector2 spritePos;
        //string filePath = @"../XmlDocuments/Lvls/";
        string filePath = @"../../../../../LevelEditor/LevelEditor/bin/Windows/XmlDocuments/Lvls";

        public DisplayFiles(ContentManager content) {
            sf = content.Load<SpriteFont>(@"Images/SpriteFont1");
            spritePos = new Vector2(0,0);
            //fileNames = new string[3];
        }

        //string tempFileString = string.Empty;
        string[] tempFileString;
        public void FileDisplay() {

            try {
                files = Directory.GetFiles(filePath); //tar emot filer i en katalog
                fileNames = new string[files.Length];
                for (int i = 0; i < files.Length; i++) {
                    tempFileString = files[i].Split('\\');
                    fileNames[i] = tempFileString[tempFileString.Length - 1]; //adds the last array location to fileNames (It removes the remaining filePath)
                }

            }
            catch { }

        }
        //spritePos.Y += 10;
        public void Draw(SpriteBatch spriteBatch) {
             for (int i = 0; i < fileNames.Length; i++) {
                spritePos.Y = 0 + (i * 20);
                spriteBatch.DrawString(sf, fileNames[i], spritePos, Color.White);
             }
        }


    }
}

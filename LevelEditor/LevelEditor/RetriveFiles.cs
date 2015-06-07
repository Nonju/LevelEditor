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
    class RetriveFiles {

        public static string[] retrive(string folderPath) {
            string[] files = Directory.GetFiles(folderPath);
            return files;
        }


    }
}

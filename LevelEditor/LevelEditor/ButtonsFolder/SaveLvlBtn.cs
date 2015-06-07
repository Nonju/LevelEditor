using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
//Addons
using System.Xml;
using System.IO;
using System.Text;


namespace LevelEditor.ButtonsFolder {
    class SaveLvlBtn : Buttons {

        //Color btnColor;

        public SaveLvlBtn(Texture2D texture, Vector2 pos, float width, float height) : base(texture, pos, width, height) { }

        MouseState mState;
        bool lmbIsPressed = false;
        public override void OnClick(Blocks[,] lvlArray, string lvlName) { //saves current level to an XML-file
            //MouseCheck
            mState = Mouse.GetState();
            if (mState.X > pos.X && mState.X < (pos.X + width) && mState.Y > pos.Y && mState.Y < (pos.Y + height)) { // X/Y-axis
                if (mState.LeftButton == ButtonState.Pressed && !lmbIsPressed) { //saveBtn is pressed
                    lmbIsPressed = true;
                    //create xmlDoc with declaration
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    xmlDoc.AppendChild(xmlDecl);

                    //create element
                    XmlElement savedEditor = xmlDoc.CreateElement("savedEditor");
                    xmlDoc.AppendChild(savedEditor);

                    //editorMeasurements
                    XmlElement eMeasurements = xmlDoc.CreateElement("eMeasurements");
                    savedEditor.AppendChild(eMeasurements);

                    XmlElement eWidth = xmlDoc.CreateElement("eWidth");
                    eWidth.InnerText = lvlArray.GetLength(0).ToString();
                    eMeasurements.AppendChild(eWidth);

                    XmlElement eHeight = xmlDoc.CreateElement("eHeight");
                    eHeight.InnerText = lvlArray.GetLength(1).ToString();
                    eMeasurements.AppendChild(eHeight);

                    //levelData
                    for (int x = 0; x < lvlArray.GetLength(0); x++) {
                        for (int y = 0; y < lvlArray.GetLength(1); y++) {
                            //arrayBlockData
                            XmlElement block = xmlDoc.CreateElement("block");
                            savedEditor.AppendChild(block);
                            //block X-pos
                            XmlElement xPos = xmlDoc.CreateElement("xPos");
                            xPos.InnerText = x.ToString();
                            block.AppendChild(xPos);
                            //block Y-pos
                            XmlElement yPos = xmlDoc.CreateElement("yPos");
                            yPos.InnerText = y.ToString();
                            block.AppendChild(yPos);
                            //contentAdd
                            XmlElement content = xmlDoc.CreateElement("content");
                            content.InnerText = lvlArray[x, y].BlockName;
                            block.AppendChild(content);
                        }
                    }
                    xmlDoc.Save(@"../XmlDocuments/Lvls/" + lvlName + ".xml"); //save file
                }//end second if-statement
                else { lmbIsPressed = false; }
            }//end first if-statement
        }

    }
}

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

namespace LevelEditor {
    class Editor {

        //first time setup variables
        Blocks[,] workSpaceArr2D;
        Texture2D blockTexture;
        Vector2 startPos, blockPos;
        float editorWidth, editorHeight, workSpaceWidth;
        float blockWidth, blockHeight; // block measurements

        //variables
        string currentSelection;
        string blockImgBasePath;
        SpriteFont sf;

        //cursor
        Game1 game; // for changing mousevisibility
        Texture2D cursorTexture;
        Vector2 cursorPos;
        float cursorWidth, cursorHeight;
        Rectangle cursorRec;

        //buttons
        Buttons saveLvlBtn;
        Texture2D saveBtnTexture;
        Vector2 saveBtnPos;
        float saveBtnWidth, saveBtnHeight;

        bool editFileNameBool = false;
        Buttons editFileName;
        Texture2D editNameBtnTexture;
        Vector2 editNameBtnPos;
        float editNameBtnWidth, editNameBtnHeight;

        bool backToMenuBool;
        Buttons backToMenu;
        Texture2D backToMenuTexture;
        Vector2 backToMenuPos;
        float backToMenuWidth, backToMenuHeight;

        //userTextInput-variables
        Keys[] pressedKeys;
        Keys[] lastPressedKeys;
        string lvlName = string.Empty;
        Vector2 lvlNameInputPos;
        SpriteFont lvlNameInput;

        //toolbox setup
        TbBlock[] tbBlocks;

        


        public Editor(ContentManager content, GameWindow window, Game1 game) { 
            startPos = new Vector2(0, (window.ClientBounds.Height * 0.1f));
            //currentSelection = "MouseCursor";
            currentSelection = "EmptyBlock";
            blockImgBasePath = "Images/BlockImages/";
            backToMenuBool = false;

            //cursor
            this.game = game; //get game properties
            cursorWidth = window.ClientBounds.Width * 0.03f;
            cursorHeight = window.ClientBounds.Height * 0.05f;
            cursorRec = new Rectangle((int)cursorPos.X, (int)cursorPos.Y, (int)cursorWidth, (int)cursorHeight);
            
            //initial load of empty editor
            blockTexture = content.Load<Texture2D>(@"Images/BlockImages/EmptyBlockTexture.png");
            editorWidth = window.ClientBounds.Width;
            editorHeight = window.ClientBounds.Height - startPos.Y;
            workSpaceWidth = (editorWidth * 0.8f);

            //buttons
            float editorBtnHeight = window.ClientBounds.Height * 0.07f;

            saveBtnTexture = content.Load<Texture2D>(@"Images/BtnImages/Temp/TempSaveBtn.bmp");
            saveBtnPos = new Vector2((window.ClientBounds.Width * 0.67f), (window.ClientBounds.Height * 0.01f));
            saveBtnWidth = window.ClientBounds.Width * 0.1f;
            saveBtnHeight = editorBtnHeight;
            saveLvlBtn = new ButtonsFolder.SaveLvlBtn(saveBtnTexture, saveBtnPos, saveBtnWidth, saveBtnHeight);

            editNameBtnTexture = content.Load<Texture2D>(@"Images/BtnImages/Temp/TempEditFileNamebtn.bmp");
            editNameBtnPos = new Vector2((window.ClientBounds.Width * 0.4f), (window.ClientBounds.Height * 0.01f));
            editNameBtnWidth = window.ClientBounds.Width * 0.12f;
            editNameBtnHeight = editorBtnHeight;
            editFileName = new ButtonsFolder.BasicBtn(editNameBtnTexture, editNameBtnPos, editNameBtnWidth, editNameBtnHeight);

            backToMenuTexture = content.Load<Texture2D>(@"Images/BtnImages/Temp/TempBackToMenuBtn.bmp");
            backToMenuPos = new Vector2((window.ClientBounds.Width * 0.55f), (window.ClientBounds.Height * 0.01f));
            backToMenuWidth = window.ClientBounds.Width * 0.1f;
            backToMenuHeight = editorBtnHeight;
            backToMenu = new ButtonsFolder.BasicBtn(backToMenuTexture, backToMenuPos, backToMenuWidth, backToMenuHeight);

            //userTextInput
            lvlNameInput = content.Load<SpriteFont>("SpriteFont2");
            lvlNameInputPos = new Vector2(0, window.ClientBounds.Height * 0.05f);

            //spritefonts
            sf = content.Load<SpriteFont>("SpriteFont2");

        }
        MouseState mState;
        bool withinWorkspace = false;
        public void Update(ContentManager content) {

            mState = Mouse.GetState();
            #region Click/placement-handling
            if (mState.LeftButton == ButtonState.Pressed) { //handling most leftClick-actions
                currentSelection = ToolboxSelection(mState);
                PlaceBlock(content, mState);
            }
            #endregion

            #region Buttons
            //SaveButton
            if (editFileName.IsClicked(mState)) { editFileNameBool = !editFileNameBool; }
            if (editFileNameBool) { readKeyBoard(); }
            if (lvlName.Length > 0) { saveLvlBtn.OnClick(workSpaceArr2D, lvlName); } //makes sure the user does not save a lvl without name
            if (backToMenu.IsClicked(mState)) { backToMenuBool = true; }
            //ReturnToMenuButton
            #endregion

            #region Mouse Cursor
            if (mState.X < (startPos.X + workSpaceWidth) && mState.X > startPos.X && mState.Y < editorHeight && mState.Y > startPos.Y) { withinWorkspace = true; }
            else { withinWorkspace = false; }
            //removes "pointer"-cursor when inside workspace

            if (withinWorkspace) { 
                game.IsMouseVisible = false;
                cursorTexture = content.Load<Texture2D>(@"" + blockImgBasePath + currentSelection + "Texture.png");
                //cursorTexture = content.Load<Texture2D>(@"Images/BlockImages/DirtBlockTexture.png");
                cursorPos.X = mState.X - (cursorWidth / 2);
                cursorPos.Y = mState.Y - (cursorHeight / 2);
                //Update cursorRecPos
                cursorRec.X = (int)cursorPos.X;
                cursorRec.Y = (int)cursorPos.Y;
            }
            else { game.IsMouseVisible = true; }

            #endregion

        }

        public void Draw(SpriteBatch spriteBatch) {
            //WorkSpaceDraw
            foreach (Blocks b in workSpaceArr2D) { b.Draw(spriteBatch); }
            //ToolboxDraw
            foreach (TbBlock tb in tbBlocks) { tb.Draw(spriteBatch); }
            //CursorDraw
            if (withinWorkspace) { spriteBatch.Draw(cursorTexture, cursorRec, Color.LightGray); } //draws the cursor when within the workspace
            //Draw buttons
            saveLvlBtn.Draw(spriteBatch, Color.LightBlue, mState);
            editFileName.Draw(spriteBatch, Color.LightBlue, mState);
            backToMenu.Draw(spriteBatch, Color.LightBlue, mState);
            //lvlNameInputbox
            spriteBatch.DrawString(lvlNameInput, "LevelName: " + lvlName, lvlNameInputPos, Color.Black); //lvlNameInputPos

            spriteBatch.DrawString(sf, currentSelection, Vector2.Zero, Color.White);
        }

        #region Create/Load/Place/Select-section
        //Builds an empty editor
        public void CreateEmptyEditor(ContentManager content, int editorX, int editorY) { //removed "Vector2 pos"
            lvlName = string.Empty; //empties the current lvlName-string
            #region WorkSpace-load
            //float blockWidth, blockHeight; // block measurements
            blockWidth = workSpaceWidth / editorX;
            blockHeight = editorHeight / editorY;
            //workSpace
            workSpaceArr2D = new Blocks[editorX, editorY]; //create new blockContainer
            for (int y = 0; y < editorY; y++) {
                for (int x = 0; x < editorX; x++) { 
                    //calc blockPos
                    blockPos.X = startPos.X + (x * blockWidth);
                    blockPos.Y = startPos.Y + (y * blockHeight);
                    Blocks tempBlock = new BlocksFolder.EmptyBlock(blockTexture, blockPos, blockWidth, blockHeight, null);
                    workSpaceArr2D[x, y] = tempBlock;
                }
            }
            #endregion

            #region Xml-load
            LoadToolbox(content, editorWidth, editorHeight, workSpaceWidth);
            #endregion
        }
        public void LoadExistingEditor(ContentManager content , string lvlName) { //removed "int editorX, int editorY"
            this.lvlName = lvlName;
            
            #region workSpace-load
            XmlDocument blockDoc = new XmlDocument();
            //load from file
            blockDoc.Load(@"../XmlDocuments/Lvls/" + lvlName + ".xml");
            
            //retrive workSpace measurements
            int editorX, editorY; //width, height of editor
            XmlNodeList wsMeasurements = blockDoc.SelectNodes("savedEditor/eMeasurements");
            editorX = Convert.ToInt32(wsMeasurements[0].SelectSingleNode("eWidth").InnerText);
            editorY = Convert.ToInt32(wsMeasurements[0].SelectSingleNode("eHeight").InnerText);
            workSpaceArr2D = new Blocks[editorX, editorY]; //creates new blockContainer
            //float blockWidth, blockHeight; // block measurements
            blockWidth = workSpaceWidth / editorX;
            blockHeight = editorHeight / editorY;

            //create workspace
            XmlNodeList blockList = blockDoc.SelectNodes("savedEditor/block");

            int x, y;
            string nodeContent, blockImgPath;
            Blocks tempBlock;
            foreach (XmlNode blockNode in blockList) {
                x = Convert.ToInt32(blockNode.SelectSingleNode("xPos").InnerText);
                y = Convert.ToInt32(blockNode.SelectSingleNode("yPos").InnerText);
                blockPos.X = startPos.X + (x * blockWidth);
                blockPos.Y = startPos.Y + (y * blockHeight);
                //retrive content from node
                nodeContent = blockNode.SelectSingleNode("content").InnerText;
                blockImgPath = string.Format("{0}{1}{2}", blockImgBasePath, nodeContent, "Texture.png");
                switch (nodeContent) {
                    case "EmptyBlock":
                        tempBlock = new BlocksFolder.EmptyBlock(content.Load<Texture2D>(@"" + blockImgPath), blockPos, blockWidth, blockHeight, nodeContent);
                        break;
                    case "GrassBlock":
                        tempBlock = new BlocksFolder.GrassBlock(content.Load<Texture2D>(@"" + blockImgPath), blockPos, blockWidth, blockHeight, nodeContent);
                        break;
                    case "StoneBlock":
                        tempBlock = new BlocksFolder.StoneBlock(content.Load<Texture2D>(@"" + blockImgPath),blockPos,blockWidth,blockHeight,nodeContent);
                        break;
                    case "DirtBlock":
                        tempBlock = new BlocksFolder.DirtBlock(content.Load<Texture2D>(@"" + blockImgPath), blockPos, blockWidth, blockHeight, nodeContent);
                        break;
                    default:
                        tempBlock = new BlocksFolder.EmptyBlock(content.Load<Texture2D>(@"" + blockImgPath), blockPos, blockWidth, blockHeight, nodeContent);
                        break;
                }
                workSpaceArr2D[x, y] = tempBlock; //adding loaded block to workSpaceArray
            }
            #endregion

            #region Xml-load
            LoadToolbox(content, editorWidth, editorHeight, workSpaceWidth);
            #endregion
        }


        private void LoadToolbox(ContentManager content, float width, float height, float workSpaceWidth) {
            XmlDocument blockDoc = new XmlDocument();
            //load from file
            blockDoc.Load(@"../XmlDocuments/blockinfo.xml");
            //create array with xmlDoc-nodes
            XmlNodeList blockList = blockDoc.SelectNodes("blocks/block");

            float toolboxHeight = height / blockList.Count; //height of block/toolbox-area
            int nrOfBlocks = 0;
            tbBlocks = new TbBlock[blockList.Count];
            foreach (XmlNode blockNode in blockList) { 
                //retrive necessary info
                string blockName = blockNode.SelectSingleNode("blockName").InnerText;
                string blockTextureName = blockNode.SelectSingleNode("imgTextureName").InnerText;
                float tbHeightPos = (height / (float)blockList.Count) * nrOfBlocks;
                Texture2D tempTexture = content.Load<Texture2D>(@"Images/BlockImages/" + blockTextureName);
                Vector2 tempPos = new Vector2(workSpaceWidth, tbHeightPos);

                TbBlock temp = new TbBlock(blockName, tempTexture, tempPos, (width - workSpaceWidth), toolboxHeight);
                tbBlocks[nrOfBlocks] = temp; //uses "nrOfBlocks" as an index because they will be the same NR anyways

                nrOfBlocks++; //increases the nrOfBlocks counter
            }
            
        }

        public string ToolboxSelection(MouseState mState) {
            string lastSelection = currentSelection;
            for (int i = 0; i < tbBlocks.Length; i++) { 
                //checks if the mouse is within the block-area
                if (mState.X > tbBlocks[i].Pos.X && mState.X < (tbBlocks[i].Pos.X + tbBlocks[i].Width)) { // X-axis
                    if (mState.Y > tbBlocks[i].Pos.Y && mState.Y < (tbBlocks[i].Pos.Y + tbBlocks[i].Height)) { // Y-axis
                        return tbBlocks[i].Name;
                    }
                }
            }
            //if ToolboxBlock == not clicked
            return lastSelection;
        }

        //Changes the current block in the editor to currentSelection
        public void PlaceBlock(ContentManager content, MouseState mState) { 
            //retrive info from xmlDoc
            XmlDocument blockDoc = new XmlDocument();
            //load from file
            blockDoc.Load(@"../XmlDocuments/blockinfo.xml");
            //create array with xml-nodes
            XmlNodeList blockList = blockDoc.SelectNodes("blocks/block");

            for (int x = 0; x < workSpaceArr2D.GetLength(0); x++) {
                for (int y = 0; y < workSpaceArr2D.GetLength(1); y++) { 
                    //checks if a block in the editor is clicked
                    if (mState.X > workSpaceArr2D[x, y].Pos.X && mState.X < (workSpaceArr2D[x, y].Pos.X + workSpaceArr2D[x, y].Width)) { //X-axis
                        if (mState.Y > workSpaceArr2D[x, y].Pos.Y && mState.Y < (workSpaceArr2D[x, y].Pos.Y + workSpaceArr2D[x, y].Height)) { //Y-axid
                            //code for placing block
                            foreach (XmlNode node in blockList) {
                                string blockName = node.SelectSingleNode("blockName").InnerText;
                                if (currentSelection == blockName) {
                                    string blockTexture = node.SelectSingleNode("imgTextureName").InnerText;

                                    //saves necessary values from last block
                                    Texture2D tempTexture = content.Load<Texture2D>(@"Images/BlockImages/" + blockTexture);
                                    Vector2 tempVector = workSpaceArr2D[x, y].Pos;
                                    float tempWidth = workSpaceArr2D[x, y].Width;
                                    float tempHeight = workSpaceArr2D[x, y].Height;
                                    //create new block to replace old one
                                    Blocks temp;
                                    switch (currentSelection) { 
                                        case "EmptyBlock":
                                            temp = new BlocksFolder.EmptyBlock(tempTexture, tempVector, tempWidth, tempHeight, blockName);
                                            break;
                                        case "GrassBlock":
                                            temp = new BlocksFolder.GrassBlock(tempTexture, tempVector, tempWidth, tempHeight, blockName);
                                            break;
                                        case "StoneBlock":
                                            temp = new BlocksFolder.StoneBlock(tempTexture, tempVector, tempWidth, tempHeight, blockName);
                                            break;
                                        case "DirtBlock":
                                            temp = new BlocksFolder.DirtBlock(tempTexture, tempVector, tempWidth, tempHeight, blockName);
                                            break;
                                        default:
                                            temp = new BlocksFolder.EmptyBlock(tempTexture, tempVector, tempWidth, tempHeight, blockName);
                                            break;
                                    }

                                    workSpaceArr2D[x, y] = temp; //adds new block to arrayPos
                                }
                            }//end foreach-loop
                        }
                    }
                }
            } //end for-loop

        }//end PlaceBlock-method
        #endregion

        #region InputText from keyboard
        KeyboardState kState;
        bool keyFound;
        int maxNrOfChars = 15;
        private void readKeyBoard() {
            kState = Keyboard.GetState();
            pressedKeys = kState.GetPressedKeys();

            if (lvlName.Length > maxNrOfChars) { lvlName = lvlName.Remove(maxNrOfChars); }
            else { 
                foreach (Keys key in pressedKeys) {
                    keyFound = false;
                    if (lastPressedKeys != null) {
                        foreach (Keys lastKey in lastPressedKeys) {
                            if (key == lastKey) { keyFound = true; } //&& key != Keys.Back
                            else { break; }
                        }
                    }
                    if (!keyFound) { KeyExceptions(key); }
                }
                lastPressedKeys = pressedKeys;
            }
        }

        bool notANr = true;
        private void KeyExceptions(Keys key) { //Exceptions for text-input
            if (key != Keys.LeftControl || key != Keys.LeftShift || key != Keys.Enter || key != Keys.LeftAlt || key != Keys.Tab || key != Keys.Escape) {
                for (int i = 0; i <= 10; i++) {
                    if (key.ToString() == "D" + i.ToString()) { 
                        lvlName += i.ToString(); 
                        notANr = false; 
                        break; 
                    }
                    else { notANr = true; }
                }
                if (notANr) {
                    switch (key) {
                        case Keys.Space:
                            lvlName += "_";
                            break;
                        case Keys.Back:
                            if (lvlName.Length <= 0) { break; }
                            else { lvlName = lvlName.Remove(lvlName.Length - 1); }
                            break;
                        default:
                            lvlName += key.ToString().ToLower();
                            break;
                    }
                }
            }
        }//end KeyExceptions
        #endregion

        //Properties
        public bool BackToMenu { get { return backToMenuBool; } set { backToMenuBool = value; } }

    }//end class-Editor

    class TbBlock { //toolboxBlock

        string name;
        Texture2D texture;
        Vector2 pos;
        float width, height;
        Rectangle rec;

        public TbBlock(string blockName, Texture2D texture, Vector2 btnPos, float width, float height) {
            this.name = blockName;
            this.texture = texture;
            this.pos = btnPos;
            this.width = width;
            this.height = height;
            rec = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rec, Color.White);
        }

        //Properties
        public string Name { get { return name; } }
        public Vector2 Pos { get { return pos; } }
        public float Width { get { return width; } }
        public float Height { get { return height; } }
    
    }//end class-TbBlock
}

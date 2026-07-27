using System;
using System.Diagnostics;
using Lidgren.Network;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;

namespace AzulNetworkBase
{

    class BasicGame : Azul.Game
    {
        public const string WindowCaption = "Basic Game";
        public const int Width = 800;
        public const int Height = 600;

        Tank P1;
        Tank P2;

        Azul.Texture TankSheet;
        Azul.Sprite GreenTankBase;
        Azul.Sprite OrangeTankBase;

        //-----------------------------------------------------------------------------
        // Game::Initialize()
        //		Allows the engine to perform any initialization it needs to before 
        //      starting to run.  This is where it can query for any required services 
        //      and load any non-graphic related content. 
        //-----------------------------------------------------------------------------
        public override void Initialize()
        {
            // Game Window Device setup
            this.SetWindowName(WindowCaption);
            this.SetWidthHeight(Width, Height);
            this.SetClearColor(0.4f, 0.4f, 0.8f, 1.0f);
        }

        //-----------------------------------------------------------------------------
        // Game::LoadContent()
        //		Allows you to load all content needed for your engine,
        //	    such as objects, graphics, etc.
        //-----------------------------------------------------------------------------
        public override void LoadContent()
        {
            Program.AdjustWindow();

            TankSheet = new Azul.Texture("Tanks.tga");
            GreenTankBase = new Azul.Sprite(TankSheet, new Azul.Rect(208, 82, 37, 35), new Azul.Rect(200, 200, 50, 50));
            OrangeTankBase = new Azul.Sprite(TankSheet, new Azul.Rect(253, 82, 37, 35), new Azul.Rect(400, 200, 50, 50));

            P1 = new Tank(GreenTankBase, 200, 300);
            P2 = new Tank(OrangeTankBase, 600, 300);
        }

        //-----------------------------------------------------------------------------
        // Game::Update()
        //      Called once per frame, update data, tranformations, etc
        //      Use this function to control process order
        //      Input, AI, Physics, Animation, and Graphics
        //-----------------------------------------------------------------------------
        public override void Update()
        {
            InputManager.Update();
            //InputTest.AxisTest();

            // Get Inputs
            int x1 = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P1);
            int y1 = InputManager.GetAxis(INPUTAXIS.VERTICAL_P1);
            int x2 = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P2);
            int y2 = InputManager.GetAxis(INPUTAXIS.VERTICAL_P2);

            // Update objects
            P1.Move(x1, y1);
            P2.Move(x2, y2);
        }

        //-----------------------------------------------------------------------------
        // Game::Draw()
        //		This function is called once per frame
        //	    Use this for draw graphics to the screen.
        //      Only do rendering here
        //-----------------------------------------------------------------------------
        public override void Draw()
        {
            P1.render();
            P2.render();
        }

        //-----------------------------------------------------------------------------
        // Game::UnLoadContent()
        //       unload content (resources loaded above)
        //       unload all content that was loaded before the Engine Loop started
        //-----------------------------------------------------------------------------
        public override void UnLoadContent()
        {

        }

    }
}


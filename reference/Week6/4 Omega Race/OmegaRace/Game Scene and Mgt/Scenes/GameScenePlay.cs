using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OmegaRace
{
    public class GameScenePlay : IGameScene
    {
        public PlayerManager PlayerMgr { get; }
        public MessageQueueManager MsgQueueMgr { get; private set; }

        DisplayManager DisplayMgr;

        public GameScenePlay()
        {
            PlayerMgr = new PlayerManager();
            MsgQueueMgr = new MessageQueueManager();
            DisplayMgr = new DisplayManager();
        }

        void IGameScene.Enter()
        {
            LoadLevel();
        }
        void IGameScene.Update()
        { 
            // First, update the physics engine
            PhysicWorld.Update();

            //Queue processing goes here
            MsgQueueMgr.Process();

            // Process reactions to inputs for Player 1
            int p1_H = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P1);
            int p1_V = InputManager.GetAxis(INPUTAXIS.VERTICAL_P1);
            PlayerMgr.P1Data.ship.Rotate(p1_H);
            PlayerMgr.P1Data.ship.Move(p1_V);
            if (InputManager.GetButtonDown(INPUTBUTTON.P1_FIRE))
            {
                PlayerMgr.P1Data.FireMissile();
            }
            if (InputManager.GetButtonDown(INPUTBUTTON.P1_LAYMINE))
            {
                PlayerMgr.P1Data.LayMine();
            }

            // Process reactions to inputs for Player 2
            int p2_H = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P2);
            int p2_V = InputManager.GetAxis(INPUTAXIS.VERTICAL_P2);
            //* Direct call version
            PlayerMgr.P2Data.ship.Rotate(p2_H);
            PlayerMgr.P2Data.ship.Move(p2_V);
            //*/
            /* Data driven example for P2 movement
            DataMessage msg1 = new DataMessage();
            msg1.horzInput = p2_H;
            msg1.vertInput = p2_V;
            MsgQueueMgr.AddToOutputQueue(msg1);
            //*/

            if (InputManager.GetButtonDown(INPUTBUTTON.P2_FIRE))
            {
                PlayerMgr.P2Data.FireMissile();
            }

            if (InputManager.GetButtonDown(INPUTBUTTON.P2_LAYMINE))
            {
                PlayerMgr.P2Data.LayMine();
            }


            /* Screen log example
            ScreenLog.Add(string.Format("Frame Time: {0:0.0}", 1 / TimeManager.GetFrameTime()));
            ScreenLog.Add(Colors.DarkKhaki, string.Format("P1 ammo: {0}", PlayerMgr.P1Data.missileCount));
            ScreenLog.Add(Colors.Orchid, string.Format("P2 ammo: {0}", PlayerMgr.P2Data.missileCount));
            //*/
        }
        void IGameScene.Draw()
        {
            DisplayMgr.DisplayHUD(PlayerMgr.P1Data, PlayerMgr.P2Data);
        }
        void IGameScene.Leave()
        {
            
        }

        void LoadLevel()
        {
            GameManager.AddGameObject(PlayerMgr.P1Data.ship);
            GameManager.AddGameObject(PlayerMgr.P2Data.ship);

            // Fence OutsideBox

            GameManager.AddGameObject(new Fence(new Azul.Rect(100, 5, 8, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(300, 5, 8, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(500, 5, 8, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(700, 5, 8, 200), 90));
  
            GameManager.AddGameObject(new Fence(new Azul.Rect(100, 495, 8, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(300, 495, 8, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(500, 495, 8, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(700, 495, 8, 200), 90));

            GameManager.AddGameObject(new Fence(new Azul.Rect(5, 125, 8, 250), 0));
            GameManager.AddGameObject(new Fence(new Azul.Rect(5, 375, 8, 250), 0));
            GameManager.AddGameObject(new Fence(new Azul.Rect(795, 125, 8, 250), 0));
            GameManager.AddGameObject(new Fence(new Azul.Rect(795, 375, 8, 250), 0));

            // Fence InsideBox
            GameManager.AddGameObject(new Fence(new Azul.Rect(300, 170, 10, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(500, 170, 10, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(300, 330, 10, 200), 90));
            GameManager.AddGameObject(new Fence(new Azul.Rect(500, 330, 10, 200), 90));

            GameManager.AddGameObject(new Fence(new Azul.Rect(200, 250, 10, 160), 0));
            GameManager.AddGameObject(new Fence(new Azul.Rect(600, 250, 10, 160), 0));


            // OutsideBox
            GameManager.AddGameObject(new FencePost(new Azul.Rect(5, 5, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(200, 5, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(400, 5, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(600, 5, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(800 - 5, 5, 10, 10)));

            GameManager.AddGameObject(new FencePost(new Azul.Rect(0 + 5, 495, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(200, 495, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(400, 495, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(600, 495, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(800 - 5, 495, 10, 10)));

            GameManager.AddGameObject(new FencePost(new Azul.Rect(5, 250, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(795, 250, 10, 10)));

            // InsideBox

            GameManager.AddGameObject(new FencePost(new Azul.Rect(200, 170, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(400, 170, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(600, 170, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(200, 330, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(400, 330, 10, 10)));
            GameManager.AddGameObject(new FencePost(new Azul.Rect(600, 330, 10, 10)));
        }
    }
}

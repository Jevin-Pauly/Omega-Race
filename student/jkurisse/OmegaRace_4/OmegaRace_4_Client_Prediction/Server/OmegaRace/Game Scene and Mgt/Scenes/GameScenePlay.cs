using OmegaRace.Data_Queues;

namespace OmegaRace
{
    public class GameScenePlay : IGameScene
    {
        public NetworkManager NetMgr;
        public PlayerManager PlayerMgr { get; }
        public MessageQueueManager MsgQueueMgr { get; private set; }

        DisplayManager DisplayMgr;
        DataMessage dataMsg;
        public bool isServer = true; // server!
        float timeSyncInterval = 5.0f;
        float timeSinceLastSync = 0.0f;
        public GameScenePlay()
        {
            PlayerMgr = new PlayerManager();
            MsgQueueMgr = new MessageQueueManager(MessageQueueStrategy.Mode.Normal);
            //MsgQueueMgr = new MessageQueueManager(MessageQueueStrategy.Mode.Record);
            //MsgQueueMgr = new MessageQueueManager(MessageQueueStrategy.Mode.Playback);
            DisplayMgr = new DisplayManager();
        }

        void IGameScene.Enter()
        {
            LoadLevel();
            TimeManager.SetTime(0);
            //NetMgr = NetworkManager.InitServer(8888);
            if (isServer)
                NetMgr = NetworkManager.InitServer(8888);
            else
                NetMgr = NetworkManager.JoinServer("127.0.0.1", 8888);
        }
        void IGameScene.Update()
        {
            // First, update the physics engine
            if (isServer)
            {
                PhysicWorld.Update(); // physics only on server
            }
            NetMgr.Process(MsgQueueMgr);
            //Queue processing goes here
            MsgQueueMgr.Process();

            int ch = 1;
            if (isServer)
            {
                HandlePlayer2Input(); // server-controlled
                BroadcastPosRot(ref ch);
                BroadcastMissiles(ref ch);
            }
            else
            {
                HandlePlayer1Input(); // client-controlled

                // sync every 5 seconds
                timeSinceLastSync += TimeManager.GetFrameTime();
                if (timeSinceLastSync >= timeSyncInterval)
                {
                    MsgQueueMgr.AddToOutputQueue(ServerTimeRequest.Create());
                    timeSinceLastSync = 0.0f;
                }
            }

            // Screen log
            DisplayHUDInfo();
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

        void HandlePlayer1Input()
        {
            MsgQueueMgr.AddToOutputQueue(MovementMessage.Create(DataMessage.PlayerID.Player1,
                                                                InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P1),
                                                                InputManager.GetAxis(INPUTAXIS.VERTICAL_P1)));

            if (InputManager.GetButtonDown(INPUTBUTTON.P1_FIRE))
                MsgQueueMgr.AddToOutputQueue(FireMessage.Create(DataMessage.PlayerID.Player1));

            if (InputManager.GetButtonDown(INPUTBUTTON.P1_LAYMINE))
                MsgQueueMgr.AddToOutputQueue(MineMessage.Create(DataMessage.PlayerID.Player1));
        }

        void HandlePlayer2Input()
        {
            MsgQueueMgr.AddToOutputQueue(MovementMessage.Create(DataMessage.PlayerID.Player2,
                                                                InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P2),
                                                                InputManager.GetAxis(INPUTAXIS.VERTICAL_P2)));

            if (InputManager.GetButtonDown(INPUTBUTTON.P2_FIRE))
                MsgQueueMgr.AddToOutputQueue(FireMessage.Create(DataMessage.PlayerID.Player2));

            if (InputManager.GetButtonDown(INPUTBUTTON.P2_LAYMINE))
                MsgQueueMgr.AddToOutputQueue(MineMessage.Create(DataMessage.PlayerID.Player2));
        }

        void BroadcastPosRot(ref int ch)
        {
            Ship p1 = PlayerMgr.P1Data.ship;
            Ship p2 = PlayerMgr.P2Data.ship;

            MsgQueueMgr.AddToOutputQueue(PosRotMessage.Create(p1.getID(), p1.GetPixelPosition(), p1.GetAngle_Deg(), ch++));
            MsgQueueMgr.AddToOutputQueue(PosRotMessage.Create(p2.getID(), p2.GetPixelPosition(), p2.GetAngle_Deg(), ch++));
        }

        void BroadcastMissiles(ref int ch)
        {
            foreach (Missile m in PlayerMgr.P1Data.missileList)
                MsgQueueMgr.AddToOutputQueue(PosRotMessage.Create(m.getID(), m.GetPixelPosition(), m.GetAngle_Deg(), ch++));

            foreach (Missile m in PlayerMgr.P2Data.missileList)
                MsgQueueMgr.AddToOutputQueue(PosRotMessage.Create(m.getID(), m.GetPixelPosition(), m.GetAngle_Deg(), ch++));
        }

        void DisplayHUDInfo()
        {
            //ScreenLog.Add(string.Format("Frame Time: {0:0.0}", 1 / TimeManager.GetFrameTime()));

            ScreenLog.Add($"Server Time: {TimeManager.GetCurrentTime()}");
            ScreenLog.Add(Colors.DarkKhaki, $"P1 ammo: {PlayerMgr.P1Data.missileCount}");
            ScreenLog.Add(Colors.Orchid, $"P2 ammo: {PlayerMgr.P2Data.missileCount}");
        }
    }
}

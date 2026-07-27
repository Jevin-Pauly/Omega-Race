using Box2DX.Common;
using Lidgren.Network;
using OmegaRace.Data_Queues;
using System;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    [Serializable]
    public class PosRotMessage : DataMessage
    {
        public int gameObjID;
        public float x;
        public float y;
        public float angle;

        public PosRotMessage()
        {
            this.msgType = MessageType.PosRot;
        }

        public static PosRotMessage Create(int id, Vec2 pos, float angle, int channel)
        {
            PosRotMessage msg = MessagePool<PosRotMessage>.Get();
            msg.msgType = MessageType.PosRot;
            msg.deliveryMethod = NetDeliveryMethod.ReliableSequenced;
            msg.destination = DestinationType.Network;
            msg.channel = channel;
            msg.gameObjID = id;
            msg.x = pos.X;
            msg.y = pos.Y;
            msg.angle = angle;
            return msg;
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(gameObjID);
            writer.Write(x);
            writer.Write(y);
            writer.Write(angle);
        }

        public static new PosRotMessage Deserialize(ref BinaryReader reader)
        {
            PosRotMessage msg = MessagePool<PosRotMessage>.Get();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
            msg.destination = (DestinationType)reader.ReadInt32();
            msg.deliveryMethod = (NetDeliveryMethod)reader.ReadInt32();
            msg.channel = reader.ReadInt32();
            msg.gameObjID = reader.ReadInt32();
            msg.x = reader.ReadSingle();
            msg.y = reader.ReadSingle();
            msg.angle = reader.ReadSingle();
            return msg;
        }

        public override void Execute()
        {
            GameObject obj = GameManager.Find(gameObjID);
            if (obj != null)
            {
                obj.SetPosAndAngle(x, y, angle);

                //if (obj.type == GAMEOBJECT_TYPE.SHIP)
                //    ((Ship)obj).prediction.Update(x, y);
                //else if (obj.type == GAMEOBJECT_TYPE.MISSILE)
                //    ((Missile)obj).prediction.Update(x, y);
            }

            Recycle();
        }

        public override void Recycle()
        {
            MessagePool<PosRotMessage>.Recycle(this);
        }

        public override void PrintMe()
        {
            Debug.WriteLine($"[PosRotMessage] ID: {gameObjID}, Pos: ({x}, {y}), Angle: {angle}");
        }
    }
}

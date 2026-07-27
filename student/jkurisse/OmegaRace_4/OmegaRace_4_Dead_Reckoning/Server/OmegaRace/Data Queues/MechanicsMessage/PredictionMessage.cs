using Box2DX.Common;
using Lidgren.Network;
using OmegaRace.Data_Queues;
using System;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    [Serializable]
    public class PredictionMessage : DataMessage
    {
        public int gameObjID;
        public float posX, posY;
        public float velX, velY;
        public float timeStamp;

        public PredictionMessage()
        {
            this.msgType = MessageType.Prediction;
            this.channel = 0;
        }

        public static PredictionMessage Create(int id, Vec2 pos, Vec2 vel, float time, int channel)
        {
            PredictionMessage msg = MessagePool<PredictionMessage>.Get();
            msg.msgType = MessageType.Prediction;
            msg.deliveryMethod = NetDeliveryMethod.UnreliableSequenced; // could change
            msg.destination = DestinationType.Network;
            msg.gameObjID = id;
            msg.posX = pos.X;
            msg.posY = pos.Y;
            msg.velX = vel.X;
            msg.velY = vel.Y;
            msg.timeStamp = time;
            msg.channel = channel;
            return msg;
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(gameObjID);
            writer.Write(posX);
            writer.Write(posY);
            writer.Write(velX);
            writer.Write(velY);
            writer.Write(timeStamp);
        }

        public static new PredictionMessage Deserialize(ref BinaryReader reader)
        {
            PredictionMessage msg = MessagePool<PredictionMessage>.Get();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
            msg.destination = (DestinationType)reader.ReadInt32();
            msg.deliveryMethod = (NetDeliveryMethod)reader.ReadInt32();
            msg.channel = reader.ReadInt32();

            msg.gameObjID = reader.ReadInt32();
            msg.posX = reader.ReadSingle();
            msg.posY = reader.ReadSingle();
            msg.velX = reader.ReadSingle();
            msg.velY = reader.ReadSingle();
            msg.timeStamp = reader.ReadSingle();

            return msg;
        }

        public override void Execute()
        {
            // Update prediction state
            GameObject obj = GameManager.Find(gameObjID);

            if (obj != null)
            {
                Vec2 pos = new Vec2(posX, posY);
                Vec2 vel = new Vec2(velX, velY);
                if (obj.type == GAMEOBJECT_TYPE.SHIP)
                    ((Ship)obj).prediction.UpdateFromServer(pos, vel, timeStamp);
                else if (obj.type == GAMEOBJECT_TYPE.MISSILE)
                    ((Missile)obj).prediction.UpdateFromServer(pos, vel, timeStamp);
                else
                    Debug.WriteLine($"[PredictionMessage] Unsupported object type for prediction: {obj.type}");  // No prediction
            }

            Recycle();
        }
        public override void Recycle()
        {
            MessagePool<PredictionMessage>.Recycle(this);
        }
    }
}

using Lidgren.Network;
using OmegaRace.Data_Queues;
using System;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    [Serializable]
    public class CollisionMessage : DataMessage
    {
        public int objectID1;
        public int objectID2;

        public CollisionMessage()
        {
            this.msgType = MessageType.Collision;
        }

        public static CollisionMessage Create(int id1, int id2)
        {
            CollisionMessage msg = MessagePool<CollisionMessage>.Get();
            msg.msgType = MessageType.Collision;
            msg.deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            msg.destination = DestinationType.Both;
            msg.channel = 13;
            msg.objectID1 = id1;
            msg.objectID2 = id2;
            return msg;
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(objectID1);
            writer.Write(objectID2);
        }

        public static new CollisionMessage Deserialize(ref BinaryReader reader)
        {
            CollisionMessage msg = MessagePool<CollisionMessage>.Get();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
            msg.destination = (DestinationType)reader.ReadInt32();
            msg.deliveryMethod = (NetDeliveryMethod)reader.ReadInt32();
            msg.channel = reader.ReadInt32();
            msg.objectID1 = reader.ReadInt32();
            msg.objectID2 = reader.ReadInt32();
            return msg;
        }

        public override void Execute()
        {
            GameObject obj1 = GameManager.Find(objectID1);
            GameObject obj2 = GameManager.Find(objectID2);

            if (obj1 != null && obj2 != null)
            {
                obj1.Accept(obj2);
            }

            Recycle();
        }

        public override void Recycle()
        {
            MessagePool<CollisionMessage>.Recycle(this);
        }

        public override void PrintMe()
        {
            Debug.WriteLine($"[CollisionMessage] {objectID1} collided with {objectID2}");
        }
    }
}

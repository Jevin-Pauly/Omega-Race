using Box2DX.Collision;
using Box2DX.Dynamics;

namespace OmegaRace
{
    public class ContactManager : ContactListener
    {

        public void BeginContact(Contact contact)
        {
            GameObject gameObjectA = contact._fixtureA.UserData as GameObject;
            GameObject gameObjectB = contact._fixtureB.UserData as GameObject;

            //gameObjectA.Accept(gameObjectB);
            // Safety check
            if (gameObjectA == null || gameObjectB == null)
                return;


            MessageQueueManager msgQueueMgr = GameSceneCollection.ScenePlay.MsgQueueMgr;
            // Only server sends collision messages
            if (GameSceneCollection.ScenePlay.isServer)
            {
                // Execute logic locally (server is authoritative)
                gameObjectA.Accept(gameObjectB);

                // Send collision event to client
                CollisionMessage msg = CollisionMessage.Create(gameObjectA.getID(), gameObjectB.getID());
                msgQueueMgr.AddToOutputQueue(msg);
            }
        }

        public void EndContact(Contact contact)
        {

        }

        public void PreSolve(Contact contact, Manifold manifold)
        {

        }

        public void PostSolve(Contact contact, ContactImpulse impulse)
        {

        }

    }
}

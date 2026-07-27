using Box2DX.Common;

namespace OmegaRace
{
    public class PlayerPrediction
    {
        // Version 0
        private GameObject gameObject;

        // Server-side state for dead reckoning
        private Vec2 lastServerPos;
        private Vec2 serverVelocity;
        private float lastServerTime;
        // Fallback for local estimation (if needed)
        private Vec2 previousPos;
        private float previousTime;

        // Working predicted position
        private Vec2 predictedPos;

        public PlayerPrediction(GameObject obj)
        {
            this.gameObject = obj;

            Vec2 startPos = obj.GetPixelPosition();
            lastServerPos = new Vec2(startPos.X, startPos.Y);
            serverVelocity = new Vec2(0.0f, 0.0f);
            predictedPos = new Vec2(startPos.X, startPos.Y);

            lastServerTime = TimeManager.GetCurrentTime();
            previousPos = new Vec2(startPos.X, startPos.Y);
            previousTime = lastServerTime;
        }

        // Update using server-provided position, velocity, and timestamp.
        public void UpdateFromServer(Vec2 pos, Vec2 vel, float serverTime)
        {
            this.lastServerPos.Set(pos.X, pos.Y);
            this.serverVelocity.Set(vel.X, vel.Y);
            this.lastServerTime = serverTime;
        }

        // Update using just position, useful for local prediction if velocity is unknown.
        public void UpdateLocally(float newX, float newY)
        {
            float currentTime = TimeManager.GetCurrentTime();
            float dt = currentTime - previousTime;

            if (dt > 0.0001f)
            {
                float vx = (newX - previousPos.X) / dt;
                float vy = (newY - previousPos.Y) / dt;
                serverVelocity.Set(vx, vy);
            }

            lastServerPos.Set(newX, newY);
            lastServerTime = currentTime;

            previousPos.Set(newX, newY);
            previousTime = currentTime;
        }

        // Move the GameObject to predicted position based on time elapsed.
        public void MoveToPredictedPosition()
        {
            float dt = TimeManager.GetCurrentTime() - lastServerTime;
            predictedPos = lastServerPos + (serverVelocity * dt);

            gameObject.SetPosAndAngle(predictedPos.X, predictedPos.Y, gameObject.GetAngle_Deg());
        }

        // What it says lol
        public Vec2 GetPredictedPosition()
        {
            float dt = TimeManager.GetCurrentTime() - lastServerTime;
            return lastServerPos + (serverVelocity * dt);
        }
    }
}

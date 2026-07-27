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

        //// Version 1
        //public GameObject gameObject;
        //private float lastTimeUpdate;      // Last time server sent an update
        //private Vec2 lastPos;              // Last known server position
        //private Vec2 velEstimation;        // Estimated velocity
        //private Vec2 currPosition;         // Current predicted position
        //
        //public PlayerPrediction(GameObject serverObj)
        //{
        //    this.gameObject = serverObj;
        //    this.lastPos = serverObj.GetPixelPosition();
        //    this.currPosition = new Vec2(lastPos.X, lastPos.Y);
        //
        //    this.lastTimeUpdate = 0.0f;
        //    this.velEstimation = new Vec2(0, 0);
        //}
        //
        //public void Update(float currPosX, float currPosY)
        //{
        //    float currentTime = TimeManager.GetCurrentTime();
        //    currPosition.Set(currPosX, currPosY);
        //    float timeDelta = currentTime - lastTimeUpdate;
        //    Vec2 positionDiff = currPosition - lastPos;
        //
        //    if (timeDelta != 0.0f)
        //    {
        //        velEstimation.Set(positionDiff.X / timeDelta, positionDiff.Y / timeDelta);
        //    }
        //
        //    lastPos.Set(currPosX, currPosY);
        //    lastTimeUpdate = currentTime;
        //}
        //
        //public void Update(Vec2 currPos, Vec2 currVel, float serverTime)
        //{
        //    lastPos.Set(currPos.X, currPos.Y);
        //    velEstimation.Set(currVel.X, currVel.Y);
        //    lastTimeUpdate = serverTime; // Server's time!
        //}
        //
        //// Move to predicted position
        //public void MoveToPredictedPosition()
        //{
        //    // Predict current position based on velocity and time elapsed since last update
        //    float timeDelta = TimeManager.GetCurrentTime() - lastTimeUpdate;
        //    currPosition = lastPos + (velEstimation * timeDelta);
        //
        //    gameObject.SetPosAndAngle(currPosition.X, currPosition.Y, gameObject.GetAngle_Deg());
        //}

        //// Version 2
        //GameObject gameObject;
        //Vec2 predictedPos;
        //Vec2 currentPos;
        //
        //public PlayerPrediction(GameObject obj)
        //{
        //    this.gameObject = obj;
        //    this.currentPos = obj.GetPixelPosition();
        //    this.predictedPos = new Vec2(currentPos.X, currentPos.Y);
        //}
        //
        //public void Update(float x, float y)
        //{
        //    this.predictedPos.X = x;
        //    this.predictedPos.Y = y;
        //}
        //
        //public void MoveToPredictedPosition()
        //{
        //    Vec2 actualPos = gameObject.GetPixelPosition();
        //    float lerpFactor = 0.1f; // Adjust for smoothing
        //    float newX = actualPos.X + (predictedPos.X - actualPos.X) * lerpFactor;
        //    float newY = actualPos.Y + (predictedPos.Y - actualPos.Y) * lerpFactor;
        //
        //    gameObject.SetPosAndAngle(newX, newY, gameObject.GetAngle_Deg());
        //}
    }
}

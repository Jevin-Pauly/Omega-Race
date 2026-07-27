using Box2DX.Common;

namespace OmegaRace
{
    public class PlayerPrediction
    {
        // Version 1
        public GameObject gameObject;
        private float lastTimeUpdate;      // Last time server sent an update
        private Vec2 lastPos;              // Last known server position
        private Vec2 velEstimation;        // Estimated velocity
        private Vec2 currPosition;         // Current predicted position

        public PlayerPrediction(GameObject serverObj)
        {
            this.gameObject = serverObj;
            this.lastPos = serverObj.GetPixelPosition();
            this.currPosition = new Vec2(lastPos.X, lastPos.Y);

            this.lastTimeUpdate = 0.0f;
            this.velEstimation = new Vec2(0, 0);
        }

        // Called when new server position is received
        public void Update(float serverX, float serverY)
        {
            float currentTime = TimeManager.GetCurrentTime();
            float timeDelta = currentTime - lastTimeUpdate;

            currPosition.Set(serverX, serverY);
            Vec2 positionDelta = currPosition - lastPos;

            lastPos.Set(serverX, serverY);
            lastTimeUpdate = currentTime;

            if (timeDelta != 0.0f)
                velEstimation.Set(positionDelta.X / timeDelta, positionDelta.Y / timeDelta);
        }

        // Move to predicted position
        public void MoveToPredictedPosition()
        {
            // Predict current position based on velocity and time elapsed since last update
            float timeDelta = TimeManager.GetCurrentTime() - lastTimeUpdate;
            currPosition = lastPos + (velEstimation * timeDelta);

            gameObject.SetPosAndAngle(currPosition.X, currPosition.Y, gameObject.GetAngle_Deg());
        }

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

using System.Collections.Generic;
using System.Linq;
using UnityBallsCore.Model;

namespace UnityBallsCore.Common
{
    public static class Physics
    {
        public static Vector2f Move(
            Vector2f source,
            Vector2f direction,
            float speed,
            IEnumerable<Rect2f> obstacles)
        {
            const float offset = 0.5f;
            const float stickOffset = offset + 0.001f;

            direction = direction.Normalize();
            var destination = source + speed * direction;

            var obstacle = obstacles.FirstOrDefault(
                obstacle => obstacle.Contains(destination, offset));

            if (obstacle == null)
            {
                return destination;
            }

            // stick to obstacle
            var stick = new Vector2f(
                source.X.Clamp(obstacle.Left - stickOffset, obstacle.Right + stickOffset),
                source.Y.Clamp(obstacle.Bottom - stickOffset, obstacle.Top + stickOffset));

            // try to move horizontally
            destination = stick + speed * new Vector2f(direction.X, 0);
            if (!obstacle.Contains(destination, offset))
            {
                return destination;
            }

            // try to move vertically
            destination = stick + speed * new Vector2f(0, direction.Y);
            if (!obstacle.Contains(destination, offset))
            {
                return destination;
            }

            return stick;
        }
    }
}

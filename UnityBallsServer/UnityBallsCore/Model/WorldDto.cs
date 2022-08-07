using System.Collections.Generic;

namespace UnityBallsCore.Model
{
    public class WorldDto
    {
        public float Width { get; set; }

        public float Height { get; set; }

        public List<ObstacleDto> Obstacles { get; set; } = new List<ObstacleDto>();
    }
}

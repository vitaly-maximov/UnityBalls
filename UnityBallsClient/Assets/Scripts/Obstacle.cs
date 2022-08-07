using UnityBallsCore.Model;
using System;
using UnityEngine;

public class Obstacle : IDisposable
{
    public Obstacle(Rect2f bounds, GameObject gameObject)
    {
        Bounds = bounds;
        GameObject = gameObject;
    }

    public Rect2f Bounds { get; }

    public GameObject GameObject { get; private set; }

    public static Obstacle Create(ObstacleDto obstacleDto, GameObject prefab)
    {
        GameObject obstacle = UnityEngine.Object.Instantiate(prefab);

        var renderer = obstacle.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", Common.GetColor(obstacleDto.ColorArgb));

        obstacle.transform.localPosition =
            new Vector3(obstacleDto.Bounds.CenterX, 0, obstacleDto.Bounds.CenterY);

        obstacle.transform.localScale =
            new Vector3(obstacleDto.Bounds.Width, 2, obstacleDto.Bounds.Height);

        return new Obstacle(obstacleDto.Bounds, obstacle);
    }

    #region IDisposable

    public void Dispose()
    {
        if (GameObject != null)
        {
            UnityEngine.Object.Destroy(GameObject);
            GameObject = null;
        }
    }

    #endregion
}
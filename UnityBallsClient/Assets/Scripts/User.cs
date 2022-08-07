using UnityBallsCore.Model;
using System;
using UnityEngine;

public class User : IDisposable
{
    public User(int id, GameObject gameObject)
    {
        Id = id;
        GameObject = gameObject;
    }

    public int Id { get; }

    public GameObject GameObject { get; private set; }

    public float Speed { get; set; }
    
    public static User Create(UserDto userDto, GameObject prefab)
    {
        GameObject user = UnityEngine.Object.Instantiate(prefab);

        var userRenderer = user.GetComponent<Renderer>();
        userRenderer.material.SetColor("_Color", Common.GetColor(userDto.ColorArgb));

        user.transform.position =
            new Vector3(userDto.Position.X, 0, userDto.Position.Y);

        return new User(userDto.Id, user)
        {
            Speed = userDto.Speed
        };
    }

    public void Update(UserDto userDto)
    {
        GameObject.transform.position =
            new Vector3(userDto.Position.X, 0, userDto.Position.Y);
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

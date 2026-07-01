using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    public static string Path(this GameObject gameObject) {
        return "/" + string.Join("/",
            gameObject.GetComponentsInParent<Transform>().Select(t => t.name).Reverse().ToArray());
    }

    public static string PathFull(this GameObject gameObject)
    {
        return gameObject.Path() + "/" + gameObject.name;
    }
}

using System.Text;
using UnityEngine;

namespace BattleBase.Utils
{
    public static class Utils
    {
        public static string GetPath(Transform transform)
        {
            if (transform.parent == false)
                return $"/{transform.name}";

            StringBuilder sb = new();

            do
            {
                sb.Insert(0, $"/{transform.name}");
                transform = transform.parent;
            } 
            while (transform);

            return sb.ToString();
        }
    }
}
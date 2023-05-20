using System;
using UnityEngine;

namespace Scripts.Features.Attributes
{
#if UNITY_EDITOR
    public class GameObjectOfTypeAttribute : PropertyAttribute
    {
        public Type Type { get; }
        public bool AllowSceneObj;

        public GameObjectOfTypeAttribute(Type type, bool allowSceneObj = true)
        {
            Type = type;
            AllowSceneObj = allowSceneObj;
        }
    }
#endif
}
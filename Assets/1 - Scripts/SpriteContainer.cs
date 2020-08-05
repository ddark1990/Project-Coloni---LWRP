using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "SpriteContainer", menuName = "ProjectColoni/SpriteContainer", order = 5)]
    public class SpriteContainer : ScriptableObject
    {
        [Serializable]
        public struct SpriteData
        {
            public string spriteName;
            public Sprite sprite;
        }

        [SerializeField] private SpriteData[] sprites;
        public Dictionary<string, Sprite> spriteCollection = new Dictionary<string, Sprite>();
        
        private void OnEnable()
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                var sprite = sprites[i].sprite;
                var spriteName = sprites[i].spriteName;
                
                spriteCollection.Add(spriteName, sprite);
            }
        }
    }
}

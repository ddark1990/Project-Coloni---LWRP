using System;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(ItemData)", menuName = "ProjectColoni/Objects/Create Item/New Consumable", order = 4)]
    public class Consumable : ItemType
    {
        [Serializable]
        public struct ConsumableSettings
        {
            public enum ConsumableType
            {
                Food, Medical, Boost
            }
            
            public ConsumableType consumableType;
            public float health;
            public float food;
        }

        [Tooltip("How much & what you will gain from consuming this item.")] public ConsumableSettings consumableSettings;

        //smart actions could maybe be created here as well
        /*
        public override void InitializeSmartAction(SmartObject smartObject)
        {
            smartObject.AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Inspect"], Inspect);
            smartObject.AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Eat"], Eat);
        }
        
        private void Inspect(AiController aiController, SmartObject smartObject)
        {
            Debug.Log("Inspecting " + aiController);
        }
        
        private void Eat(AiController aiController, SmartObject smartObject)
        {
            Debug.Log("Eating " + aiController);
        }
    */
    }
}
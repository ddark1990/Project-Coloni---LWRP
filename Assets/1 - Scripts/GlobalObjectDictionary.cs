using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectColoni
{
    public class GlobalObjectDictionary : MonoBehaviour
    {
        public static GlobalObjectDictionary Instance { get; private set; }
        
        public Dictionary<string, object> dictionary = new Dictionary<string, object>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddToGlobalDictionary(string id, Object obj)
        {
            if (dictionary.ContainsKey(id)) return;
            
            dictionary.Add(id, obj);
            //Debug.Log("Added: " + dictionary[id] + " | Hash Id: " + id, obj);
        }
    }
}

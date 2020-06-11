using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectColoni
{
    public class _preload : MonoBehaviour
    {
        [SerializeField] private GameObject[] managers;

        private int _managersLoaded;
        
        private void Awake()
        {
            foreach (var manager in managers)
            {
                Instantiate(manager);
                _managersLoaded++;

                Debug.Log(_managersLoaded);
                
                if (_managersLoaded == managers.Length)
                {
                    SceneManager.LoadSceneAsync(1);
                }
            }
        }
    }
}

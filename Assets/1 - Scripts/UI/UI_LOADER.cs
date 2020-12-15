using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectColoni
{
    public class UI_LOADER : MonoBehaviour
    {
        private void Start()
        {
            if (SceneManager.GetSceneByName("UI").isLoaded == false)
            {
                SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
                Destroy(gameObject);
            }
                
        }
    }
}

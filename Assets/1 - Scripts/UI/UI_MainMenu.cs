using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectColoni
{
    public class UI_MainMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Screen.SetResolution(1920, 1080, true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnNewColoniPress()
        {
            UIView.ShowView("MainMenuUI", "NewColoniUI");
        }

        public void StartNewRandomlyGeneratedColoni()
        {
            UIView.HideView("MainMenuUI", "NewColoniUI");

            SceneManager.LoadSceneAsync("GameScene");
        }
    }
}

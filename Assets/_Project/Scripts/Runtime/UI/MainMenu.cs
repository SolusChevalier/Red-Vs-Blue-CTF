using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CTF._project.Scripts.Runtime.UI
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject mainMenuCanvas;

        private void Start()
        {
            mainMenuCanvas.SetActive(true);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("Arena");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    bool soundEnabled = true;

    public Text soundText;

	public void Play() {
        SceneManager.LoadScene(1);
    }

    public void Menu() {
        SceneManager.LoadScene(0);
    }

    public void ToggleSound() {
        if (soundEnabled) {
            soundText.text = "Enable Sound";
            soundEnabled = false;
            SoundManager.disabled = true;
        } else {
            soundText.text = "Disable Sound";
            soundEnabled = true;
            SoundManager.disabled = false;
        }
    }

    public void Exit() {
        Application.Quit();
    }
}

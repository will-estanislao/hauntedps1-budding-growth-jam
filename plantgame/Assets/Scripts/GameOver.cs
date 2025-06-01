using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    private UIDocument doc;

    private Button restart;
    private Button quit;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();

        restart = doc.rootVisualElement.Q<Button>(name: "restart");
        quit = doc.rootVisualElement.Q<Button>(name: "quit");

        restart.RegisterCallback<ClickEvent>(RestartGame);
        quit.RegisterCallback<ClickEvent>(QuitGame);
    }

    private void RestartGame(ClickEvent evnt)
    {
        SceneManager.LoadScene(1);
    }

    private void QuitGame(ClickEvent evnt)
    {
        Application.Quit();
    }
}

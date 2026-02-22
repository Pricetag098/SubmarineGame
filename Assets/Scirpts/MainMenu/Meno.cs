using UnityEngine;
using UnityEngine.SceneManagement;

public class Meno : MonoBehaviour
{
    [SerializeField] float loadTime;
    [SerializeField] float fadeTime;
    [SerializeField] CanvasGroup warning;

    float timer;
    bool inTransition;


    public void StartGame()
    {
        if(inTransition)
            return;

        inTransition = true;
    }

    public void Quit()
    {
        if (inTransition)
            return;

        Application.Quit();
    }

    private void Update()
    {
        if (!inTransition)
            return;

        timer += Time.deltaTime;
        warning.alpha = timer / fadeTime;

        if (timer > loadTime)
            SceneManager.LoadScene("ManagmentScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.HDROutputUtils;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private string Submarine, Level;
    [SerializeField] private Submarine submarinePrefab;
    List<AsyncOperation> sceneLoads = new();

    public static GameLoader Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        sceneLoads.Add(SceneManager.LoadSceneAsync(Submarine, LoadSceneMode.Additive));
        sceneLoads.Add(SceneManager.LoadSceneAsync(Level, LoadSceneMode.Additive));
        foreach (var operation in sceneLoads)
        {
            operation.allowSceneActivation = true;
        }
        while (true)
        {
            var loading = false;
            foreach (var operation in sceneLoads)
            {
                if (!operation.isDone)
                    loading = true;
            }
            if (!loading)
                break;
            yield return null;
            
        }
        var scene = SceneManager.GetSceneByName(Submarine);
        SceneManager.SetActiveScene(scene);

        var map = SceneManager.GetSceneByName(Level);

        var point = FindAnyObjectByType<Spawnpoint>();
        var sub = Instantiate(submarinePrefab, map) as Submarine;
        sub.transform.position = point.transform.position;
        sub.transform.rotation = point.transform.rotation;

        var console = FindAnyObjectByType<PlayerConsole>();
        console.submarine = sub;
        sub.console = console;
    }
}

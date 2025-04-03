using UnityEngine;

public class AppLoader : SystemLoader
{
    private void Awake()
    {
        // Clear any statics that may have been held between Playmode sessions.
        ClearStatics();

        // Load the application config.
        LoadConfig();

        // Register tasks to be run.
        RegisterTasks();

        // Run tasks that have been registered.
        RunTasks();
    }

    private void ClearStatics()
    {
        ServiceLocator.Clear();
    }

    private void LoadConfig()
    {
        Debug.Log("Loading Config");
    }

    private void RegisterTasks()
    {
        Debug.Log("Register Tasks");
    }
}

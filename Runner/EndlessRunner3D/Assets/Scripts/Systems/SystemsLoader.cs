using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This class is responsible for loading systems
/// </summary>
public class SystemLoader : MonoBehaviour
{
    private static event Action Complete = delegate { };
    private static bool _loadingComplete = false;
    private static readonly Queue<Func<Task>> _taskQueue = new();

    /// <summary>
    /// Add a task to the queue. The task should be a function that returns a Task.
    /// </summary>
    /// <param name="loadingTask"></param>
    public void AddTask(Func<Task> loadingTask)
    {
        _taskQueue.Enqueue(loadingTask);
    }

    /// <summary>
    /// Run all tasks in the queue sequentially.
    /// </summary>
    public async void RunTasks()
    {
        if (_taskQueue.Count != 0)
        {
            // Create a list of tasks to run
            List<Task> tasks = new();
            tasks.AddRange(_taskQueue.Select(task => task()));

            // Run all tasks in parallel
            Task allTasks = Task.WhenAll(tasks);
            await allTasks;

            // Check if all tasks are completed successfully
            if (allTasks.IsCompletedSuccessfully)
            {
                Debug.Log("All tasks completed successfully.");
            }
            else
            {
                Debug.LogError("One or more tasks failed.");
            }
        }

        // All tasks are complete, invoke the complete event
        _loadingComplete = true;
        OnComplete();
    }

    /// <summary>
    /// Call this method to register a callback that will be invoked when all tasks are complete.
    /// </summary>
    /// <param name="callback"></param>
    public static void CallOnComplete(Action callback)
    {
        if (_loadingComplete == false)
        {
            Complete += callback;
        }
        else
        {
            callback.Invoke();
        }
    }

    private static void OnComplete()
    {
        Debug.Log("SystemLoader -> OnComplete");
        Complete.Invoke();
    }
}

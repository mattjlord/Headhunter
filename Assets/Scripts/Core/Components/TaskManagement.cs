using System.Collections.Generic;
using UnityEngine;

public class TaskManagement : MonoBehaviour
{
    private List<BehaviorTask> _tasks;
    private BehaviorTask _currentTask;
    [SerializeField] private string _currentTaskName;

    public BehaviorTask CurrentTask
    {
        get { return _currentTask; }
        set { _currentTask = value; }
    }

    private void Update()
    {
        List<BehaviorTask> toRemove = new List<BehaviorTask>();
        foreach (var task in _tasks)
        {
            if (!task.IsFrozen)
                task.UpdatePriority();

            if (task.Priority == 0 && !task.IsEssential)
                toRemove.Add(task);
        }

        foreach (var task in toRemove)
        {
            _tasks.Remove(task);
        }

        BehaviorTask nextTask = GetHighestPriorityTask();
        if (_currentTask != null && nextTask != _currentTask)
        {
            _currentTask.Exit();
        }

        _currentTask = nextTask;
        if (_currentTask != null)
        {
            _currentTask.Run();
            _currentTaskName = _currentTask.GetName() + " - Priority " + _currentTask.Priority;
            if (_currentTask.Description != null)
                _currentTaskName += " - " + _currentTask.Description;
        }
    }

    private BehaviorTask GetHighestPriorityTask()
    {
        BehaviorTask highestTask = null;
        float highestPriority = -1;

        foreach (var task in _tasks)
        {
            if (task.Priority > highestPriority)
            {
                highestPriority = task.Priority;
                highestTask = task;
            }
        }

        return highestTask;
    }

    public void ClearTasks()
    {
        _tasks = new List<BehaviorTask>();
    }

    public void AddTask(BehaviorTask task)
    {
        _tasks.Add(task);
    }

    public void RemoveAssociatedTasks(ALocation location)
    {
        List<BehaviorTask> toRemove = new List<BehaviorTask>();
        foreach (BehaviorTask task in _tasks)
        {
            if (task.HasAssociatedLocation(location))
            {
                toRemove.Add(task);
            }
        }
        foreach (BehaviorTask task in toRemove)
        {
            _tasks.Remove(task);
        }
    }
}

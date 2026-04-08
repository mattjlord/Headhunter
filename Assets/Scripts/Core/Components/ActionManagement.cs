using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private OrganismAction _currentAction;
    [SerializeField] private OrganismAction _nextAction;
    public bool IsReadyForQueue()
    {
        return _nextAction == null || !_nextAction.Constructed;
    }

    public void QueueAction(OrganismAction action)
    {
        _nextAction = action;
    }

    public void Stop()
    {
        _currentAction = null;
        _nextAction = null;
        _animator.SetBool("Is Busy", false);
    }

    private void Update()
    {
        bool startNextAction = true;

        if (_currentAction != null && _currentAction.Constructed && !_currentAction.IsFinished)
            startNextAction = false;

        if (startNextAction)
        {
            _currentAction = null;
            _animator.SetBool("Is Busy", false);
            if (_nextAction != null && _nextAction.Constructed)
            {
                _currentAction = _nextAction;
                _nextAction = null;
                _currentAction.Start(_animator);
            }
        }

        if (_currentAction != null && _currentAction.Constructed)
            _currentAction.Update(Time.deltaTime);

        // TODO: Remove this later, it's sloppy
        AIOrganism organism = GetComponent<AIOrganism>();
        if (organism != null)
        {
            if (_currentAction != null && _currentAction.Constructed)
                organism.ActionMsg = _currentAction.AnimationName + " - " + (int)(_currentAction.Progress * 100f) + "%";
            else
                organism.ActionMsg = "No Action";
        }
    }
}
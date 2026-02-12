using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagement : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool _isBusy = false;
    private bool _currentActionTriggered = false;

    private OrganismAction _currentAction;
    private OrganismAction _nextAction;

    private float _elapsedTime = 0;

    private void Update()
    {
        if (_currentAction != null)
        {
            _elapsedTime += Time.deltaTime;

            if (IsCurrentActionTriggerReady())
                FireCurrentActionTrigger();

            if (IsCurrentActionComplete())
                OnCurrentActionEnd();
        }

        if (ReadyForNextAction())
        {
            StartNextAction();
        }
    }

    private bool IsCurrentActionTriggerReady()
    {
        return _currentAction.TriggeredAction != null && _elapsedTime >= _currentAction.TriggerDelay && !_currentActionTriggered;
    }

    private bool IsCurrentActionComplete()
    {
        if (_currentAction.Duration > 0f)
            return _elapsedTime >= _currentAction.Duration;

        if (!string.IsNullOrEmpty(_currentAction.AnimationName))
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);

            return state.IsName(_currentAction.AnimationName)
                   && !state.loop
                   && state.normalizedTime >= 1f;
        }

        return true;
    }

    private void FireCurrentActionTrigger()
    {
        _currentAction.TriggeredAction.Invoke();
        _currentActionTriggered = true;
    }

    private void OnCurrentActionEnd()
    {
        _isBusy = false;
        _currentAction = null;
        _elapsedTime = 0;
    }

    private bool ReadyForNextAction()
    {
        return !_isBusy && _nextAction != null;
    }

    private void StartNextAction()
    {
        _currentAction = _nextAction;
        _nextAction = null;
        _isBusy = true;

        if (_currentAction.AnimationName != null)
        {
            _animator.Play(_currentAction.AnimationName);
        }
        _currentActionTriggered = false;
    }

    public bool IsReadyForQueue()
    {
        return _nextAction == null;
    }

    public void QueueAction(OrganismAction action)
    {
        _nextAction = action;
    }
}

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

    private Vector2 _startPosition;
    private Vector2 _endPosition;

    private void Update()
    {
        if (_currentAction != null)
        {
            _elapsedTime += Time.deltaTime;

            UpdateCurrentActionDisplacement();

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

    private void UpdateCurrentActionDisplacement()
    {
        if (_currentAction.Displacement == null || _currentAction.Displacement == Vector2.zero)
            return;

        float positionLerp = _elapsedTime / _currentAction.Duration;

        Vector2 currentPosition = Vector2.Lerp(_startPosition, _endPosition, positionLerp);

        _currentAction.Organism.Position = currentPosition;
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
        if (_currentAction.Displacement != null)
        {
            _startPosition = _currentAction.Organism.Position;
            _endPosition = _startPosition + _currentAction.Displacement;
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

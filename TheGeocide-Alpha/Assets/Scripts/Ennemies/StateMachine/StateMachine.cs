using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
   private string _oldPrintState = "";
   private IState _currentState;
   
   private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
   private List<Transition> _currentTransitions = new List<Transition>();
   private List<Transition> _anyTransitions = new List<Transition>();
   
   private static List<Transition> EmptyTransitions = new List<Transition>(0);
    private EnemyStateChannel _stateChannel;
    private string _enemyId;
    public StateMachine()
    {

    }

    public StateMachine(EnemyStateChannel stateChannel, string enemyId)
    {
        _enemyId = enemyId;
        _stateChannel = stateChannel;
    }

   public void Tick(bool displayLog = false)
   {
      var transition = GetTransition();
      if (transition != null)
        {
            if (displayLog)
            {
                Debug.Log($"{DateTime.UtcNow} transition : " + transition.DisplayName);
            }
            SetState(transition.To);
        }
         

      if(_currentState != null && _oldPrintState != _currentState.GetType().ToString())
      {
            _oldPrintState = _currentState.GetType().ToString();
            if (displayLog)
            {
                Debug.Log($"{DateTime.UtcNow} _currentState : " + _currentState.GetType().ToString());
            }
        }
        //Debug.Log("_currentState : " + _currentState.GetType().ToString());

        _currentState?.Tick();
   }

   public void SetState(IState state)
   {
      if (state == _currentState)
         return;
      
      if(_currentState != null){
            _currentState.OnExit();
            _stateChannel.OnStateExit(new EnemyStateEvent(_currentState.GetStateType(), _enemyId));
      }
     
      _currentState = state;
      
      _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
      if (_currentTransitions == null)
         _currentTransitions = EmptyTransitions;
      
      _currentState.OnEnter();
      _stateChannel.OnStateEnter(new EnemyStateEvent(_currentState.GetStateType(), _enemyId));
    }

   public void AddTransition(IState from, IState to, Func<bool> predicate)
   {
      if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
      {
         transitions = new List<Transition>();
         _transitions[from.GetType()] = transitions;
      }
      
      transitions.Add(new Transition(to, predicate));
   }

   public void AddAnyTransition(IState state, Func<bool> predicate)
   {
      _anyTransitions.Add(new Transition(state, predicate));
   }

   private class Transition
   {
      public string DisplayName;

      public Func<bool> Condition {get; }
      public IState To { get; }

      public Transition(IState to, Func<bool> condition)
      {
         To = to;
         Condition = condition;
         DisplayName = $"To {to.GetStateType()} if {condition.Method.Name}";
      }
   }

   private Transition GetTransition()
   {
      foreach(var transition in _anyTransitions)
         if (transition.Condition())
            return transition;
      
      foreach (var transition in _currentTransitions)
         if (transition.Condition())
            return transition;

      return null;
   }

    internal IState GetCurrentSate()
    {
        return _currentState;
    }
}
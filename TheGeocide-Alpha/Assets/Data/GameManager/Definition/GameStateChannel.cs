using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/GameStateChannel")]
public class GameStateChannel : ScriptableObject
{
    //Player US
    public delegate void RequestCallback();
    public delegate void UnfortunateSoulAmountCallback(int newAmount);
    public UnfortunateSoulAmountCallback OnUnfortunateSoulAmoutUpdate;
    public UnfortunateSoulAmountCallback OnUnfortunateSoulAddition;
    public RequestCallback OnUnfortunateSoulAmoutRequested;

    //Player Health
    public delegate void HealthCallback(HealthEvent hpEvt);
    public delegate void HealPointResquestCallback();
    public delegate void HealPointAnswerCallback(int maxHp, int currenthp);

    public HealthCallback OnHealthChanged;
    public HealPointResquestCallback OnHealthPointRequested;
    public HealPointAnswerCallback OnHealthPointAnswered;


    //US
    public void RaiseUnfortunateSoulAmoutUpdate(int newAmount)
    {
        OnUnfortunateSoulAmoutUpdate?.Invoke(newAmount);
    }

    public void RaiseUnfortunateSoulAddition(int addAmount)
    {
        OnUnfortunateSoulAddition?.Invoke(addAmount);
    }
    
    public void RaiseRequestUnfortunateSoulAmout()
    {
        OnUnfortunateSoulAmoutRequested?.Invoke();
    }

    //Hp
    public void RaiseHealthChanged(HealthEvent hpEvt)
    {
        OnHealthChanged?.Invoke(hpEvt);
    }
    public void RaisedHealthPointRequest()
    {
        OnHealthPointRequested?.Invoke();
    }
    public void RaisedHealthPointAnswer(int maxHp, int currentHp)
    {
        OnHealthPointAnswered?.Invoke(maxHp, currentHp);
    }
}
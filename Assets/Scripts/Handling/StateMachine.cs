namespace StateMachineNamespace
{
    public class StateMachine<T>
    {
        public State<T> currentState { get; private set; }
        public T stateMachineOwner;

        public StateMachine(T owner)
        {
            stateMachineOwner = owner;
            currentState = null;
        }

        public void ChangeState(State<T> newstate)
        {
            if (currentState != null)
                currentState.ExitState(stateMachineOwner);
            currentState = newstate;
            currentState.EnterState(stateMachineOwner);
        }

        public void Update()
        {
            if (currentState != null)
                currentState.UpdateState(stateMachineOwner);
        }
 
    }

    public abstract class State<T>
    {

        public virtual void EnterState(T stateOwner)
        {

        }

        public virtual void ExitState(T stateOwner)
        {

        }

        public  virtual  void UpdateState(T stateOwner)
        {

        }
    }
}



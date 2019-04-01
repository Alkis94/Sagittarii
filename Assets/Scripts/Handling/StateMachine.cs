namespace StateMachineNamespace
{
    public class StateMachine<T>
    {
        public State<T> currentState { get; private set; }
        public State<T> previousState { get; private set; }
        public T stateMachineOwner;

        public StateMachine(T owner)
        {
            stateMachineOwner = owner;
            currentState = null;
            previousState = null;
        }

        public void ChangeState(State<T> newstate)
        {
            if (currentState != null)
            {
                currentState.ExitState(stateMachineOwner);
                previousState = currentState;
            }

            currentState = newstate;

            if (newstate != null)
            {
                currentState.EnterState(stateMachineOwner);
            }
        }

        public void Update()
        {
            if (currentState != null)
                currentState.UpdateState(stateMachineOwner);
        }

        public void FixedUpdate()
        {
            if (currentState != null)
                currentState.FixedUpdateState(stateMachineOwner);
        }

    }

    public abstract class State<T>
    {
        T stateOwner;

        public virtual void EnterState(T stateOwner)
        {

        }

        public virtual void ExitState(T stateOwner)
        {

        }

        public  virtual  void UpdateState(T stateOwner)
        {

        }

        public virtual void FixedUpdateState(T stateOwner)
        {

        }
    }
}



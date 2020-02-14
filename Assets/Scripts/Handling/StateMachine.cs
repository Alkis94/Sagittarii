namespace StateMachineNamespace
{
    public class StateMachine<T>
    {
        public State<T> currentState { get; private set; }
        public State<T> previousState { get; private set; }

        public StateMachine(T owner)
        {
            currentState = null;
            previousState = null;
        }

        public void ChangeState(State<T> newstate)
        {
            if (currentState != null)
            {
                currentState.ExitState();
                previousState = currentState;
            }

            currentState = newstate;

            if (newstate != null)
            {
                currentState.EnterState();
            }
        }

        public void Update()
        {
            if (currentState != null)
                currentState.UpdateState();
        }

        public void FixedUpdate()
        {
            if (currentState != null)
                currentState.FixedUpdateState();
        }

    }

    public abstract class State<T>
    {
        protected T stateOwner;

       
        public virtual void EnterState()
        {

        }

        public virtual void ExitState()
        {

        }

        public  virtual  void UpdateState()
        {

        }

        public virtual void FixedUpdateState()
        {

        }
    }
}



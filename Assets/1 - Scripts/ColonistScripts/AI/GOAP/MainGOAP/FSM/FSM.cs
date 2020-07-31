using System.Collections.Generic;
/**
 * Stack-based Finite State Machine.
 * Push and pop states to the FSM.
 * 
 * States should push other states onto the stack 
 * and pop themselves off.
 */


namespace ProjectColoni
{
	public class FSM {

		private Stack<FSMState> stateStack = new Stack<FSMState> ();

		public delegate void FSMState (FSM fsm, AiController aiAgent);
	

		public void Update (AiController aiController) {
			if (stateStack.Peek() != null)
				stateStack.Peek().Invoke (this, aiController);
		}

		public void pushState(FSMState state) {
			stateStack.Push (state);
		}

		public void popState() {
			stateStack.Pop ();
		}
	}
}

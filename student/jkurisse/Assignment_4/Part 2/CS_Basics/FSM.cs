using System;
using System.Diagnostics;

namespace CS_Basics
{
    public enum FSM_STATE
    {
        A,
        B,
        C,
        D,
        E,
        UNDEF
    }
    public class FSM
    {
        private FSM_STATE state;

        public FSM()
        {
            state = FSM_STATE.A;
        }

        public FSM_STATE GetState() { return state; }

        public void set(FSM_STATE _state)
        {
            state = _state;
            Debug.WriteLine("FSM(): set(STATE_{0}) : STATE_{0}", state);
        }
        public void advance(Byte b)
        {
            switch (state)
            {
                case FSM_STATE.A: state = (b == 1) ? FSM_STATE.B : FSM_STATE.A; break;
                case FSM_STATE.B: state = (b == 1) ? FSM_STATE.C : FSM_STATE.E; break;
                case FSM_STATE.C: state = (b == 1) ? FSM_STATE.E : FSM_STATE.D; break;
                case FSM_STATE.D: state = (b == 1) ? FSM_STATE.B : FSM_STATE.D; break;
                case FSM_STATE.E: state = (b == 1) ? FSM_STATE.A : FSM_STATE.C; break;
                default: throw new ArgumentOutOfRangeException();
            }
            Debug.WriteLine("FSM():   advance({0}) : STATE_{1}", b, state);
        }



        public void doWork(FSM_Data data)
        {
            FSM myFSM = Program.GetFSM();
            if (data.fsmOp == FSM_STATE_OPERATION.ADVANCE)
                myFSM.advance(data.advData);
            else if (data.fsmOp == FSM_STATE_OPERATION.SET)
                myFSM.set(data.stateData);
        }
    }
}

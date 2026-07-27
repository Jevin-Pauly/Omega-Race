using System;

namespace CS_Basics
{
    public enum FSM_STATE_OPERATION
    {
        ADVANCE,
        SET,

        UNDEF
    }
    public class FSM_Data : QueueItem
    {
        public FSM_Data()
        {
            advData = 0;
            stateData = Program.GetFSM().GetState();
            fsmOp = FSM_STATE_OPERATION.UNDEF;
        }

        public void SetAdvData(Byte advData)
        {
            this.advData = advData;
            this.fsmOp = FSM_STATE_OPERATION.ADVANCE;
        }

        public void SetStateData(FSM_STATE stateData)
        {
            this.stateData = stateData;
            this.fsmOp = FSM_STATE_OPERATION.SET;
        }

        public override void doWork(Calc calc, FSM fsm)
        {
            fsm.doWork(this);
        }

        //public FSM_STATE_OPERATION fsmOp { get; private set; }
        public FSM_STATE_OPERATION fsmOp;
        public Byte advData;
        public FSM_STATE stateData;
    }
}

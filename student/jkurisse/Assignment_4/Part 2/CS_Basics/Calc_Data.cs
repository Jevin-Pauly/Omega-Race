namespace CS_Basics
{
    public enum Calc_Type
    {
        CALC_ADD,
        CALC_SUB,
        CALC_MULT,
        CALC_SET,
        CALC_UNDEF
    }
    public class Calc_Data : QueueItem
    {
        public Calc_Type operation { get; set; }
        public int value { get; set; }
        public Calc_Data() : this(Calc_Type.CALC_UNDEF, 0) { }
        public Calc_Data(Calc_Type op, int val)
        {
            operation = op;
            value = val;
        }
        public void set(Calc_Type op, int val)
        {
            operation = op;
            value = val;
        }

        public override void doWork(Calc calc, FSM fsm)
        {
            calc.doWork(this);
        }
    }
}

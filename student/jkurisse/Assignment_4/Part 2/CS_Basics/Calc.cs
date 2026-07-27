using System;
using System.Diagnostics;

namespace CS_Basics
{
    public class Calc
    {
        private int result = 0;

        public void set(int val) { result = val; Debug.WriteLine("calc():    set({0,2}) : {1}", val, result); }
        public void add(int val) { result += val; Debug.WriteLine("calc():    add({0,2}) : {1}", val, result); }
        public void sub(int val) { result -= val; Debug.WriteLine("calc():    sub({0,2}) : {1}", val, result); }
        public void mult(int val) { result *= val; Debug.WriteLine("calc():   mult({0,2}) : {1}", val, result); }

        public void doWork(Calc_Data data)
        {
            Calc myCalc = Program.GetCalc();

            switch (data.operation)
            {
                case Calc_Type.CALC_ADD: myCalc.add(data.value); break;
                case Calc_Type.CALC_SUB: myCalc.sub(data.value); break;
                case Calc_Type.CALC_MULT: myCalc.mult(data.value); break;
                case Calc_Type.CALC_SET: myCalc.set(data.value); break;
                //case Calc_Type.CALC_ADD: this.result += data.value; break;
                //case Calc_Type.CALC_SUB: this.result -= data.value; break;
                //case Calc_Type.CALC_MULT: this.result *= data.value; break;
                //case Calc_Type.CALC_SET: this.result = data.value; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }


    }

}

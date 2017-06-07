﻿#region Reference

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using SimplePIM.General;
using SimplePIM.Configs;

#endregion

namespace SimplePIM.General
{
    /// <summary>
    /// Instruction Defination
    /// </summary>
    public class Instruction :InputType
    {
        #region Public Varibles

        /// <summary>
        /// Type
        /// </summary>
        public InstructionType type;

        /// <summary>
        /// Used Address.
        /// Vaild when is_mem bit sets true.
        /// </summary>
        public UInt64 address = 0;

        /// <summary>
        /// Data.
        /// Vaild when is_mem bit sets true.
        /// </summary>
        public UInt64 data = 0;

        /// <summary>
        /// Indicate the pid of input units.
        /// </summary>
        public int pid;


        //Operation and Operands
        public string Operation = "";
        public string Operand1 = "";
        public string Operand2 = "";
        public string Operand3 = "";

        /// <summary>
        /// PC
        /// </summary>
        public UInt64 pc = 0;   

        /// <summary>
        /// Memory Instruction Bit.
        /// True : This instruction contains memory operations.
        /// False : None memory operations.
        /// </summary>
        public bool is_mem = false;

        /// <summary>
        /// Block address
        /// </summary>
        public UInt64 block_addr = 0;

        /// <summary>
        /// True : This instruction is completly processed.
        /// False : This instruction is waitting for processing.
        /// </summary>
        public bool ready = false;

        /// <summary>
        /// PIM bit.
        /// True : This Instruction is executed at memory-side.
        /// False : host-side.
        /// </summary>
        public bool pim = false;

        /// <summary>
        /// Processed cycle.
        /// </summary>
        public UInt64 served_cycle = 0;

        #endregion

        #region Public Method
        /// <summary>
        /// Construction Function
        /// </summary>
        /// <param name="ins">instruction string</param>
        /// <param name="pc_">PC</param>
        public Instruction(string ins, UInt64 pc_ = 0)
        {
            Operation = ins.Substring(0, ins.IndexOf(" ") + 1).Trim();
            string inst = ins.Substring(ins.IndexOf(" ")).Trim();
            string[] split = inst.Split(',');
            for (int i = 1; i <= split.Length; i++)
            {
                FieldInfo fi = this.GetType().GetField("Operand" + i);
                fi.SetValue(this, split[i - 1]);
            }
            is_mem = false;

        }

        /// <summary>
        /// Construction Function
        /// </summary>
        /// <param name="op"></param>
        /// <param name="cycles"></param>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        /// <param name="op3"></param>
        /// <param name="pc_"></param>
        public Instruction(string op, UInt64 cycles, string op1, string op2 = "", string op3 = "", UInt64 pc_ = 0)
        {
            Operation = op;
            cycle = cycles;
            Operand1 = op1;
            Operand2 = op2;
            Operand3 = op3;
            is_mem = false;
        }

        /// <summary>
        /// Construction Function
        /// </summary>
        /// <param name="pc_"></param>
        public Instruction(UInt64 pc_ = 0)
        {
            type = InstructionType.NOP;
            is_mem = false;
        }

        /// <summary>
        /// Construction Function
        /// </summary>
        /// <param name="type_"></param>
        public Instruction(InstructionType type_)
        {
            type = type_;
            is_mem = false;
        }

        /// <summary>
        /// Count bytes of the instruction
        /// </summary>
        /// <returns></returns>
        public override ulong Length()
        {
            return  Config.operationcode_length;
        }



        /// <summary>
        /// Print whole instruction
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            if (type == InstructionType.NOP)
                return "NOP";
            StringBuilder sb = new StringBuilder();
            if (Operation != "")
                sb.Append(Operation + " ");
            if (Operand1 != "")
                sb.Append(Operand1 + " ");
            if (Operand2 != "")
                sb.Append(Operand2 + " ");
            if (Operand3 != "")
                sb.Append(Operand3 + " ");
            sb.Append(" [" + cycle+"]");
            sb.Append(" [" + type+"]");
            if (address != 0)
                sb.Append(" [0x" + address.ToString("X") + "]");
            if (data != 0)
                sb.Append(" [0x" + data.ToString("X") + "]");
            return sb.ToString();

        }
        #endregion
    }

    /// <summary>
    /// Intruction Type
    /// <para>READ : Memory Instruction.</para>
    /// <para>WRITE : Memory Instruction.</para>
    /// <para>EOF : No trace input.</para>
    /// <para>NOP : No operations.</para>
    /// <para>CALCULATION : ALU operations.</para>
    /// </summary>
    public enum InstructionType { READ, WRITE, EOF, NOP,CALCULATION };
}

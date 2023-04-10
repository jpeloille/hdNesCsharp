using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace hdNes.Nes
{
    public sealed partial class Ricoh2A03
    {
         //Board where the component is tied to:
        private Board _board;

        //Instructions set container:
        public delegate void Instruction();
        private readonly Instruction[] _instructionMethod = new Instruction[0xFF];

        private delegate void InstructionAddressMode();
        private readonly InstructionAddressMode[] _instructionAddressMode = new InstructionAddressMode[0xFF];

        private readonly InstructionAttribute[] _instructionAttributes = new InstructionAttribute[0xFF];

        //16 bits address structure:
        [StructLayout(LayoutKind.Explicit, Size = 16)]
        private struct WordAddress
        {
            [FieldOffset(0)] public ushort word;
            [FieldOffset(0)] public byte low;
            [FieldOffset(1)] public byte high;

            public WordAddress(ushort address) : this()
            {
                word = address;
            }
        }

        //Cpu registers:
        public byte A;    //Accumulator
        public byte X;    //Index register X
        public byte Y;    //Index register Y
        public byte S;    //Stack pointer
        public ushort PC; //Program counter

        //Cpu flags:
        public bool C;     //Carry flag,
        public bool Z;     //Zero flag,
        public bool I;     //IR0 inhibit flag, 
        public bool D;     //Decimal mode flag,
        public bool B;     //Brake command flag,
        public bool V;     //Overflow flag,
        public bool N;     //Negative flag.

        public byte P
        {
            get
            {
                return (byte)(
                    (N ? 0x80 : 0) |
                    (V ? 0x40 : 0) |
                    (B ? 0x10 : 0) |
                    (D ? 0x08 : 0) |
                    (I ? 0x04 : 0) |
                    (Z ? 0x02 : 0) |
                    (C ? 0x01 : 0) | 0x20);
            }
            set
            {
                N = (value & 0x80) != 0;
                V = (value & 0x40) != 0;
                B = (value & 0x10) != 0;
                D = (value & 0x08) != 0;
                I = (value & 0x04) != 0;
                Z = (value & 0x02) != 0;
                C = (value & 0x01) != 0;
            }
        }

        //Cpu emulations state variables:
        private byte _opcode;
        private WordAddress _physicalAddress;
        private WordAddress _relativeAddress;

        private byte M;
        private ushort result16;
        private byte result8;
        private byte operand;

        private int cycles = 0;

        public Ricoh2A03(Board board)
        {
            _board = board;

            InitializeInstructionDelegateTable();
            InitializeInstructionAddressModeDelegateTable();
        }

        private void InitializeInstructionDelegateTable()
        {
            var instructionList =
                from instruction in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                let instructionsAttributes = instruction.GetCustomAttributes(typeof(InstructionAttribute), false)
                where instructionsAttributes.Length > 0
                select new
                {
                    delegateLink = (Instruction)Delegate.CreateDelegate(typeof(Instruction), this, instruction.Name),
                    name = instruction.Name,
                    attributes = (from instructionAttributes in instructionsAttributes select (InstructionAttribute)instructionAttributes)
                };

            foreach (var instruction in instructionList)
            {
                foreach (var instructionAttribute in instruction.attributes)
                {
                    _instructionMethod[instructionAttribute.OpCode] = instruction.delegateLink;
                    _instructionAttributes[instructionAttribute.OpCode] = instructionAttribute;
                }
            }
        }

        private void InitializeInstructionAddressModeDelegateTable()
        {
            var addressingModeList =
                from addressingMode in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                let attributes = addressingMode.GetCustomAttributes(typeof(AddressingModeAttribute), false)
                where attributes.Length > 0
                select new
                {
                    delegateLink = (InstructionAddressMode)Delegate.CreateDelegate(typeof(InstructionAddressMode), this, addressingMode.Name),
                    name = addressingMode.Name,
                    attributes = (from addressingModeAttribute in attributes select (AddressingModeAttribute)addressingModeAttribute)
                };

            foreach (var addressingMode in addressingModeList)
            {
                foreach (var addressingModeAttribute in addressingMode.attributes)
                {
                     foreach (var instruction in _instructionAttributes)
                     {
                         if ((instruction != null) && (instruction.AddressingMode == addressingModeAttribute.AddressingMode))
                             _instructionAddressMode[instruction.OpCode] = addressingMode.delegateLink;
                     }                   
                }
            }
        }

        #region Cpu emalution methods

        public void SetInResetState()
        {
            PC = 0xC000;
            S = 0xFD;
            P = 0x24;
            A = 0x00;
            
            Write(0x4017, 0x00); //Frame irq enabled.
            Write(0x4015, 0x00); //All channels disabled.
            
            _physicalAddress.word = 0x0000;
            _relativeAddress.word = 0x0000;
        }

        public void SetInUnitTestInitialState()
        {
            PC = 0xC000;
            P = 0x24;
            S = 0xFF;
            A = 0x00;
            X = 0x00;
            Y = 0x00;
            _physicalAddress.word = 0x0000;
            _relativeAddress.word = 0x0000;

            for (ushort i = 0; i < 0x2000; i++)
            {
                Write(i, 0x00);
            }
        }
        
        
        public void Tick(int count)
        {
            List<DebugLine> debugReport = new List<DebugLine>();
            DebugLine debugLine;
            
            while (count >= 1)
            {
                FetchOpcode();
                _instructionAddressMode[_opcode]();
                _instructionMethod[_opcode]();
                count -= _instructionAttributes[_opcode].NoCycles;
            }
        }

        public void FetchOpcode()
        {
            _opcode = Read(PC);
            PC++;
        }

        //only public for unit test - prefer private for final version
        public byte Read(ushort address)
        {
            return _board.CpuRead(address);
        }

        //only public for unit test - prefer private for final version
        public void Write(ushort address, byte data)
        {
            _board.CpuWrite(address, data);
        }
        
        #endregion
    }
}

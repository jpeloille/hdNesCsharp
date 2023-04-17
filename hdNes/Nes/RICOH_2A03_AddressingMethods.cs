using System;

namespace hdNes.Nes
{
    sealed partial class Ricoh2A03
    {
        public enum AddressingMode
        {
            None,
            Implied,
            Accumulator,
            Immediate,
            ZeroPage,
            Absolute,
            ZeroPageX,
            ZeroPageY,
            AbsoluteX,
            AbsoluteY,
            Indirect,
            IndirectX,
            IndirectY,
            Relative
        }

        [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
        public class AddressingModeAttribute : Attribute
        {
            public AddressingMode AddressingMode = AddressingMode.None;
        }
        
        [AddressingModeAttribute(AddressingMode = AddressingMode.Accumulator)]
        [AddressingModeAttribute(AddressingMode = AddressingMode.Implied)]
        private void ImpliedOrAccumulator()
        {

        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.Immediate)]
        private void FetchAddress_Immediate()
        {
            _physicalAddress.word = PC;
            PC++;
        } //Confirmed 26-09-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.Absolute)]
        private void FetchAddress_Absolute()
        {
            _physicalAddress.low = ReadByte(PC);
            PC++;
            
            _physicalAddress.high = ReadByte(PC);
            PC++;
        } //Confirmed 26-09-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPage)]
        private void FetchAddress_ZeroPage()
        {
            _physicalAddress.low = ReadByte(PC);
            PC++;

            _physicalAddress.high = 0x00;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPageX)]
        private void FetchAddress_ZeroPageX()
        {
            byte low = ReadByte(PC); PC++;
            low += X;

            _physicalAddress.low = low;
            _physicalAddress.high = 0x00;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPageY)]
        private void FetchAddress_ZeroPageY()
        {
            byte low = ReadByte(PC); PC++;
            low += Y;

            _physicalAddress.low = low;
            _physicalAddress.high = 0x00;
        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.AbsoluteX)]
        private void FetchAddress_AbsoluteX()
        {
            _physicalAddress.low = ReadByte(PC);
            PC++;

            _physicalAddress.high = ReadByte(PC);
            PC++;

            _physicalAddress.word += X;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.AbsoluteY)]
        private void FetchAddress_AbsoluteY()
        {
            _physicalAddress.low = ReadByte(PC);
            PC++;

            _physicalAddress.high = ReadByte(PC);
            PC++;

            _physicalAddress.word += Y;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.IndirectX)]
        private void FetchAddress_IndirectX()
        {
            byte loa = ReadByte(PC);
            PC++;

            ushort lsb = (ushort)((loa + X) & 0x00FF);
            _physicalAddress.low = ReadByte(lsb);

            ushort msb = (ushort)((loa + X + 1) & 0x00FF);
            _physicalAddress.high = ReadByte(msb);
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.IndirectY)]
        private void FetchAddress_IndirectY()
        {
            byte loa = ReadByte(PC);
            PC++;

            ushort lsb = (ushort)((loa) & 0x00FF);
            _physicalAddress.low = ReadByte(lsb);

            ushort msb = (ushort)((loa + 1) & 0x00FF);
            _physicalAddress.high = ReadByte(msb);

            _physicalAddress.word += Y;
        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.Indirect)]
        private void FetchAddress_Indirect()
        {
            byte IAL = ReadByte(PC);
            PC++;
            
            byte IAH = ReadByte(PC);
            PC++;

            ushort ADL_Adress = (ushort)(((IAH << 8) & 0XFF00) | IAL);
            _physicalAddress.low = ReadByte(ADL_Adress);

            IAL = (byte)(IAL + 0X01);
            ushort ADH_Adress = (ushort)(((IAH << 8) & 0XFF00) | IAL);
            _physicalAddress.high = ReadByte(ADH_Adress);
        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.Relative)]
        private void FetchAddress_Relative()
        {
            // This address mode is exclusive to branch instructions. The address
            // The address must reside within -128 to +127 of the branch instruction.

            _relativeAddress.low = ReadByte(PC);
            PC++;

            if ((_relativeAddress.low & 0x80) == 1)
            {
                _relativeAddress.low = 0x00;
                _relativeAddress.high = 0xFF;
            }

            _physicalAddress.word = (ushort)(PC + _relativeAddress.low);
        }
    }
}

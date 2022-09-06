namespace hdNes.Nes
{
    sealed partial class Cpu_2A03
    {
        private void ADC()
        {
            //Get operand from memory:
            M = Read(_absoluteAddress.word);
            M &= 0xFF;
            
            //Instruction Computation:
            result16 = (ushort)(A + M);
            if (C) result16 += 1;
            result8 = (byte)result16;

            //Refresh flag status:
            P &= 0x3C;
            N = ((result8 >> 7) & 1) == 1;
            V = ((result8 ^ A) & (result8 ^ M) & 0x80) == 0x80;
            Z = result8 == 0;
            C = result16 > 0xFF;
            
            //Set result:
            A = result8;
        }

        private void AND()
        {
            //Get operand from memory:
            M = Read(_absoluteAddress.word);
            M &= 0xFF;
            
            //Instruction Computation:
            result16 = (ushort)(A & M);
            result8 = (byte)result16;

            //Refresh flag status:
            P &= 0x7D;
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;
            
            //Set result:
            A = result8;
        }

        private void ASL()
        {
            if (_opcode == 0x0A) operand = A;
                else operand = Read(_absoluteAddress.word);
            operand &= 0xFF;
            
            /* Faire le reset des flags impliqués */
            C = ((operand >> 7) & 1 ) == 1;
            operand = (byte)(((operand << 1) | 0) & 0xFF);
            
            N = ((operand >> 7) & 1) == 1;
            Z = operand == 0;

            if (_opcode == 0x0A) A = operand;
                else Write(_absoluteAddress.word, operand);
        }

        private void BCC()
        {
            if (!C)
            {
                _absoluteAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void BCS()
        {
            if (C)
            {
                _absoluteAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void BEQ()
        {
            if (Z)
            {
                _absoluteAddress.word =  (ushort)(PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void BIT()
        {
            //Get operand from memory:
            M = Read(_absoluteAddress.word);
            M &= 0xFF;

            Z = (A & M) == 0;
            N = (M & 0x80) != 0;
            V = (M & 0x60) != 0;
        }

        private void BMI()
        {
            if (N)
            {
                _absoluteAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void BNE()
        {
            if (!Z)
            {
                _absoluteAddress.word = (ushort) (PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void BPL()
        {
            if (!N)
            {
                _absoluteAddress.word = (ushort) (PC + _relativeAddress.word);
                PC = _absoluteAddress.word;   
            }
        }

        private void BRK()
        {
            //! To implement !//
        }

        private void BVC()
        {
            if (!V)
            {
                _absoluteAddress.word = (ushort) (PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }
        
        private void BVS()
        {
            if (V)
            {
                _absoluteAddress.word = (ushort) (PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void CLC()
        {
            C = false;
        }

        private void CLD()
        {
            D = false;
        }

        private void CLI()
        {
            I = false;
        }

        private void CLV()
        {
            V = false;
        }

        private void CMP()
        {
            M = Read(_absoluteAddress.word);
            Z = (A == M);
            C = (A >= M);
            N = (((A - M) >> 7) & 1) == 1;
        }

        private void CPX()
        {
            M = Read(_absoluteAddress.word);
            Z = (X == M);
            C = (X >= M);
            N = (((X - M) >> 7) & 1) == 1;
        }

        private void CPY()
        {
            M = Read(_absoluteAddress.word);
            Z = (Y == M);
            C = (Y >= M);
            N = (((Y - M) >> 7) & 1) == 1;
        }

        private void DEC()
        {
            M = Read(_relativeAddress.word);
            result8 = (byte)(M - 0x01);
            Write(_relativeAddress.word, result8);
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;
        }
        
        
    }
}
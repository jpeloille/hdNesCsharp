using System;
using System.Collections.Generic;

namespace hdNes.Nes
{
    sealed partial class Ricoh2A03
    {
        public class DebugLine
        {
            public string programCounter { get; set; }
            public string opcode { get; set; }
            public ushort opcodeAdress { get; set; }
            
            public byte byteOne { get; set; }
            
            public byte byteTwo { get; set; }
            
            public int byteCount { get; set; }
            
            public AddressingMode addressingMode { get; set; }
            
            public string operandHigh { get; set; }
            public string operandLow { get; set; }
            public string mnemonic { get; set; }
            
        }
        
        public void FetchDebugAndReport(int count)
        {
            List<DebugLine> debugReport = new List<DebugLine>();
            DebugLine currentInstruction;
            
            while (count >= 1)
            {
                FetchOpcode();

                
                if (_instructionAttributes[_opcode] == null)
                {
                    ReportUnknownInstruction();
                    break;
                }
                else
                {                
                    currentInstruction = new DebugLine();
                    currentInstruction.opcode = _opcode.ToString("X2");
                    currentInstruction.opcodeAdress = (ushort)(PC - 1);
                    currentInstruction.programCounter = ((ushort)(PC - 1)).ToString("X4");
                    currentInstruction.addressingMode = _instructionAttributes[_opcode].AddressingMode;
                    currentInstruction.mnemonic = _instructionAttributes[_opcode].Mnemonic;
                    currentInstruction.byteCount = _instructionAttributes[_opcode].NoBytes;
                    
                    _instructionAddressMode[_opcode]();
                    _instructionMethod[_opcode]();
                    count -= _instructionAttributes[_opcode].NoCycles;
                    
                    currentInstruction = DebugCurrentInstruction(currentInstruction);
                    
                    debugReport.Add(currentInstruction);
                    PrintDebugReport(currentInstruction, debugReport.Count);
                }
                

            }
        }
        
        public DebugLine DebugCurrentInstruction(DebugLine argument)
        {
            DebugLine currentInstruction = argument;
            
            switch (_instructionAttributes[_opcode].AddressingMode)
            {
                case AddressingMode.Immediate:
                    currentInstruction.operandHigh = string.Empty;
                    currentInstruction.operandLow = Read(_physicalAddress.word).ToString("X2");
                    break;
                case AddressingMode.Accumulator:
                    currentInstruction.operandHigh = string.Empty;
                    currentInstruction.operandLow = A.ToString("X2");
                    break;
                case AddressingMode.Relative:
                    currentInstruction.operandHigh =string.Empty;;
                    currentInstruction.operandLow = _physicalAddress.word.ToString("X4");
                    currentInstruction.byteOne = Read((ushort)(currentInstruction.opcodeAdress + 1));
                    break;

                default:
                    if (_instructionAttributes[_opcode].AddressingMode == AddressingMode.Relative)
                    {
                        currentInstruction.operandHigh = _physicalAddress.high.ToString("X2");
                        currentInstruction.operandLow = _physicalAddress.low.ToString("X2");
                    }
                    else
                    {
                        switch (_instructionAttributes[_opcode].NoBytes)
                        {
                            case 1:
                                currentInstruction.operandHigh = string.Empty;
                                currentInstruction.operandLow = string.Empty;
                                break;
                            case 2:
                                currentInstruction.operandHigh = string.Empty;
                                currentInstruction.operandLow = _physicalAddress.low.ToString("X2");
                                currentInstruction.byteOne = Read((ushort)(currentInstruction.opcodeAdress + 1));
                                break;
                            case 3:
                                currentInstruction.operandHigh = _physicalAddress.high.ToString("X2");
                                currentInstruction.operandLow = _physicalAddress.low.ToString("X2");
                                currentInstruction.byteOne = Read((ushort)(currentInstruction.opcodeAdress + 1));
                                currentInstruction.byteTwo = Read((ushort)(currentInstruction.opcodeAdress + 2));
                                break;
                            default:
                                currentInstruction.operandHigh = _physicalAddress.high.ToString("X2");
                                currentInstruction.operandLow = _physicalAddress.low.ToString("X2");
                                currentInstruction.byteOne = Read((ushort)(currentInstruction.opcodeAdress + 1));
                                currentInstruction.byteTwo = Read((ushort)(currentInstruction.opcodeAdress + 2));
                                break;
                        }
                    }
                    break;
            }

            return currentInstruction;
        }

        public void PrintDebugReport(DebugLine currentInstruction, int lineNumber)
        {
            
            Console.Write(lineNumber.ToString().PadLeft(5,' ') + " : ");
            Console.Write(currentInstruction.programCounter + " ");
            Console.Write(currentInstruction.opcode + " ");
            switch (currentInstruction.byteCount)
            {
                case 1:
                    Console.Write("   ");
                    Console.Write("   ");
                    break;
                case 2:
                    Console.Write(currentInstruction.byteOne.ToString("X2") + " ");
                    Console.Write("   ");
                    break;
                case 3:
                    Console.Write(currentInstruction.byteOne.ToString("X2") + " ");
                    Console.Write(currentInstruction.byteTwo.ToString("X2") + " ");
                    break;
            }
            Console.Write(" - " + currentInstruction.mnemonic);
            if (currentInstruction.operandLow != String.Empty)
            {
                if (currentInstruction.operandHigh != String.Empty)
                    Console.Write(" 0x" + currentInstruction.operandHigh + currentInstruction.operandLow);
                else
                {
                    Console.Write(" Ox" + currentInstruction.operandLow + "  ");
                }
            }
            else
            {
                Console.Write("       ");
            }
                
            Console.WriteLine(" - A:" + A.ToString("X2") + " X:" + X.ToString("X2")
                              + " Y:" + Y.ToString("X2") + " P:" + P.ToString("X2")
                              + " S:" + S.ToString("X2"));
            
        }

        public void ReportUnknownInstruction()
        {
            Console.WriteLine(((ushort)(PC - 1)).ToString("X4") + " : 0x" + _opcode.ToString("X2") + " opcode unknown."); 
        }
        
        public void Debug(int count)
        {
            List<DebugLine> debugReport = new List<DebugLine>();
            DebugLine debugLine;
            int lineNumber = 1;
            while (count >= 1)
            {
               
                FetchOpcode();

                if (_instructionAttributes[_opcode] == null)
                {
                    Console.WriteLine(((ushort)(PC - 1)).ToString("X4") + " : 0x" + _opcode.ToString("X2") + " opcode unknown.");
                }
                
                debugLine = new DebugLine();
                
                debugLine.programCounter = ((ushort)(PC - 1)).ToString("X4");
                debugLine.opcode = _opcode.ToString("X2");     
                
                _instructionAddressMode[_opcode]();
                _instructionMethod[_opcode]();
                count -= _instructionAttributes[_opcode].NoCycles;

                switch (_instructionAttributes[_opcode].AddressingMode)
                {
                    case AddressingMode.Immediate:
                        debugLine.operandHigh = string.Empty;
                        debugLine.operandLow = Read(_physicalAddress.word).ToString("X2");
                        break;
                    case AddressingMode.Accumulator:
                        debugLine.operandHigh = string.Empty;
                        debugLine.operandLow = A.ToString("X2");
                        break;
                    
                /*
                    case AddressingMode.Relative:
                        debugLine.operandHigh =string.Empty;;
                        debugLine.operandLow = _relativeAddress.low.ToString("X2");
                        break;
      */
                    
                    default:
                        if (_instructionAttributes[_opcode].AddressingMode == AddressingMode.Relative)
                        {
                            debugLine.operandHigh = _physicalAddress.high.ToString("X2");
                            debugLine.operandLow = _physicalAddress.low.ToString("X2");      
                        }
                        else
                        {



                            switch (_instructionAttributes[_opcode].NoBytes)
                            {
                                case 1:
                                    debugLine.operandHigh = string.Empty;
                                    debugLine.operandLow = string.Empty;
                                    break;
                                case 2:
                                    debugLine.operandHigh = string.Empty;
                                    debugLine.operandLow = _physicalAddress.low.ToString("X2");
                                    break;
                                case 3:
                                    debugLine.operandHigh = _physicalAddress.high.ToString("X2");
                                    debugLine.operandLow = _physicalAddress.low.ToString("X2");
                                    break;
                                default:
                                    debugLine.operandHigh = _physicalAddress.high.ToString("X2");
                                    debugLine.operandLow = _physicalAddress.low.ToString("X2");
                                    break;
                            }
                        }

                        break;
                }

                debugLine.mnemonic = _instructionAttributes[_opcode].Mnemonic;

                debugReport.Add(debugLine);

                Console.Write(lineNumber.ToString().PadLeft(5,' ') + " : ");
                Console.Write(debugLine.programCounter + " ");
                Console.Write(debugLine.opcode + " ");
                if (debugLine.operandLow != String.Empty)
                    Console.Write(debugLine.operandLow + " ");
                else 
                    Console.Write("   ");
                if (debugLine.operandHigh != String.Empty)
                    Console.Write(debugLine.operandHigh + " ");
                else
                {
                    Console.Write("   ");
                }
                Console.Write(" - " + debugLine.mnemonic);
                if (debugLine.operandLow != String.Empty)
                {
                    if (debugLine.operandHigh != String.Empty)
                        Console.Write(" 0x" + debugLine.operandHigh + debugLine.operandLow);
                    else
                    {
                        Console.Write(" Ox" + debugLine.operandLow + "  ");
                    }
                }
                else
                {
                    Console.Write("       ");
                }
                
                Console.WriteLine(" - A:" + A.ToString("X2") + " X:" + X.ToString("X2")
                                  + " Y:" + Y.ToString("X2") + " P:" + P.ToString("X2")
                                  + " S:" + S.ToString("X2"));

                lineNumber++;
            }
        }
    }
}
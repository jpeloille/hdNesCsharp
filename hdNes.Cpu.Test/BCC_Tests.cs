using NUnit.Framework;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class BCC_Tests
    {
        [Test]
        public void BCC_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x90, 0x0A, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.psr.C = true;
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0xC000, board._cpu2A03.utPC);
            Assert.AreEqual(0xC002, board._cpu2A03.utPC);
        }
        
        [Test]
        public void BCC_CarryClear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x90, 0x0A, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.psr.C = false;
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0xC000, board._cpu2A03.utPC);
            Assert.AreEqual(0xC00C, board._cpu2A03.utPC);
        }
        
        [Test]
        public void BCC_CarryClear_PageBoundary()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x00});
            board.UnitTest_Reset();
            
            board.CpuWrite(0x0F0, 0x90);
            board.CpuWrite(0x0F1, 0x79);
            board.CpuWrite(0x0F2, 0x00);
            board._cpu2A03.psr.C = false;
            board._cpu2A03.utPC = 0xF0;
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.utPC);
            Assert.AreEqual(0x016B, board._cpu2A03.utPC);
            
        }        
    }
}
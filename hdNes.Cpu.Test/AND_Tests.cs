using NUnit.Framework;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class AND_Tests
    {
        [Test]
        public void AND_Immediate()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x29, 0x0A});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7F;
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x7F, board._cpu2A03.regA);
            Assert.AreEqual(0x0A, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
        }
        
        [Test]
        public void AND_Immediate_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x29, 0x05});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x0A;
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x0A, board._cpu2A03.regA);
            Assert.AreEqual(0x00, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(true, board._cpu2A03.Z);
        }
        
        [Test]
        public void AND_Immediate_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x29, 0x80});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0xFF;
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x0A, board._cpu2A03.regA);
            Assert.AreEqual(0x80, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(true, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
        } 
    }
}
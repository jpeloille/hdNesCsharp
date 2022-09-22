using NUnit.Framework;
using hdNes;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class BRK_Tests
    {
        [Test]
        public void BRK()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0xFFFE, 0xFE);
            board.cpu.Write(0xFFFF, 0xC0);
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0xC000, board.cpu.PC);
            Assert.AreEqual(0xC0FE, board.cpu.PC);
            
            //Check flags output:
            
        }
    }
}


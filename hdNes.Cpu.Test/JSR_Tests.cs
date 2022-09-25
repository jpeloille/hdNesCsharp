using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class JSR_Tests
    {
        [Test]
        public void JSR_Absolute()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x20, 0x00, 0xC1 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xC003, board.cpu.PC);
            Assert.AreEqual(0xC100, board.cpu.PC);
        }   
    }
}
using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class JMP_Tests
    {
        [Test]
        public void JMP_Absolute()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x4C, 0x0F, 0xC0 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xC000, board.cpu.PC);
            Assert.AreEqual(0xC00F, board.cpu.PC);
        }   
        
        [Test]
        public void JMP_Indirect()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x6C, 0x03, 0xC0, 0x0F, 0xC0 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xC000, board.cpu.PC);
            Assert.AreEqual(0xC00F, board.cpu.PC);
        } 
                
        [Test]
        public void JMP_Indirect_PageBoundaryBug()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x6C, 0xFF, 0xC0 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.PC);
            Assert.AreEqual(0x6C00, board.cpu.PC);
        } 
    }
}
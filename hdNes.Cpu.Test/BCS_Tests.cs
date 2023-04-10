using System.Transactions;
using NUnit.Framework;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class BCS_Tests
    {
        [Test]
        public void BCS_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0xB0, 0x0A, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.C = false;
            
            board.cpu.Tick(1);
            
            //Verify Memory Values
            Assert.AreNotEqual(0xC000, board.cpu.PC, "PC should not be equal to $C000 !");
            Assert.AreEqual(0xC002, board.cpu.PC, "PC should be equal to $C002 !");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.C);
            Assert.AreEqual(false, board.cpu.N);
        }
        
        [Test]
        public void BCS_CarryClear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0xB0, 0x0A, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.C = true;
            
            board.cpu.Tick(1);
            
            //Verify Memory Values
            Assert.AreNotEqual(0xC000, board.cpu.PC);
            Assert.AreEqual(0xC00C, board.cpu.PC);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(true, board.cpu.C);
            Assert.AreEqual(false, board.cpu.N);
        } 
        
        [Test]
        public void BCS_CarryClear_PageBoundary()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00F0,0xB0);
            board.cpu.Write(0x00F1,0x79);
            board.cpu.Write(0x00F2,0x00);
            board.cpu.C = true;
            board.cpu.PC = 0xF0;
            
            board.cpu.Tick(1);
            
            //Verify Memory Values
            Assert.AreNotEqual(0x00, board.cpu.PC);
            Assert.AreEqual(0x16B, board.cpu.PC);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(true, board.cpu.C);
            Assert.AreEqual(false, board.cpu.N);
        } 
    }
}
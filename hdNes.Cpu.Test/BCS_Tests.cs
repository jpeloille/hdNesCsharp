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
            board.UnitTest_Reset();
            board._cpu2A03.C = false;
            
            board._cpu2A03.Tick(1);
            
            //Verify Memory Values
            Assert.AreNotEqual(0xC000, board._cpu2A03.utPC);
            Assert.AreEqual(0xC002, board._cpu2A03.utPC);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        }
        
        [Test]
        public void BCS_CarryClear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0xB0, 0x0A, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.C = true;
            
            board._cpu2A03.Tick(1);
            
            //Verify Memory Values
            Assert.AreNotEqual(0xC000, board._cpu2A03.utPC);
            Assert.AreEqual(0xC00C, board._cpu2A03.utPC);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        [Test]
        public void BCS_CarryClear_PageBoundary()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0x00F0,0xB0);
            board._cpu2A03.UnitTest_Write(0x00F1,0x79);
            board._cpu2A03.UnitTest_Write(0x00F2,0x00);
            board._cpu2A03.C = true;
            board._cpu2A03.utPC = 0xF0;
            
            board._cpu2A03.Tick(1);
            
            //Verify Memory Values
            Assert.AreNotEqual(0x00, board._cpu2A03.utPC);
            Assert.AreEqual(0x16B, board._cpu2A03.utPC);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
    }
}
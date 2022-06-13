using System.Transactions;
using NUnit.Framework;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class BIT_Tests
    {
        [Test]
        public void BIT_ZeroPage_ZeroClear_NegativeClear_OverflowClear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x24,0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0x00,0x01);
            board._cpu2A03.Z = true;
            board._cpu2A03.N = true;
            board._cpu2A03.V = true;
            board._cpu2A03.regA = 0x7F;
            
            board._cpu2A03.Tick(1);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
        }
     
        [Test]
        public void BIT_ZeroPage_ZeroClear_NegativeClear_Overflow()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x24,0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0x00,0x7F);
            board._cpu2A03.Z = true;
            board._cpu2A03.N = true;
            board._cpu2A03.V = true;
            board._cpu2A03.regA = 0x7F;
            
            board._cpu2A03.Tick(1);
            
            //Check flags output:
             Assert.AreEqual(false, board._cpu2A03.Z);     
             Assert.AreEqual(true, board._cpu2A03.V);            
             Assert.AreEqual(false, board._cpu2A03.N);
        }
        
        [Test]
        public void BIT_ZeroPage_Zero_NegativeClear_OverflowClear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x24,0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0x00,0x00);
            board._cpu2A03.Z = true;
            board._cpu2A03.N = true;
            board._cpu2A03.V = true;
            board._cpu2A03.regA = 0x7F;
            
            board._cpu2A03.Tick(1);
            
            //Check flags output:
            Assert.AreEqual(true, board._cpu2A03.Z);     
            Assert.AreEqual(false, board._cpu2A03.V);            
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        [Test]
        public void BIT_ZeroPage_ZeroClear_Negative_OverflowClear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x24,0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0x00,0x80);
            board._cpu2A03.Z = true;
            board._cpu2A03.N = true;
            board._cpu2A03.V = true;
            board._cpu2A03.regA = 0x80;
            
            board._cpu2A03.Tick(1);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);     
            Assert.AreEqual(false, board._cpu2A03.V);            
            Assert.AreEqual(true, board._cpu2A03.N);
        } 
        
        [Test]
        public void BIT_Absolute_ZeroClear_NegativeClear_OverflowClear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x2C,0x00, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0x00,0x01);
            board._cpu2A03.Z = true;
            board._cpu2A03.N = true;
            board._cpu2A03.V = true;
            board._cpu2A03.regA = 0x7F;
            
            board._cpu2A03.Tick(1);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);     
            Assert.AreEqual(false, board._cpu2A03.V);            
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        
    }
}
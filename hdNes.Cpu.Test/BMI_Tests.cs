using NUnit.Framework;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class BMI_Tests
    {
        [Test]
        public void BMI_NegativeClear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x30, 0x0A, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.N = false;
            
            board._cpu2A03.Tick(1);

            //Verify Memory Values
            Assert.AreNotEqual(0xC000, board._cpu2A03.utPC);
            Assert.AreEqual(0xC002, board._cpu2A03.utPC);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
        }     
        
        [Test]
        public void BMI_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x30, 0x0A, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.N = true;
            
            board._cpu2A03.Tick(1);

            //Verify Memory Values
            Assert.AreNotEqual(0xC000, board._cpu2A03.utPC);
            Assert.AreEqual(0xC00C, board._cpu2A03.utPC);
            
            //Check flags output:
            Assert.AreEqual(true, board._cpu2A03.N);
        }  
        
        [Test]
        public void BMI_Negative_PageBoundary()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0xF0,0x30);
            board._cpu2A03.UnitTest_Write(0xF1,0x79);
            board._cpu2A03.UnitTest_Write(0xF2,0x00);
            board._cpu2A03.N = true;
            board._cpu2A03.utPC = 0xF0;
            
            board._cpu2A03.Tick(1);

            //Verify Memory Values
            Assert.AreNotEqual(0x00, board._cpu2A03.utPC);
            Assert.AreEqual(0x16B, board._cpu2A03.utPC);
            
            //Check flags output:
            Assert.AreEqual(true, board._cpu2A03.N);
        }  
    }
}
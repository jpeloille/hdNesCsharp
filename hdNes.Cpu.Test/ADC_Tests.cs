using NUnit.Framework;
using hdNes;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class ADC_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ADC_Immediate()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x69,0x01});
            board.UnitTest_Reset();
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.psr.N);
            Assert.AreEqual(false, board._cpu2A03.psr.Z);
            Assert.AreEqual(false, board._cpu2A03.psr.V);
            Assert.AreEqual(false, board._cpu2A03.psr.C);
            
        }
        
        [Test]
        public void ADC_Immediate_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x69,0x02});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0xFF;
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.psr.N);
            Assert.AreEqual(false, board._cpu2A03.psr.Z);
            Assert.AreEqual(false, board._cpu2A03.psr.V);
            Assert.AreEqual(true, board._cpu2A03.psr.C);
        }

        [Test]
        public void ADC_Immediate_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x69,0x81});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7F;
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x7F, board._cpu2A03.regA);
            Assert.AreEqual(0x00, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.psr.N);
            Assert.AreEqual(true, board._cpu2A03.psr.Z);
            Assert.AreEqual(false, board._cpu2A03.psr.V);
            Assert.AreEqual(true, board._cpu2A03.psr.C);    
        }
        
        [Test]
        public void ADC_Absolute()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x6D, 0x03, 0xC0, 0x40});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x40;
            
            board._cpu2A03.Tick(1);

            //Check register values output:
            //Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x80, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(true, board._cpu2A03.psr.N);
            Assert.AreEqual(false, board._cpu2A03.psr.Z);
            Assert.AreEqual(true, board._cpu2A03.psr.V);
            Assert.AreEqual(false, board._cpu2A03.psr.C);
        }
        
    }
}
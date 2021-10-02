using NUnit.Framework;
using hdNes;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class ADC_Tests
    {
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
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);
            
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
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(true, board._cpu2A03.C);
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
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(true, board._cpu2A03.C);    
        }

        [Test]
        public void ADC_ZeroPage()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x65, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0x0000,0x01);
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);          
        }

        [Test]
        public void ADC_ZeroPageX()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x75, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.UnitTest_Write(0x0000,0x01);
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);     
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
            Assert.AreEqual(true, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);
        }

        [Test]
        public void ADC_AbsoluteX()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x7D, 0x02, 0xC0, 0x01});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x00;
            board._cpu2A03.regX = 0x01;
            
            board._cpu2A03.Tick(1);

            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);
        }

        [Test]
        public void ADC_AbsoluteX_PageBoundary()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x7D, 0x01, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x00;
            board._cpu2A03.regX = 0xFF;
            board._cpu2A03.UnitTest_Write(0x0100,0x01);

            board._cpu2A03.Tick(1);

            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);
        }
        
        [Test]
        public void ADC_AbsoluteY()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x79, 0x02, 0xC0, 0x01});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x00;
            board._cpu2A03.regY = 0x01;
            
            board._cpu2A03.Tick(1);

            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);
        }
        
        [Test]
        public void ADC_AbsoluteY_PageBoundary()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x79, 0x01, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x00;
            board._cpu2A03.regY = 0xFF;
            board._cpu2A03.UnitTest_Write(0x0100,0x01);

            board._cpu2A03.Tick(1);

            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);
        }
        
        [Test]
        public void ADC_IndexedIndirect()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x61, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x00;
            board._cpu2A03.regX = 0x01;
            board._cpu2A03.UnitTest_Write(0x0000,0x00);
            board._cpu2A03.UnitTest_Write(0x0001,0x03);
            board._cpu2A03.UnitTest_Write(0x0002,0x00);
            board._cpu2A03.UnitTest_Write(0x0003,0x01);
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);     
        }
        
        [Test]
        public void ADC_IndirectIndexed()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x71, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x00;
            board._cpu2A03.regY = 0x01;
            board._cpu2A03.UnitTest_Write(0x0000,0x01);
            board._cpu2A03.UnitTest_Write(0x0001,0x00);
            board._cpu2A03.UnitTest_Write(0x0002,0x01);
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);     
        }
        
        [Test]
        public void ADC_IndirectIndexed_PageBoundary()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x71, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x00;
            board._cpu2A03.regY = 0x01;
            board._cpu2A03.UnitTest_Write(0x0000,0xFF);
            board._cpu2A03.UnitTest_Write(0x0001,0x00);
            board._cpu2A03.UnitTest_Write(0x0100,0x01);
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board._cpu2A03.regA);
            Assert.AreEqual(0x01, board._cpu2A03.regA);
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.N);
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.V);
            Assert.AreEqual(false, board._cpu2A03.C);     
        }   

    }
}
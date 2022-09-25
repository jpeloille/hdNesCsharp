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
            board.cpu.SetInUnitTestInitialState();
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);
            
        }
        
        [Test]
        public void ADC_Immediate_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x69,0x02});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(true, board.cpu.C);
        }

        [Test]
        public void ADC_Immediate_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x69,0x81});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x7F;
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x7F, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(true, board.cpu.C);    
        }

        [Test]
        public void ADC_ZeroPage()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x65, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x0000,0x01);
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);          
        }

        [Test]
        public void ADC_ZeroPageX()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x75, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x0000,0x01);
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);     
        }
        
        [Test]
        public void ADC_Absolute()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x6D, 0x03, 0xC0, 0x40 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x40;
            
            board.cpu.Tick(1);

            //Check register values output:
            //Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x80, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(true, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(true, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);
        }

        [Test]
        public void ADC_AbsoluteX()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x7D, 0x02, 0xC0, 0x01});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x00;
            board.cpu.X = 0x01;
            
            board.cpu.Tick(1);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);
        }

        [Test]
        public void ADC_AbsoluteX_PageBoundary()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x7D, 0x01, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x00;
            board.cpu.X = 0xFF;
            board.cpu.Write(0x0100,0x01);

            board.cpu.Tick(1);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);
        }
        
        [Test]
        public void ADC_AbsoluteY()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x79, 0x02, 0xC0, 0x01});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x00;
            board.cpu.Y = 0x01;
            
            board.cpu.Tick(1);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);
        }
        
        [Test]
        public void ADC_AbsoluteY_PageBoundary()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0x79, 0x01, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x00;
            board.cpu.Y = 0xFF;
            board.cpu.Write(0x0100,0x01);

            board.cpu.Tick(1);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);
        }
        
        [Test]
        public void ADC_IndexedIndirect()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x61, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x00;
            board.cpu.X = 0x01;
            board.cpu.Write(0x0000,0x00);
            board.cpu.Write(0x0001,0x03);
            board.cpu.Write(0x0002,0x00);
            board.cpu.Write(0x0003,0x01);
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);     
        }
        
        [Test]
        public void ADC_IndirectIndexed()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x71, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x00;
            board.cpu.Y = 0x01;
            board.cpu.Write(0x0000,0x01);
            board.cpu.Write(0x0001,0x00);
            board.cpu.Write(0x0002,0x01);
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);     
        }
        
        [Test]
        public void ADC_IndirectIndexed_PageBoundary()
        {
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x71, 0x00});
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x00;
            board.cpu.Y = 0x01;
            board.cpu.Write(0x00,0xFF);
            board.cpu.Write(0x01,0x00);
            board.cpu.Write(0x100,0x01);
            
            board.cpu.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.V);
            Assert.AreEqual(false, board.cpu.C);     
        }   

    }
}
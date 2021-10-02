using NUnit.Framework;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class ASL_Tests
    {
        [Test]
        public void ASL_ZeroPage_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x06, 0x00});
            board.UnitTest_Reset();
            board.CpuWrite(0x0000,0x7F);
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x7F, board.CpuRead(0x0000));
            Assert.AreEqual(0xFE, board.CpuRead(0x0000));
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.C);
            Assert.AreEqual(true, board._cpu2A03.N);
        }
        
        [Test]
        public void ASL_ZeroPage_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x06, 0x00});
            board.UnitTest_Reset();
            board.CpuWrite(0x0000,0x80);
            
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x80, board.CpuRead(0x0000));
            Assert.AreEqual(0x00, board.CpuRead(0x0000));
            
            //Check flags output:
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        }
        
        [Test]
        public void ASL_ZeroPage()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x06, 0x00});
            board.UnitTest_Reset();
            board.CpuWrite(0x0000,0x01);
            
            
            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x01, board.CpuRead(0x0000));
            Assert.AreEqual(0x02, board.CpuRead(0x0000));
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        }
        
        [Test]
        public void ASL_ZeroPageX()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x16, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.regX = 0x01;
            board.CpuWrite(0x0001,0x01);

            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x01, board.CpuRead(0x0001));
            Assert.AreEqual(0x02, board.CpuRead(0x0001));
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        }
        
        [Test]
        public void ASL_Absolute()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x0E, 0x00, 0x00});
            board.UnitTest_Reset();
            board.CpuWrite(0x0000,0x01);

            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(0x01, board.CpuRead(0x0000));
            Assert.AreEqual(0x02, board.CpuRead(0x0000));
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        }
        
        [Test]
        public void ASL_AbsoluteX()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[]{0x1E, 0x00, 0x00});
            board.UnitTest_Reset();
            board._cpu2A03.regX = 0x01;
            board.CpuWrite(0x0001,0x01);

            board._cpu2A03.Tick(1);
            
            //Check register values output:
            Assert.AreNotEqual(001, board.CpuRead(0x0001));
            Assert.AreEqual(0x02, board.CpuRead(0x0001));
            
            //Check flags output:
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        }
    }
}
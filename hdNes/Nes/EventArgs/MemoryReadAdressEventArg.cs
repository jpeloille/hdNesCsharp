namespace hdNes.Nes.EventArgs
{
    public class MemoryReadAdressEventArg : System.EventArgs
    {
        public bool isDdataReady = false;
        public byte data { get; set; }
        public uint address { get; set; }

        public MemoryReadAdressEventArg(uint address)
        {
            this.address = address;
        }
    }
}
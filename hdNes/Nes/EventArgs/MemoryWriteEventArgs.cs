namespace hdNes.Nes.EventArgs
{
    public class MemoryWriteEventArgs : System.EventArgs
    {
        public bool hasBeenWriten = false;
        public byte data { get; set; }
        public uint address { get; set; }

        public MemoryWriteEventArgs(uint address, byte data)
        {
            this.address = address;
            this.data = data;
        }
    }
}
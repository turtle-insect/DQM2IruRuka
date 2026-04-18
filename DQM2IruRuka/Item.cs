namespace DQM2IruRuka
{
	internal class Item
	{
		public uint ID { get;private set; }
		private readonly uint mAddress;

		public Item(uint address, uint id)
		{
			mAddress = address;
			ID = id;
		}

		public uint Count
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 1); }
			set { Util.WriteNumber(mAddress, 1, value, 0, 999); }
		}
	}
}

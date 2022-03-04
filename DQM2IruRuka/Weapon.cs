using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQM2IruRuka
{
	internal class Weapon : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		private readonly uint mAddress;

		public Weapon(uint address)
		{
			mAddress = address;
		}

		public uint ID
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
			}
		}

		public uint Star
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 2, 1); }
			set
			{
				Util.WriteNumber(mAddress + 2, 1, value, 0, 3);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Star)));
			}
		}

		public uint Option1
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 4, 1); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 4, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Option1)));
			}
		}

		public uint Option2
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 5, 1); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 5, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Option2)));
			}
		}

		public uint Option3
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 6, 1); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 6, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Option3)));
			}
		}
	}
}

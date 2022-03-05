using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQM2IruRuka
{
	internal class Egg : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		private readonly uint mAddress;

		public Egg(uint address)
		{
			mAddress = address;
		}

		public uint Exp
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 4); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress, 4, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exp)));
			}
		}

		public uint Type
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 4, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 4, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
			}
		}

		public uint Lv
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 6, 1); }
			set
			{
				Util.WriteNumber(mAddress + 6, 1, value, 1, 99);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lv)));
			}
		}

		public uint Plus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 7, 1); }
			set
			{
				Util.WriteNumber(mAddress + 7, 1, value, 1, 99);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Plus)));
			}
		}
	}
}


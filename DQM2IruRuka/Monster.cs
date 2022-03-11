using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQM2IruRuka
{
	internal class Monster : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public Number Type { get; private set; }

		public Number Skill1 { get; private set; }
		public Number Skill2 { get; private set; }

		public Number Father { get; private set; }
		public Number Mother { get; private set; }
		public Number GrandFather_F { get; private set; }
		public Number GrandMother_F { get; private set; }
		public Number GrandFather_M { get; private set; }
		public Number GrandMother_M { get; private set; }


		public Monster(uint address)
		{
			mAddress = address;

			Type = new Number(address + 32, 2);
			Skill1 = new Number(address + 194, 4);
			Skill2 = new Number(address + 198, 4);

			Father = new Number(address + 124, 2);
			Mother = new Number(address + 126, 2);
			GrandFather_F = new Number(address + 128, 2);
			GrandMother_F = new Number(address + 132, 2);
			GrandFather_M = new Number(address + 130, 2);
			GrandMother_M = new Number(address + 134, 2);
		}

		public uint Lv
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 72, 1); }
			set
			{
				Util.WriteNumber(mAddress + 72, 1, value, 1, 99);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lv)));
			}
		}

		public uint MaxHP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 56, 2); }
			set
			{
				Util.WriteNumber(mAddress + 56, 2, value, 1, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxHP)));
			}
		}

		public uint MaxMP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 58, 2); }
			set
			{
				Util.WriteNumber(mAddress + 58, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMP)));
			}
		}

		public uint HP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 60, 2); }
			set
			{
				Util.WriteNumber(mAddress + 60, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HP)));
			}
		}

		public uint MP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 62, 2); }
			set
			{
				Util.WriteNumber(mAddress + 62, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MP)));
			}
		}

		public uint Offense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 64, 2); }
			set
			{
				Util.WriteNumber(mAddress + 64, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Offense)));
			}
		}

		public uint Defense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 66, 2); }
			set
			{
				Util.WriteNumber(mAddress + 66, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Defense)));
			}
		}

		public uint Speed
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 68, 2); }
			set
			{
				Util.WriteNumber(mAddress + 68, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speed)));
			}
		}

		public uint Wise
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 70, 2); }
			set
			{
				Util.WriteNumber(mAddress + 70, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Wise)));
			}
		}

		public uint Exp
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 80, 4); }
			set
			{
				Util.WriteNumber(mAddress + 80, 4, value, 0, 9999999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exp)));
			}
		}

		public uint SkillPoint
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 120, 2); }
			set
			{
				Util.WriteNumber(mAddress + 120, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SkillPoint)));
			}
		}



		private readonly uint mAddress;
	}
}

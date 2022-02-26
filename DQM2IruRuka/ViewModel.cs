using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQM2IruRuka
{
	internal class ViewModel
	{
		private Info mInfo = Info.Instance();
		public ObservableCollection<Monster> Monsters { get; private set; } = new ObservableCollection<Monster>();

		public ViewModel()
		{
			for (uint index = 0; index < 500; index++)
			{
				Monster monster = new Monster(0x4580 + index * 220);
				if (monster.Type == 0) continue;

				Monsters.Add(monster);
			}
		}

		public uint Money
		{
			get { return SaveData.Instance().ReadNumber(0x10C, 4); }
			set { Util.WriteNumber(0x10C, 4, value, 0, 999999); }
		}

		public uint Bank
		{
			get { return SaveData.Instance().ReadNumber(0x110, 4); }
			set { Util.WriteNumber(0x110, 4, value, 0, 99999999); }
		}
	}
}

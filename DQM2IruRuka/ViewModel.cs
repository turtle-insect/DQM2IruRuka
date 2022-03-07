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
		public String ItemFilter { get; set; } = "";
		private List<Item> AllItems = new List<Item>();
		public Info Info { get; private set; } = Info.Instance();
		public ObservableCollection<Item> Items { get; private set; } = new ObservableCollection<Item>();
		public ObservableCollection<Egg> Eggs { get; private set; } = new ObservableCollection<Egg>();
		public ObservableCollection<Weapon> Weapons { get; private set; } = new ObservableCollection<Weapon>();
		public ObservableCollection<Monster> Monsters { get; private set; } = new ObservableCollection<Monster>();

		public ViewModel()
		{
			foreach (var itemInfo in Info.Instance().Item)
			{
				AllItems.Add(new Item(0x136 + (itemInfo.Value - 1) * 2, itemInfo.Value));
			}
			FilterItem();

			{
				Egg egg = new Egg(0xCB0);
				if(egg.Type != 0)
				{
					Eggs.Add(egg);
				}
			}

			for (uint index = 0; index < 100; index++)
			{
				Weapon weapon = new Weapon(0x40B4 + index * 12);
				if (weapon.ID == 0) continue;

				Weapons.Add(weapon);
			}

			for (uint index = 0; index < 500; index++)
			{
				Monster monster = new Monster(0x4580 + index * 220);
				if (monster.Type == 0) continue;

				Monsters.Add(monster);
			}
		}

		public void FilterItem()
		{
			Items.Clear();
			foreach (var item in AllItems)
			{
				String name = Info.Instance().Search(Info.Instance().Item, item.ID)?.Name;
				if (name?.IndexOf(ItemFilter) != -1) Items.Add(item);
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

		public uint MiniMedalHave
		{
			get { return SaveData.Instance().ReadNumber(0x33C, 1); }
			set { Util.WriteNumber(0x33C, 1, value, 0, 150); }
		}

		public uint MiniMedalPass
		{
			get { return SaveData.Instance().ReadNumber(0xAA7, 2); }
			set { Util.WriteNumber(0xAA7, 2, value, 0, 150); }
		}

		public uint BattleWin
		{
			get { return SaveData.Instance().ReadNumber(0x548, 2); }
			set { SaveData.Instance().WriteNumber(0x548, 2, value); }
		}

		public uint Scout
		{
			get { return SaveData.Instance().ReadNumber(0x54A, 2); }
			set { SaveData.Instance().WriteNumber(0x54A, 2, value); }
		}

		public uint Alignment
		{
			get { return SaveData.Instance().ReadNumber(0x54C, 2); }
			set { SaveData.Instance().WriteNumber(0x54C, 2, value); }
		}

		public uint Cooperation
		{
			get { return SaveData.Instance().ReadNumber(0x55A, 2); }
			set { SaveData.Instance().WriteNumber(0x55A, 2, value); }
		}

		public uint Key
		{
			get { return SaveData.Instance().ReadNumber(0x55C, 2); }
			set { SaveData.Instance().WriteNumber(0x55C, 2, value); }
		}

		public uint Clear
		{
			get { return SaveData.Instance().ReadNumber(0x55E, 2); }
			set { SaveData.Instance().WriteNumber(0x55E, 2, value); }
		}

		public uint Dress
		{
			get { return SaveData.Instance().ReadNumber(0x2373F, 1); }
			set { SaveData.Instance().WriteNumber(0x2373F, 1, value); }
		}

		public uint Passing
		{
			get { return SaveData.Instance().ReadNumber(0x10F8, 2); }
			set { SaveData.Instance().WriteNumber(0x10F8, 2, value); }
		}

		public uint PassingWin
		{
			get { return SaveData.Instance().ReadNumber(0x1100, 2); }
			set { Util.WriteNumber(0x1100, 2, value, 0, 9999); }
		}

		public uint MastersGPRank
		{
			get { return SaveData.Instance().ReadNumber(0x215F8, 2); }
			set { SaveData.Instance().WriteNumber(0x215F8, 2, value); }
		}

		public uint MastersGPJoin
		{
			get { return SaveData.Instance().ReadNumber(0x215FC, 2); }
			set { SaveData.Instance().WriteNumber(0x215FC, 2, value); }
		}

		public uint WiFiDanPoint
		{
			get { return SaveData.Instance().ReadNumber(0x1F360, 2); }
			set { Util.WriteNumber(0x1F360, 2, value, 0, 9999); }
		}
	}
}

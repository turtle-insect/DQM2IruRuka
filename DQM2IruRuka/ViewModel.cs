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
	}
}

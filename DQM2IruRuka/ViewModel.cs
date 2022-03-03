﻿using System;
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
		public ObservableCollection<Item> Items { get; private set; } = new ObservableCollection<Item>();
		private List<Item> AllItems = new List<Item>();
		public ObservableCollection<Monster> Monsters { get; private set; } = new ObservableCollection<Monster>();

		public ViewModel()
		{
			foreach (var itemInfo in Info.Instance().Item)
			{
				AllItems.Add(new Item(0x136 + (itemInfo.Value - 1) * 2, itemInfo.Value));
			}
			FilterItem();

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

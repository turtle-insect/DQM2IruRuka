﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQM2IruRuka
{
	internal class Info
	{
		private static Info mThis;
		public List<NameValueInfo> Item { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Type { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Skill { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Weapon { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Option { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Dress { get; private set; } = new List<NameValueInfo>();

		private Info() { }

		public static Info Instance()
		{
			if (mThis == null)
			{
				mThis = new Info();
				mThis.Init();
			}
			return mThis;
		}

		public NameValueInfo Search<Type>(List<Type> list, uint id)
			where Type : NameValueInfo, new()
		{
			int min = 0;
			int max = list.Count;
			for (; min < max;)
			{
				int mid = (min + max) / 2;
				if (list[mid].Value == id) return list[mid];
				else if (list[mid].Value > id) max = mid;
				else min = mid + 1;
			}
			return null;
		}

		private void Init()
		{
			AppendList("info\\item.txt", Item);
			AppendList("info\\type.txt", Type);
			AppendList("info\\skill.txt", Skill);
			AppendList("info\\weapon.txt", Weapon);
			AppendList("info\\option.txt", Option);
			AppendList("info\\dress.txt", Dress);
		}

		private void AppendList<Type>(String filename, List<Type> items)
			where Type : ILineAnalysis, new()
		{
			if (!System.IO.File.Exists(filename)) return;
			String[] lines = System.IO.File.ReadAllLines(filename);
			foreach (String line in lines)
			{
				if (line.Length < 3) continue;
				if (line[0] == '#') continue;
				String[] values = line.Split('\t');
				if (values.Length < 2) continue;
				if (String.IsNullOrEmpty(values[0])) continue;
				if (String.IsNullOrEmpty(values[1])) continue;

				Type type = new Type();
				if (type.Line(values))
				{
					items.Add(type);
				}
			}

			items.Sort();
		}
	}
}

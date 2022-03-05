using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DQM2IruRuka
{
	internal class EggExpConverter : IValueConverter
	{
		private readonly List<uint> ExpList = new List<uint>() { 1000, 10000, 100000, 1000000 };
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint exp = (uint)value;
			for (int index = 0; index < ExpList.Count; index++)
			{
				if (ExpList[index] == exp) return index;
			}

			return -1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ExpList[(int)(uint)value];
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQM2IruRuka
{
	internal class Util
	{
		public static void WriteNumber(uint address, uint size, uint value, uint min, uint max)
		{
			if (value > max) value = max;
			if (value < min) value = min;
			SaveData.Instance().WriteNumber(address, size, value);
		}

		public static String ReadName(uint address)
		{
			var normal = new Dictionary<int, String>();
			// 0 - 9
			for (int index = 0; index < 10; index++)
			{
				normal.Add(index, (index + 1).ToString());
			}
			// A - Z
			// a - z
			for (int index = 0; index < 26; index++)
			{
				normal.Add(index + 13, ((Char)('A' + index)).ToString());
				normal.Add(index + 39, ((Char)('a' + index)).ToString());
			}
			// 
			var spell = new[]
			{
				new { index = 65, size = 4, value = "A",},
				new { index = 69, size = 1, value = "C", },
				new { index = 70, size = 4, value = "E", },
				new { index = 74, size = 4, value = "I", },
				new { index = 78, size = 1, value = "N", },
				new { index = 79, size = 4, value = "O", },
				new { index = 83, size = 1, value = "D", },
				new { index = 84, size = 4, value = "U", },
				new { index = 88, size = 4, value = "a",},
				new { index = 92, size = 1, value = "c",},
				new { index = 93, size = 4, value = "e",},
				new { index = 97, size = 4, value = "i",},
				new { index = 101, size = 1, value = "n",},
				new { index = 102, size = 4, value = "o",},
				new { index = 106, size = 1, value = "d",},
				new { index = 107, size = 4, value = "u",},
			};
			foreach (var item in spell)
			{
				for (int index = 0; index < item.size; index++)
				{
					normal.Add(index + item.index, item.value);
				}
			}
			// 空白
			normal.Add(12, "　");
			// ひらがな
			// 濁点・半濁点は別
			String tmp = "あぁいぃうぅえぇおぉかきくけこさしすせそたちつってとなにぬねのはひふへほまみむめもやゃゆゅよょらりるれろわをん";
			foreach (var ch in tmp.Select((value, index) => new { value, index }))
			{
				normal.Add(ch.index + 111, ch.value.ToString());
			}
			// カタカナ
			tmp = "アァイィウゥエェオォカキクケコサシスセソタチツッテトナニヌネノハヒフヘホマミムメモヤャユュヨョラリルレロワヲン";
			foreach (var ch in tmp.Select((value, index) => new { value, index }))
			{
				normal.Add(ch.index + 166, ch.value.ToString());
			}

			var e0 = new Dictionary<int, String>();
			tmp = "がぎぐげござじずぜぞだぢづでどばびぶべぼガギグゲゴザジズゼゾダヂヅデドバビブベボぱぴぷぺぽパピプペポ";
			foreach (var ch in tmp.Select((value, index) => new { value, index }))
			{
				e0.Add(ch.index + 1, ch.value.ToString());
			}
			var e1 = new Dictionary<int, String>()
			{
				{0x52, "ー"},
				{0x50, "。"},
				{0x47, "、"},
				{0x21, "Ⅰ"},
				{0x22, "Ⅱ"},
				{0x23, "Ⅲ"},
				{0x24, "Ⅴ"},
				{0x25, "Ⅹ"},
				{0x1b, "＋"},
				{0x28, "ー"},
				{0x3d, "✖"},
				{0x1c, "＝"},
				{0x09, "＿"},
				{0x3b, "．"},
				{0x45, "，"},
				{0x3f, "■"},
				{0x3e, "："},
				{0x26, "；"},
				{0x4d, "”"},
				{0x4c, "“"},
				{0x68, "’"},
				{0x69, "‘"},
				{0x19, "°"},
				{0x07, "！"},
				{0x08, "？"},
				{0x02, "¿"},
				{0x03, "¡"},
				{0x04, "b"},
				{0x0a, "←"},
				{0x0b, "↑"},
				{0x0c, "→"},
				{0x0d, "↓"},
				{0x48, "「"},
				{0x49, "」"},
				{0x1d, "／"},
				{0x32, "～"},
				{0x34, "＊"},
				{0x10, "※"},
				{0x33, "♪"},
				{0x51, "・"},
				{0x5b, "…"},
			};

			StringBuilder sb = new StringBuilder();
			Byte[] buffer = SaveData.Instance().ReadValue(address, 26);
			for (int index = 0; index < buffer.Length; index++)
			{
				if (buffer[index] == 0 || buffer[index] == 0xE3) break;
				switch (buffer[index])
				{
					case 0xE0:
						// 濁点・半濁点
						index++;
						if (e0.ContainsKey(buffer[index]))
						{
							sb.Append(e0[buffer[index]]);
						}
						break;

					case 0xE1:
						index++;
						if (e1.ContainsKey(buffer[index]))
						{
							sb.Append(e1[buffer[index]]);
						}
						break;

					case 0xE4:
						// 漢字.
						// unicode.
						Byte[] ch = new Byte[2];
						ch[0] = buffer[index + 2];
						ch[1] = buffer[index + 1];
						sb.Append(System.Text.Encoding.Unicode.GetString(ch));
						index += 2;
						break;

					default:
						if (normal.ContainsKey(buffer[index]))
						{
							sb.Append(normal[buffer[index]]);
						}
						break;
				}
			}

			return sb.ToString();
		}
	}
}

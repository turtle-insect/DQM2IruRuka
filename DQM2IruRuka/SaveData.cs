using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQM2IruRuka
{
	internal class SaveData
	{
		private static SaveData mThis;
		private String mFileName = null;
		private Byte[] mBuffer = null;
		private Byte[] mHeaderKey = { 0xBD, 0x0B, 0x31, 0x67, 0xB6, 0xF0, 0xAD, 0xA0, 0x70, 0xAC, 0xE5, 0x6D, 0xD1, 0xC8, 0xAF, 0x99, };
		private Byte[] mBodyKey = { 0xF6, 0xDB, 0xBA, 0x24, 0x3C, 0x35, 0xD9, 0x46, 0xD7, 0x4E, 0x40, 0x1E, 0x41, 0x9A, 0x48, 0x05, };
		private readonly System.Text.Encoding mEncode = System.Text.Encoding.UTF8;
		public uint Adventure { private get; set; } = 0;

		private SaveData()
		{ }

		public static SaveData Instance()
		{
			if (mThis == null) mThis = new SaveData();
			return mThis;
		}

		public bool Open(String filename, bool force)
		{
			if (System.IO.File.Exists(filename) == false) return false;

			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			Byte[] plaintext;
			Decrypt(buffer, mHeaderKey, 0, 0xB0, out plaintext);
			// headerとbodyのmac(tag)以外をmBufferに保存.
			// 別に元のnonceを利用しなくても問題ない
			// 念のため再利用する
			mBuffer = new Byte[buffer.Length - 32];
			int index = 0;
			Array.Copy(buffer, 0, mBuffer, index, 16);
			index += 16;
			Array.Copy(plaintext, 0, mBuffer, index, plaintext.Length);
			index += plaintext.Length;

			Decrypt(buffer, mBodyKey, 0xD0, 0x2F568, out plaintext);
			Array.Copy(buffer, 0xD0, mBuffer, index, 16);
			index += 16;
			Array.Copy(plaintext, 0, mBuffer, index, plaintext.Length);

			mFileName = filename;
			Backup();
			return true;
		}

		public bool Save()
		{
			if (mFileName == null || mBuffer == null) return false;

			Byte[] buffer = new Byte[mBuffer.Length + 32];
			Byte[] ciphertext;
			Byte[] tag;
			int index = 0;
			Array.Copy(mBuffer, 0, buffer, index, 16);
			index += 16;
			Encrypt(mHeaderKey, 0, 0xB0, out tag, out ciphertext);
			Array.Copy(tag, 0, buffer, index, tag.Length);
			index += tag.Length;
			Array.Copy(ciphertext, 0, buffer, index, ciphertext.Length);
			index += ciphertext.Length;
			Array.Copy(mBuffer, 0xC0, buffer, index, 16);
			index += 16;
			Encrypt(mBodyKey, 0xC0, 0x2F568, out tag, out ciphertext);
			Array.Copy(tag, 0, buffer, index, tag.Length);
			index += tag.Length;
			Array.Copy(ciphertext, 0, buffer, index, ciphertext.Length);

			System.IO.File.WriteAllBytes(mFileName, buffer);
			return true;
		}

		public bool SaveAs(String filename)
		{
			if (mFileName == null || mBuffer == null) return false;
			mFileName = filename;
			return Save();
		}

		public void Import(String filename)
		{
			if (mFileName == null) return;

			mBuffer = System.IO.File.ReadAllBytes(filename);
		}

		public void Export(String filename)
		{
			System.IO.File.WriteAllBytes(filename, mBuffer);
		}

		public uint ReadNumber(uint address, uint size)
		{
			if (mBuffer == null) return 0;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return 0;
			uint result = 0;
			for (int i = 0; i < size; i++)
			{
				result += (uint)(mBuffer[address + i]) << (i * 8);
			}
			return result;
		}

		public Byte[] ReadValue(uint address, uint size)
		{
			Byte[] result = new Byte[size];
			if (mBuffer == null) return result;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return result;
			for (int i = 0; i < size; i++)
			{
				result[i] = mBuffer[address + i];
			}
			return result;
		}

		public Byte ReadByte(uint address, bool isLow)
		{
			Byte result = 0;
			if (mBuffer == null) return result;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return result;
			result = mBuffer[address];
			if (isLow == false)
			{
				result = (Byte)(result >> 4);
			}
			result &= 0x0F;

			return result;
		}

		// 0 to 7.
		public bool ReadBit(uint address, uint bit)
		{
			if (bit < 0) return false;
			if (bit > 7) return false;
			if (mBuffer == null) return false;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return false;
			Byte mask = (Byte)(1 << (int)bit);
			Byte result = (Byte)(mBuffer[address] & mask);
			return result != 0;
		}

		public String ReadText(uint address, uint size)
		{
			if (mBuffer == null) return "";
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return "";

			Byte[] tmp = new Byte[size];
			for (uint i = 0; i < size; i++)
			{
				if (mBuffer[address + i] == 0) break;
				tmp[i] = mBuffer[address + i];
			}
			return mEncode.GetString(tmp).Trim('\0');
		}

		public void WriteNumber(uint address, uint size, uint value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = (Byte)(value & 0xFF);
				value >>= 8;
			}
		}

		public void WriteByte(uint address, bool isLow, Byte value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return;
			Byte tmp = mBuffer[address];
			if (isLow == false)
			{
				tmp &= 0x0F;
				value = (Byte)(value << 4);
			}
			else
			{
				tmp &= 0xF0;
			}
			mBuffer[address] = (Byte)(tmp | value);
		}

		// 0 to 7.
		public void WriteBit(uint address, uint bit, bool value)
		{
			if (bit < 0) return;
			if (bit > 7) return;
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return;
			Byte mask = (Byte)(1 << (int)bit);
			if (value) mBuffer[address] = (Byte)(mBuffer[address] | mask);
			else mBuffer[address] = (Byte)(mBuffer[address] & ~mask);
		}

		public void WriteText(uint address, uint size, String value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			Byte[] tmp = mEncode.GetBytes(value);
			Array.Resize(ref tmp, (int)size);
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = tmp[i];
			}
		}

		public void WriteValue(uint address, Byte[] buffer)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + buffer.Length > mBuffer.Length) return;

			for (uint i = 0; i < buffer.Length; i++)
			{
				mBuffer[address + i] = buffer[i];
			}
		}

		public void Fill(uint address, uint size, Byte number)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = number;
			}
		}

		public void Copy(uint from, uint to, uint size)
		{
			if (mBuffer == null) return;
			from = CalcAddress(from);
			to = CalcAddress(to);
			if (from + size > mBuffer.Length) return;
			if (to + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[to + i] = mBuffer[from + i];
			}
		}

		public void Swap(uint from, uint to, uint size)
		{
			if (mBuffer == null) return;
			from = CalcAddress(from);
			to = CalcAddress(to);
			if (from + size > mBuffer.Length) return;
			if (to + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				Byte tmp = mBuffer[to + i];
				mBuffer[to + i] = mBuffer[from + i];
				mBuffer[from + i] = tmp;
			}
		}

		public List<uint> FindAddress(String name, uint index)
		{
			List<uint> result = new List<uint>();
			if (mBuffer == null) return result;

			for (; index < mBuffer.Length; index++)
			{
				if (mBuffer[index] != name[0]) continue;

				int len = 1;
				for (; len < name.Length; len++)
				{
					if (mBuffer[index + len] != name[len]) break;
				}
				if (len >= name.Length) result.Add(index);
				index += (uint)len;
			}
			return result;
		}

		private void Decrypt(Byte[] buffer, Byte[] key, uint address, uint size, out Byte[] plaintext)
		{
			plaintext = new Byte[size];
			Byte[] ciphertext = new Byte[size];
			if (buffer == null) return;
			if(buffer.Length < address + size) return;

			Byte[] nonce = new Byte[12];
			Array.Copy(buffer, address, nonce, 0, nonce.Length);
			Byte[] tag = new Byte[16];
			Array.Copy(buffer, address + 16, tag, 0, tag.Length);
			
			Array.Copy(buffer, address + 32, ciphertext, 0, ciphertext.Length);

			var aesccm = new System.Security.Cryptography.AesCcm(key);
			aesccm.Decrypt(nonce, ciphertext, tag, plaintext);
		}

		private void Encrypt(Byte[] key, uint address, uint size, out Byte[] tag, out Byte[] ciphertext)
		{
			tag = new Byte[16];
			ciphertext = new Byte[size];
			if (mBuffer == null) return;

			Byte[] nonce = new Byte[12];
			Array.Copy(mBuffer, address, nonce, 0, nonce.Length);
			Byte[] plaintext = new Byte[size];
			Array.Copy(mBuffer, address + 16, plaintext, 0, plaintext.Length);

			var aesccm = new System.Security.Cryptography.AesCcm(key);
			aesccm.Encrypt(nonce, plaintext, ciphertext, tag);
		}

		private uint CalcAddress(uint address)
		{
			return address + Adventure;
		}

		private void Backup()
		{
			DateTime now = DateTime.Now;
			String path = "backup";
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
			path = System.IO.Path.Combine(path, $"{now:yyyy-MM-dd HH-mm-ss} {System.IO.Path.GetFileName(mFileName)}");
			System.IO.File.Copy(mFileName, path, true);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace DQM2IruRuka
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			String[] files = e.Data.GetData(DataFormats.FileDrop) as String[];
			if (files == null) return;

			FileOpen(files[0], false);
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			FileOpen(false);
		}

		private void MenuItemFileForceOpen_Click(object sender, RoutedEventArgs e)
		{
			FileOpen(true);
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveData.Instance().Save();
		}

		private void MenuItemFileImport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Import(dlg.FileName);
		}

		private void MenuItemFileExport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Export(dlg.FileName);
		}

		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void TextBoxItemFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			var vm = DataContext as ViewModel;
			if (vm == null) return;
			vm.FilterItem();
		}

		private void ButtonChoiceMonsterType_Click(object sender, RoutedEventArgs e)
		{
			var number = (sender as Button)?.DataContext as Number;
			if (number == null) return;

			number.Value = Choice(ChoiceWindow.eType.eType, number.Value);
		}

		private void ButtonChoiceEggType_Click(object sender, RoutedEventArgs e)
		{
			var egg = (sender as Button)?.DataContext as Egg;
			if (egg == null) return;

			egg.Type = Choice(ChoiceWindow.eType.eType, egg.Type);
		}

		private void ButtonChoiceMonsterSkill_Click(object sender, RoutedEventArgs e)
		{
			var number = (sender as Button)?.DataContext as Number;
			if (number == null) return;

			number.Value = Choice(ChoiceWindow.eType.eSkill, number.Value);
		}

		private void ButtonChoiceWeaponID_Click(object sender, RoutedEventArgs e)
		{
			var weapon = (sender as Button)?.DataContext as Weapon;
			if (weapon == null) return;

			weapon.ID = Choice(ChoiceWindow.eType.eWeapon, weapon.ID);
		}

		private void ButtonChoiceWeaponOption1_Click(object sender, RoutedEventArgs e)
		{
			var weapon = (sender as Button)?.DataContext as Weapon;
			if (weapon == null) return;

			weapon.Option1 = Choice(ChoiceWindow.eType.eOption, weapon.Option1);
		}

		private void ButtonChoiceWeaponOption2_Click(object sender, RoutedEventArgs e)
		{
			var weapon = (sender as Button)?.DataContext as Weapon;
			if (weapon == null) return;

			weapon.Option2 = Choice(ChoiceWindow.eType.eOption, weapon.Option2);
		}

		private void ButtonChoiceWeaponOption3_Click(object sender, RoutedEventArgs e)
		{
			var weapon = (sender as Button)?.DataContext as Weapon;
			if (weapon == null) return;

			weapon.Option3 = Choice(ChoiceWindow.eType.eOption, weapon.Option3);
		}

		private void FileOpen(bool force)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;
			FileOpen(dlg.FileName, force);
		}

		private void FileOpen(String filename, bool force)
		{
			if (SaveData.Instance().Open(filename, force) == false)
			{
				MessageBox.Show("CheckSum Error", "File Open", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			DataContext = new ViewModel();
		}

		private uint Choice(ChoiceWindow.eType type, uint id)
		{
			var dlg = new ChoiceWindow();
			dlg.ID = id;
			dlg.Type = type;
			dlg.ShowDialog();
			return dlg.ID;
		}
	}
}

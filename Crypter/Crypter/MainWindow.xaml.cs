using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Crypter
{

	public partial class MainWindow : Window
	{
		#region initialization

		int z = 0;
		string selectedKey = null;

		#region Defining Keys

		private List<string> lmkPair = new List<string>
		{
			"01010101010101017902CD1FD36EF8BA",
			"20202020202020203131313131313131",
			"40404040404040405151515151515151",
			"61616161616161617070707070707070",
			"80808080808080809191919191919191",
			"A1A1A1A1A1A1A1A1B0B0B0B0B0B0B0B0",
			"C1C1010101010101D0D0010101010101",
			"E0E0010101010101F1F1010101010101",
			"1C587F1C13924FEF0101010101010101",
			"01010101010101010101010101010101",
			"02020202020202020404040404040404",
			"07070707070707071010101010101010",
			"13131313131313131515151515151515",
			"16161616161616161919191919191919",
			"1A1A1A1A1A1A1A1A1C1C1C1C1C1C1C1C",
			"23232323232323232525252525252525",
			"26262626262626262929292929292929",
			"2A2A2A2A2A2A2A2A2C2C2C2C2C2C2C2C",
			"2F2F2F2F2F2F2F2F3131313131313131",
			"01010101010101010101010101010101"
		};

		private string[,] lmkVariant =
		{
			{"A7", "86", "E6", "C7", "26", "07", "67", "46", "BA", "A7", "A4", "A1", "B5", "B0", "BC", "85", "80", "8C", "89", "A7"},
			{"5B", "7A", "1A", "3B", "DA", "FB", "9B", "BA", "46", "5B", "58", "5D", "49", "4C", "40", "79", "7C", "70", "75", "5B"},
			{"6B", "4A", "2A", "0B", "EA", "CB", "AB", "8A", "76", "6B", "68", "6D", "79", "7C", "70", "49", "4C", "40", "45", "6B"},
			{"DF", "FE", "9E", "BF", "5E", "7F", "1F", "3E", "C2", "DF", "DC", "D9", "CD", "C8", "C4", "FD", "F8", "F4", "F1", "DF"},
			{"2A", "0B", "6B", "4A", "AB", "8A", "EA", "CB", "37", "2A", "29", "2C", "38", "3D", "31", "08", "0D", "01", "04", "2A"},
			{"51", "70", "10", "31", "D0", "F1", "91", "B0", "4C", "51", "52", "57", "43", "46", "4A", "73", "76", "7A", "7F", "51"},
			{"75", "54", "34", "15", "F4", "D5", "B5", "94", "68", "75", "76", "73", "67", "62", "6E", "57", "52", "5E", "5B", "75"},
			{"9D", "BC", "DC", "FD", "1C", "3D", "5D", "7C", "80", "9D", "9E", "9B", "8F", "8A", "86", "BF", "BA", "B6", "B3", "9D"}
		};

		#endregion

		public MainWindow()
		{
			InitializeComponent();

			#region ComboBoxFill

			for (int i = 0; i < lmkPair.Count; i++)
				comboBoxPair.Items.Add(z++ + "-" + z++);

			for (int i = 0; i < 9;)
				comboBoxVariant.Items.Add(i++);

			#endregion
		}
		#endregion



		#region Button handlers

		private void btnEncrypt_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(textBoxInput.Text))
			{
				MessageBox.Show("Please enter the text you want to encrypt");
			}
			else if (string.IsNullOrEmpty(selectedKey))
			{
				MessageBox.Show("Please select DES Key");
			}
			else if (!CryptoEngine.OnlyHexInString(textBoxInput.Text))
			{
				MessageBox.Show("Input is not hex");
			}
			else if (textBoxInput.Text.Length != 32)
			{
				MessageBox.Show("Input must be 16 Bytes long");
			}
			else
			{
				string clearText = textBoxInput.Text.Trim();

				byte[] result = CryptoEngine.Crypt(CryptoEngine.StringToByteArray(clearText), CryptoEngine.StringToByteArray(selectedKey));

				string hex = BitConverter.ToString(result);
				hex = hex.Replace("-", "");
				textBoxOutput.Text = hex;

				byte[] kcv = CryptoEngine.GetKCVDES(hex);
				if (kcv != null)
				{
					string kcvHex = BitConverter.ToString(kcv);
					kcvHex = kcvHex.Replace("-", "");

					labelKCV.Content = kcvHex.Substring(0, 6);
				}

				if (!string.IsNullOrWhiteSpace(textBoxOutput.Text))
				{
					btnCopyOutput.IsEnabled = true;
					btnCopyKCV.IsEnabled = true;
				}
				else btnCopyOutput.IsEnabled = false;
			}

		}

		private void btnDecrypt_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(textBoxInput.Text))
			{
				MessageBox.Show("Please enter the text you want to decrypt");
			}
			else if (string.IsNullOrEmpty(selectedKey))
			{
				MessageBox.Show("Please select DES Key");
			}
			else if (!CryptoEngine.OnlyHexInString(textBoxInput.Text))
			{
				MessageBox.Show("Input is not hex");
			}
			else if (textBoxInput.Text.Length != 32)
			{
				MessageBox.Show("Input must be 16 Bytes long");
			}
			else
			{
				string cipherText = textBoxInput.Text.Trim();

				byte[] result = CryptoEngine.Decrypt(CryptoEngine.StringToByteArray(cipherText), CryptoEngine.StringToByteArray(selectedKey));

				string hex = BitConverter.ToString(result);
				hex = hex.Replace("-", "");
				textBoxOutput.Text = hex;

				byte[] kcv = CryptoEngine.GetKCVDES(hex);

				if (kcv != null)
				{
					string kcvHex = BitConverter.ToString(kcv);
					kcvHex = kcvHex.Replace("-", "");

					labelKCV.Content = kcvHex.Substring(0, 6);
				}

				if (!string.IsNullOrWhiteSpace(textBoxOutput.Text))
				{
					btnCopyOutput.IsEnabled = true;
					btnCopyKCV.IsEnabled = true;
				}
				else
				{
					btnCopyOutput.IsEnabled = false;
				}
			}
		}

		private void btnCopyOutput_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.Clear();
			Clipboard.SetText(textBoxOutput.Text);
		}

		private void buttonExpertMode_Click(object sender, RoutedEventArgs e)
		{
			comboBoxPair.Visibility = Visibility.Visible;
			comboBoxPair.IsEnabled = true;
			lblPair.Visibility = Visibility.Visible;

			comboBoxVariant.Visibility = Visibility.Visible;
			comboBoxVariant.IsEnabled = true;
			lblVariant.Visibility = Visibility.Visible;

			comboBoxPair.SelectedIndex = -1;
			comboBoxVariant.SelectedIndex = -1;
		}
		#endregion

		#region comboBox handlers

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (comboBoxKeyType.SelectedIndex > -1)
			{
				comboBoxPair.SelectedIndex = -1;
				comboBoxPair.IsEnabled = false;
				comboBoxVariant.SelectedIndex = -1;
				comboBoxVariant.IsEnabled = false;

				switch (comboBoxKeyType.SelectedIndex)
				{
					case 0:
						selectedKey = "BC" + lmkPair[14].Substring(2);
						break;
					case 1:
						selectedKey = "40" + lmkPair[14].Substring(2);
						break;
					case 2:
						selectedKey = "70" + lmkPair[14].Substring(2);
						break;
					case 3:
						selectedKey = "4A" + lmkPair[14].Substring(2);
						break;
					case 4:
						selectedKey = "6E" + lmkPair[14].Substring(2);
						break;
					case 5:
						selectedKey = lmkPair[3];
						break;
				}

				labelKEY.Content = selectedKey;
			}
		}

		private void comboBoxPair_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			comboBoxKeyType.SelectedIndex = -1;

			if (comboBoxPair.SelectedIndex > -1)
			{
				selectedKey = lmkPair[comboBoxPair.SelectedIndex];
				labelKEY.Content = selectedKey;
				comboBoxVariant.SelectedIndex = 0;
			}
		}

		private void comboBoxVariant_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			comboBoxKeyType.SelectedIndex = -1;

			if (comboBoxVariant.SelectedIndex == 0 && comboBoxPair.SelectedIndex > -1)
			{
				selectedKey = lmkPair[comboBoxPair.SelectedIndex];
				labelKEY.Content = selectedKey;
			}
			else if (comboBoxVariant.SelectedIndex > 0 && comboBoxPair.SelectedIndex > -1)
			{
				selectedKey = lmkVariant[comboBoxVariant.SelectedIndex - 1, comboBoxPair.SelectedIndex] + selectedKey.Substring(2);
				labelKEY.Content = selectedKey;
			}
		}

		#endregion

		private void btnKCV_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.Clear();
			Clipboard.SetText(labelKCV.Content.ToString());
		}

		private void textBoxInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			int hexNumber;
			e.Handled = !int.TryParse(e.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out hexNumber);
		}
	}
}

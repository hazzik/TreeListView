using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace TestApp
{
	internal class RegImageConverter : IValueConverter
	{
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			if (o is RegValue)
			{
				if (((RegValue)o).Kind == Microsoft.Win32.RegistryValueKind.String)
					return "/Images/dataString.png";
				else
					return "/Images/data.png";
			}
			else
				return "/Images/folder.png";
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

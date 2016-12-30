using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApp
{
	/// <summary>
	/// Interaction logic for RegistrySample.xaml
	/// </summary>
	public partial class RegistrySample : UserControl
	{
        RegistryModel model;
        public RegistrySample()
		{
            model = new RegistryModel();
            model.keys.Add(Registry.ClassesRoot);
            InitializeComponent();
			_tree.Model = this.model;
		}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            model.keys.Add(Registry.CurrentUser);
            model.keys.Add(Registry.LocalMachine);
            model.keys.Add(Registry.Users);
            model.keys.Add(Registry.CurrentConfig);
            _tree.Model = this.model;
        }
    }
}

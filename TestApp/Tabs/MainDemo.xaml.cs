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
using Aga.Controls.Tree;

namespace TestApp
{
	/// <summary>
	/// Interaction logic for MainDemo.xaml
	/// </summary>
	public partial class MainDemo : UserControl
	{
		public MainDemo()
		{
			InitializeComponent();

			LoadModel(3,5,5);
		}

		private void LoadModel(int c1, int c2, int c3)
		{
			var model = PersonModel.CreateTestModel(c1, c2, c3);
			_treeList.Model = model;
			_treeView.ItemsSource = model.Root.Children;
		}

		private void Small_Click(object sender, RoutedEventArgs e)
		{
			LoadModel(3, 5, 5);
		}

		private void Big_Click(object sender, RoutedEventArgs e)
		{
			LoadModel(3, 5000, 0);
		}

		private void Toggle_Click(object sender, RoutedEventArgs e)
		{
			foreach(var node in _treeList.SelectedNodes)
				if (node.IsExpandable)
					node.IsExpanded = !node.IsExpanded;
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			if (_treeList.SelectedNode != null)
			{
				var p = new Person() { Name = "NewPerson" };
				(_treeList.SelectedNode.Tag as Person).Children.Add(p);
				_treeList.SelectedNode.IsExpanded = true;
			}
		}

		private void Remove_Click(object sender, RoutedEventArgs e)
		{
			if (_treeList.SelectedNode != null)
			{
				var parent = _treeList.SelectedNode.Parent.Tag as Person;
				var child = _treeList.SelectedNode.Tag as Person;
				parent.Children.Remove(child);
			}
		}
	}
}

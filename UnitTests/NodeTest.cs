using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aga.Controls.Tree;

namespace UnitTests
{
	[TestClass]
	public class NodeTest
	{

		[TestMethod]
		public void VisibleChildren()
		{
			TreeNode root = CreateTreeNode(3, 3);
			root.IsExpanded = true;
			var a = root.Children[0];
			a.IsExpanded = true;
			var b = root.Children[0].Children[2];
			b.IsExpanded = true;

			Assert.AreEqual(6, a.VisibleChildrenCount);
			Assert.AreEqual(3, b.VisibleChildrenCount);
		}

		private static TreeNode CreateTreeNode(int depth, int count)
		{
			TreeNode root = new TreeNode(null, null);
			if (depth > 0)
			{
				for (int i = 0; i < count; i++)
				{
					root.Children.Add(CreateTreeNode(depth - 1, count));
				}
			}
			return root;
		}
	}
}

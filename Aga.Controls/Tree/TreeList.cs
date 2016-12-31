using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace Aga.Controls.Tree
{
	public class TreeList: ListView
	{
		#region Properties

		/// <summary>
		/// Internal collection of rows representing visible nodes, actually displayed in the ListView
		/// </summary>
		internal ObservableCollectionAdv<TreeNode> Rows
		{
			get;
			private set;
		} 


		private ITreeModel _model;
		public ITreeModel Model
		{
		  get { return _model; }
		  set
            {
                if (_model != value)
                {
                    _model = value;
                    _root.Children.Clear();
                    Rows.Clear();
                }
                else//How to update?
                {
                    ClearRemovedNodes(_root);
                    Rows.RefeshData();
                }
                CreateChildrenNodes(_root);
            }
		}

        void ClearRemovedNodes(TreeNode node)
        {
            if (node != null && node.Children.Count > 0)
            {
                IEnumerable child = GetChildren(node);
                if (IEnumerableIsNullOrEmpty(child))
                {
                    node.Children.Clear();
                    return;
                }

                for (int i = 0; i < node.Children.Count; i++)
                {
                    bool flag = true;
                    foreach (object obj in child)
                    {
                        if (node.Children[i].Tag == obj) { flag = false; break; }
                    }

                    if (flag)
                    {
                        ClearNodeAndChildInRows(node.Children[i]);
                        node.Children.RemoveAt(i);
                        i--;
                    }
                    else ClearRemovedNodes(node.Children[i]);
                }
            }
        }

        static bool IEnumerableIsNullOrEmpty(IEnumerable source)
        {
            if (source != null)
            {
                foreach (object obj in source)
                {
                    return false;
                }
            }
            return true;
        }
        void ClearNodeAndChildInRows(TreeNode node)
        {
            if (node.HasChildren) for (int i = 0; i < node.Children.Count; i++)
                {
                    ClearNodeAndChildInRows(node.Children[i]);
                }
            Rows.Remove(node);
        }




        private TreeNode _root;
		internal TreeNode Root
		{
			get { return _root; }
		}

		public ReadOnlyCollection<TreeNode> Nodes
		{
			get { return Root.Nodes; }
		}

		internal TreeNode PendingFocusNode
		{
			get;
			set;
		}

		public ICollection<TreeNode> SelectedNodes
		{
			get
			{
				return SelectedItems.Cast<TreeNode>().ToArray();
			}
		}

		public TreeNode SelectedNode
		{
			get
			{
				if (SelectedItems.Count > 0)
					return SelectedItems[0] as TreeNode;
				else
					return null;
			}
		}
		#endregion

		public TreeList()
		{
			Rows = new ObservableCollectionAdv<TreeNode>();
			_root = new TreeNode(this, null);
			_root.IsExpanded = true;
			ItemsSource = Rows;
			ItemContainerGenerator.StatusChanged += ItemContainerGeneratorStatusChanged;
		}

		void ItemContainerGeneratorStatusChanged(object sender, EventArgs e)
		{
			if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated && PendingFocusNode != null)
			{
				var item = ItemContainerGenerator.ContainerFromItem(PendingFocusNode) as TreeListItem;
				if (item != null)
					item.Focus();
				PendingFocusNode = null;
			}
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeListItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeListItem;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			var ti = element as TreeListItem;
			var node = item as TreeNode;
			if (ti != null && node != null)
			{
				ti.Node = item as TreeNode;
				base.PrepareContainerForItemOverride(element, node.Tag);
			}
		}

		internal void SetIsExpanded(TreeNode node, bool value)
		{
			if (value)
			{
				if (!node.IsExpandedOnce)
				{
					node.IsExpandedOnce = true;
					node.AssignIsExpanded(value);
					CreateChildrenNodes(node);
				}
				else
				{
					node.AssignIsExpanded(value);
					CreateChildrenRows(node);
				}
			}
			else
			{
				DropChildrenRows(node, false);
				node.AssignIsExpanded(value);
			}
		}

		internal void CreateChildrenNodes(TreeNode node)
		{
            var childrens = GetChildren(node);
            if (childrens != null)
            {
                int rowIndex = Rows.IndexOf(node);
                node.ChildrenSource = childrens as INotifyCollectionChanged;
                Collection<TreeNode> newnode = new Collection<TreeNode>();
                foreach (object obj in childrens)
                {
                    TreeNode child = new TreeNode(this, obj);
                    child.HasChildren = HasChildren(child);
                    bool flag_new = true;
                    foreach (TreeNode n in node.Children)
                    {
                        if (n.Tag == obj)
                        {
                            flag_new = false;
                            break;
                        }
                    }
                    if (flag_new)
                    {
                        node.Children.Add(child);
                        newnode.Add(child);
                    }
                }
                if (newnode.Count > 0) Rows.InsertRange(rowIndex + 1, newnode.ToArray());
            }
        }

		private void CreateChildrenRows(TreeNode node)
		{
			int index = Rows.IndexOf(node);
			if (index >= 0 || node == _root) // ignore invisible nodes
			{
				var nodes = node.AllVisibleChildren.ToArray();
				Rows.InsertRange(index + 1, nodes);
			}
		}

		internal void DropChildrenRows(TreeNode node, bool removeParent)
		{
			int start = Rows.IndexOf(node);
			if (start >= 0 || node == _root) // ignore invisible nodes
			{
				int count = node.VisibleChildrenCount;
				if (removeParent)
					count++;
				else
					start++;
				Rows.RemoveRange(start, count);
			}
		}

		private IEnumerable GetChildren(TreeNode parent)
		{
			if (Model != null)
				return Model.GetChildren(parent.Tag);
			else
				return null;
		}

		private bool HasChildren(TreeNode parent)
		{
			if (parent == Root)
				return true;
			else if (Model != null)
				return Model.HasChildren(parent.Tag);
			else
				return false;
		}

		internal void InsertNewNode(TreeNode parent, object tag, int rowIndex, int index)
		{
			TreeNode node = new TreeNode(this, tag);
			if (index >= 0 && index < parent.Children.Count)
				parent.Children.Insert(index, node);
			else
			{
				index = parent.Children.Count;
				parent.Children.Add(node);
			}
			Rows.Insert(rowIndex + index + 1, node);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aga.Controls.Tree;

namespace TestApp
{
	public class PersonModel : ITreeModel
	{
		public static PersonModel CreateTestModel(int count1, int count2, int count3)
		{
			var model = new PersonModel();
			for (int i = 0; i < count1; i++)
			{
				var p = new Person() { Name = "Person A " + i.ToString() };
				model.Root.Children.Add(p);
				for (int n = 0; n < count2; n++)
				{
					var p2 = new Person() { Name = "Person B" + n.ToString() };
					p.Children.Add(p2);
					for (int k = 0; k < count3; k++)
						p2.Children.Add(new Person() { Name = "Person C" + k.ToString() });
				}
			}
			return model;
		}

		public Person Root { get; private set; }

		public PersonModel()
		{
			Root = new Person();
		}

		public System.Collections.IEnumerable GetChildren(object parent)
		{
			if (parent == null)
				parent = Root;
			return (parent as Person).Children;
		}

		public bool HasChildren(object parent)
		{
			return (parent as Person).Children.Count > 0;
		}
	}
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace TestApp
{
	public class Person
	{
		private readonly ObservableCollection<Person> _children = new ObservableCollection<Person>();
		public ObservableCollection<Person> Children
		{
			get { return _children; }
		}

		public string Name { get; set; }

		public int Id { get; set; }

		static int _i;
		public Person()
		{
			Id = ++_i;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
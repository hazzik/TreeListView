using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace TestApp
{
    public class TreeData 
    {
		private readonly ObservableCollection<TreeData> _children = new ObservableCollection<TreeData>();
		public ObservableCollection<TreeData> Children
        {
            get 
			{
				return _children; 
			}
        }

		public int Id { get; set; }

		static int _i;
		public TreeData()
		{
			Id = ++_i;
		}

		public override string ToString()
		{
			return Id.ToString();
		}
	}
}
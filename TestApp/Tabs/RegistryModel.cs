using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aga.Controls.Tree;
using Microsoft.Win32;
using System.Collections;

namespace TestApp
{
	class RegistryModel : ITreeModel
	{
		public IEnumerable GetChildren(object parent)
		{
			var key = parent as RegistryKey;
			if (parent == null)
			{
				yield return Registry.ClassesRoot;
				yield return Registry.CurrentUser;
				yield return Registry.LocalMachine;
				yield return Registry.Users;
				yield return Registry.CurrentConfig;
			}
			else if (key != null)
			{
				foreach (var name in key.GetSubKeyNames())
				{
					RegistryKey subKey = null;
					try
					{
						subKey = key.OpenSubKey(name);
					}
					catch
					{
					}
					if (subKey != null)
						yield return subKey;
				}

				foreach (var name in key.GetValueNames())
				{
					yield return new RegValue()
					{
						Name = name,
						Data = key.GetValue(name),
						Kind = key.GetValueKind(name)
					};
				}
			}
		}

		public bool HasChildren(object parent)
		{
			return parent is RegistryKey;
		}
	}

	public struct RegValue
	{
		public string Name { get; set; }
		public object Data { get; set; }
		public RegistryValueKind Kind { get; set; }
	}
}

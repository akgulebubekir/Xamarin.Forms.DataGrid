using System;
using System.Linq;
using System.Reflection;

namespace Xamarin.Forms.DataGrid.Utils
{
	public static class ReflectionUtils
	{
		private const char IndexBeginOp = '[';
		private const char IndexEndOp = ']';
		private const char PropertyOfOp = '.';

		public static object GetValueByPath(object obj, string path)
		{
			object result = obj;
			var tokens = path?.Split(IndexBeginOp, PropertyOfOp).ToList();
			foreach (var token in tokens)
			{
				if (result == null)
					break;

				//  Property
				if (!token.Contains(IndexEndOp.ToString()))
				{
					result = GetPropertyValue(result, token);
				}
				// Index
				else
				{
					result = GetIndexValue(result, token.Replace(IndexEndOp.ToString(), ""));
				}
			}
			return result;
		}

		private static object GetPropertyValue(object obj, string propertyName)
		{
			try
			{
				return obj?.GetType().GetRuntimeProperty(propertyName)?.GetValue(obj);
			}
			catch
			{
				return null;
			}
		}

		private static object GetIndexValue(object obj, string index)
		{
			object result = null;
			var indexOperator = obj?.GetType().GetRuntimeProperty("Item");
			if (indexOperator != null)
			{
				var indexParameters = indexOperator.GetIndexParameters();
				// Looking up suitable index operator
				foreach (var parameter in indexParameters)
				{
					bool isIndexOpWorked = true;
					try
					{
						var indexVal = Convert.ChangeType(index, parameter.ParameterType);
						result = indexOperator.GetValue(obj, new object[] { indexVal });
					}
					catch
					{
						isIndexOpWorked = false;
					}

					// If the index operator worked, skip looking up others
					if (isIndexOpWorked)
						break;
				}
			}
			return result;
		}
	}
}

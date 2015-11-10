using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class ReflectionUtil
	{
		public static bool HasAttribute(this Type tested, Type attribute)
		{
			return tested.GetCustomAttributes(attribute, false).Length > 0;
		}
		public static bool HasAttribute<TAtt>(this Type tested)
		{
			return tested.HasAttribute(typeof(TAtt));
		}

		public static bool HasAttribute(this MethodInfo method, Type attribute)
		{
			return method.GetCustomAttributes(attribute).Count() > 0;
		}
		public static bool HasAttribute<TAtt>(this MethodInfo method)
		{
			return method.HasAttribute(typeof(TAtt));
		}

		public static Action<T> CreateDelegate<T>(object target, MethodInfo method)
		{
			Action<T> del = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), target, method);

			return del;
		}
		public static object CreateDelegateGeneric(Type genericType, object target, MethodInfo method)
		{

			MethodInfo createDelegate = typeof(ReflectionUtil).GetMethod(
				nameof(CreateDelegate)).MakeGenericMethod(genericType);

			object del = createDelegate.Invoke(null, new object[] { target, method });

			return del;
		}

		public static object InvokeMethod(this object obj, string methodName, params object[] parameters)
		{
			Type t = obj.GetType();
			MethodInfo method = t.GetMethod(methodName);
			if (method == null)
			{
				throw new ArgumentException("Method not found: " + methodName);
			}

			return method.Invoke(obj, parameters);
		}

		public static object InvokeGeneric(this MethodInfo method, object instance, 
			Type[] typeParams, object[] parameters)
		{
			MethodInfo generic = method.MakeGenericMethod(typeParams);
			return generic.Invoke(instance, parameters);
		}

		public static List<Type> TypesInNamespace(Assembly assembly, string ns)
		{
			List<Type> res = new List<Type>();
			foreach (Type t in assembly.GetTypes())
			{
				if (t.Namespace == ns)
				{
					res.Add(t);
				}
			}

			return res;
		}

		public static bool InheritsFrom(this Type inheriting, Type inherited)
		{
			return inherited.IsAssignableFrom(inheriting);
		}
	}
}

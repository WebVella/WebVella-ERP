using System;
using System.Reflection;
using System.Reflection.Emit;

namespace WebVella.Erp.Utilities.Dynamic
{
	public class DynamicObjectCreater
	{
		delegate object MethodInvoker();
		MethodInvoker methodHandler = null;

		public DynamicObjectCreater(Type type)
		{
			CreateMethod(type.GetConstructor(Type.EmptyTypes));
		}

		public DynamicObjectCreater(ConstructorInfo target)
		{
			CreateMethod(target);
		}

		void CreateMethod(ConstructorInfo target)
		{
			DynamicMethod dynamic = new DynamicMethod(string.Empty,
						typeof(object),
						new Type[0],
						target.DeclaringType);
			ILGenerator il = dynamic.GetILGenerator();
			il.DeclareLocal(target.DeclaringType);
			il.Emit(OpCodes.Newobj, target);
			il.Emit(OpCodes.Stloc_0);
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ret);

			methodHandler = (MethodInvoker)dynamic.CreateDelegate(typeof(MethodInvoker));
		}

		public object CreateInstance()
		{
			return methodHandler();
		}
	}

}

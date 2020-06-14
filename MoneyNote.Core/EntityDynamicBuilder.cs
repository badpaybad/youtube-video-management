using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;

namespace MoneyNote.Core
{
    public class EntityDynamicBuilder<T>
    {
        private static readonly MethodInfo getValueMethod = typeof(DbDataReader).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(DbDataReader).GetMethod("IsDBNull", new Type[] { typeof(int) });
        private delegate T Load(DbDataReader dataRecord);

        private Load handler;

        private EntityDynamicBuilder() { }

        private static ConcurrentDictionary<Type, Delegate> _cachedMappers = new ConcurrentDictionary<Type, Delegate>();

        public T Build(DbDataReader dataRecord)
        {
            return handler(dataRecord);
        }

        public static EntityDynamicBuilder<T> CreateBuilder(DbDataReader dataRecord)
        {
            Type typeOfObject = typeof(T);
            EntityDynamicBuilder<T> dynamicBuilder = new EntityDynamicBuilder<T>();

            if (_cachedMappers.ContainsKey(typeOfObject))
            {
                dynamicBuilder.handler = (Load)_cachedMappers[typeOfObject];
                return dynamicBuilder;
            }

            DynamicMethod method = new DynamicMethod("DynamicCreate", typeOfObject, new Type[] { typeof(DbDataReader) }, typeOfObject, true);
            ILGenerator generator = method.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(typeOfObject);
            generator.Emit(OpCodes.Newobj, typeOfObject.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                string propName = dataRecord.GetName(i);
                PropertyInfo propertyInfo = typeOfObject.GetProperty(propName);
                //var propertyInfo = typeOfObject.GetProperty(ConvertLowerUnderscoreNamingToPascalNaming(dataRecord.GetName(i)));

                Label endIfLabel = generator.DefineLabel();

                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);

                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());

                    generator.MarkLabel(endIfLabel);
                }
            }

            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            var delegateMethod = method.CreateDelegate(typeof(Load));

            _cachedMappers.GetOrAdd(typeOfObject, delegateMethod);

            dynamicBuilder.handler = (Load)delegateMethod;
            return dynamicBuilder;
        }
    }

}

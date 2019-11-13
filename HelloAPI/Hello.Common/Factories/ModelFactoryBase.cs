using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hello.Common.Extensions;

namespace Hello.Common.Factories
{    
    public abstract class ModelFactoryBase
        : IModelFactory
    {        
        private readonly ConcurrentDictionary<Type, Type> TypeConstructionMap;
       
        private readonly ConcurrentDictionary<Type, ConstructorInfo> PreferredConstructorForTypeMap;
        
        private readonly ISet<Type> TypeDomain;

        protected ModelFactoryBase(ISet<Type> typeDomain)
        {            
            TypeDomain = Validators.ThrowArgNullExcIfNull(typeDomain, nameof(typeDomain));
            
            if (TypeDomain.Any(x => x.IsAbstract || !x.IsClass))
            {
                throw new ArgumentOutOfRangeException(nameof(typeDomain), "No type is allowed to be a value-type, interface, or abstract type.");
            }
            
            TypeConstructionMap = new ConcurrentDictionary<Type, Type>();
            
            PreferredConstructorForTypeMap = new ConcurrentDictionary<Type, ConstructorInfo>();
        }
        
        public T Create<T>() where T : class
        {
            Type sourceInterfaceType = typeof(T);
            return Create(sourceInterfaceType) as T;
        }
        
        public object Create(Type objectType)
        {
            Validators.ThrowArgNullExcIfNull(objectType, nameof(objectType));
            
            if (!IsSourceTypeConstructable(objectType))
            {
                throw new ArgumentOutOfRangeException("Type '" + objectType.FullName + "' cannot be used to construct a concrete data model by the model factory.");
            }
            
            Type targetConcreteType = null;
            if (!TypeConstructionMap.TryGetValue(objectType, out targetConcreteType) || targetConcreteType == null)
            {                
                throw new ArgumentOutOfRangeException("No type found in search space of concrete types that implements type '" + objectType.Name + "'.");
            }
            
            object model = CreateInstanceOfType(targetConcreteType);
            return model;
        }

        private Type GetTargetTypeForSourceType(Type sourceInterfaceType)
        {
            Type targetConcreteType = null;
                        
            IEnumerable<Type> concreteTypes = TypeDomain.Where(x => sourceInterfaceType.IsAssignableFrom(x));

            if (concreteTypes != null && concreteTypes.Any())
            {                
                if (concreteTypes.Count() == 1)
                {                    
                    targetConcreteType = concreteTypes.First();
                }
                else
                {                    
                    IEnumerable<Type> candidateTypes = concreteTypes.Where(x => x.GetNonInheritedInterfaces().Contains(sourceInterfaceType));
                    if (candidateTypes.Any())
                    {                        
                        targetConcreteType = candidateTypes.First();
                    }
                    else
                    {                        
                        targetConcreteType = concreteTypes.First();
                    }
                }
            }
            
            return targetConcreteType;
        }

        private ConstructorInfo GetPreferredConstructorForType(Type objectType)
        {            
            ConstructorInfo[] cInfos = objectType.GetConstructors();
            IOrderedEnumerable<ConstructorInfo> constructors = cInfos.OrderByDescending(x => x.GetParameters().Length);
            
            foreach (ConstructorInfo cInfo in constructors)
            {
                bool useThisConstructor = true;
                
                foreach (ParameterInfo pInfo in cInfo.GetParameters())
                {
                    if (!IsSourceTypeConstructable(pInfo.ParameterType))
                    {                        
                        useThisConstructor = false;
                        break;
                    }
                }

                if (useThisConstructor)
                {                    
                    return cInfo;
                }
            }
           
            return null;
        }

        private object CreateInstanceOfType(Type objectType)
        {            
            ConstructorInfo cInfo;
            if (!PreferredConstructorForTypeMap.TryGetValue(objectType, out cInfo))
            {
                cInfo = GetPreferredConstructorForType(objectType);
                PreferredConstructorForTypeMap.TryAdd(objectType, cInfo);
            }
            
            if (cInfo == null)
            {                
                throw new ArgumentOutOfRangeException("No usable constructor found for type '" + objectType.Name + "'.");
            }
            
            List<object> parameterInstances = new List<object>();
            foreach (ParameterInfo pInfo in cInfo.GetParameters())
            {
                parameterInstances.Add(Create(pInfo.ParameterType));
            }
           
            return Activator.CreateInstance(objectType, parameterInstances.ToArray());
        }
        
        private bool IsSourceTypeConstructable(Type sourceType)
        {            
            if (!sourceType.IsInterface)
            {
                return false;
            }

            Type targetConcreteType = null;

            if (!TypeConstructionMap.TryGetValue(sourceType, out targetConcreteType))
            {
                targetConcreteType = GetTargetTypeForSourceType(sourceType);                
                TypeConstructionMap.TryAdd(sourceType, targetConcreteType);
            }

            return targetConcreteType != null;
        }

    }
}

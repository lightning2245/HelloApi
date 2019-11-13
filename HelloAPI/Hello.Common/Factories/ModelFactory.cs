using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hello.Common.Models;
using Hello.Common.Extensions;

namespace Hello.Common.Factories
{    
    public class ModelFactory
        : ModelFactoryBase, IModelFactory
    {
        private static readonly IModelFactory _Current = new ModelFactory(GetAssembly());
        
        public static IModelFactory Current
        {
            get
            {
                return _Current;
            }
        }
        
        private static Assembly GetAssembly()
        {            
            Type typeExemplar = typeof(Message);
            
            Assembly modelAssembly = Assembly.GetAssembly(typeExemplar);
            if (modelAssembly == null)
            {
                throw new TypeLoadException("Cannot load concrete data model assembly - example type: " + typeExemplar.FullName);
            }

            return modelAssembly;
        }
        
        private ModelFactory(Assembly concreteTypeAssembly)
            : base(GetTypeDomain(concreteTypeAssembly))
        { }

        private static ISet<Type> GetTypeDomain(Assembly searchSpace)
        {
            Validators.ThrowArgNullExcIfNull(searchSpace, nameof(searchSpace));

            return new HashSet<Type>(searchSpace.GetLoadableTypes().Where(x => x.IsClass && !x.IsAbstract));
        }

    }

}
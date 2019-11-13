using System;

namespace Hello.Common.Factories
{    
    public interface IModelFactory
    {        
        T Create<T>() where T : class;
        object Create(Type objectType);
    }
}

using Hello.Common;
using Hello.Common.Factories;

namespace Hello.Data
{
    public abstract class BaseRepository
    {        
        protected readonly IModelFactory _ModelFactory;
        
        protected BaseRepository(IModelFactory factory)
        {
            _ModelFactory = Validators.ThrowArgNullExcIfNull(factory, "factory");
        }
    }
}

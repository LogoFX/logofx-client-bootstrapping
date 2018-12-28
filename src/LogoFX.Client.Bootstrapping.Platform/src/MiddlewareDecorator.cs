using Solid.Extensibility;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    //TODO: Move to Solid.Extensibility|Bootstrapping - pay attention to dependency on IoC
    //Add Use<T> at the IExtensible level
    //Add IMiddleware interface as marker interface for easier automagical registration
    class MiddlewareDecorator<TExtensible, TMiddleware> : IMiddleware<TExtensible>
        where TExtensible : class, IExtensible<TExtensible>
        where TMiddleware : class, IMiddleware<TExtensible>
    {
        private readonly IDependencyResolver _resolver;
        private TMiddleware _instance;

        public MiddlewareDecorator(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public TExtensible Apply(TExtensible @object)
        {
            if (_instance == null)
            {
                _instance = _resolver.Resolve<TMiddleware>();
            }
            return _instance.Apply(@object);
        }
    }    
}

using Autofac;
using Czeum.Core.GameServices.BoardConverter;
using Czeum.Core.GameServices.BoardCreator;
using Czeum.Core.GameServices.MoveHandler;

namespace Czeum.Web.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterGame<TBoardCreator, TBoardConverter, TMoveHandler>(this ContainerBuilder builder)
            where TBoardCreator : IBoardCreator
            where TBoardConverter : IBoardConverter
            where TMoveHandler : IMoveHandler
        {
            builder.RegisterType<TBoardCreator>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterType<TBoardConverter>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterType<TMoveHandler>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return builder;
        }
    }
}

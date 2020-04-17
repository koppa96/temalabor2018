using Autofac;
using Czeum.Core.GameServices;
using Czeum.Core.GameServices.BoardConverter;
using Czeum.Core.GameServices.BoardCreator;
using Czeum.Core.GameServices.MoveHandler;
using Czeum.Core.GameServices.ServiceMappings;
using System.Linq;

namespace Czeum.Web.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterGame<TBoardCreator, TBoardConverter, TMoveHandler>(this ContainerBuilder builder, string displayName)
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

            var lobbyDataType = typeof(TBoardCreator).BaseType!
                .GetGenericArguments()
                .Single();

            var moveDataType = typeof(TMoveHandler).BaseType!
                .GetGenericArguments()
                .First();

            var moveResultType = typeof(TBoardConverter).BaseType!
                .GetGenericArguments()
                .Last();

            GameTypeMapping.Instance.RegisterServiceMapping(
                displayName,
                lobbyDataType,
                moveDataType,
                moveResultType);

            return builder;
        }
    }
}

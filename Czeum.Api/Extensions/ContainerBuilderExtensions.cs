using Autofac;
using Czeum.Abstractions.GameServices.BoardConverter;
using Czeum.Abstractions.GameServices.BoardCreator;
using Czeum.Abstractions.GameServices.MoveHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Api.Extensions
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

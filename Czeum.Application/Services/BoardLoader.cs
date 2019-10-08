using Czeum.DAL;
using Czeum.DAL.Extensions;
using System;
using System.Threading.Tasks;
using Czeum.Core.Domain;
using Czeum.Core.GameServices;

namespace Czeum.Application.Services
{
    public class BoardLoader<TBoard> : IBoardLoader<TBoard>
        where TBoard : SerializedBoard
    {
        private readonly CzeumContext context;

        public BoardLoader(CzeumContext context)
        {
            this.context = context;
        }

        public async Task<TBoard> LoadByMatchIdAsync(Guid matchId)
        {
            var serializedBoard = await context.Boards
                .CustomSingleAsync(b => b.MatchId == matchId,
                    "No board found for the given match id.");

            if (serializedBoard is TBoard board)
            {
                return board;
            } 
            else
            {
                throw new ArgumentException("Invalid game type");
            }
        }
    }
}

using Czeum.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czeum.Domain.Entities
{
    public class Match : EntityBase
    {
        public ApplicationUser Player1 { get; set; }
        public ApplicationUser Player2 { get; set; }

        public MatchState State { get; private set; }
        public SerializedBoard Board { get; set; }

        public List<StoredMessage> Messages { get; set; }

        public Match()
        {
            State = MatchState.Player1Moves;
        }

        public void NextTurn()
        {
            switch (State)
            {
                case MatchState.Player1Moves:
                    State = MatchState.Player2Moves;
                    return;
                case MatchState.Player2Moves:
                    State = MatchState.Player1Moves;
                    return;
                default:
                    throw new InvalidOperationException("The match already ended.");
            }
        }

        public void CurrentPlayerWon()
        {
            switch (State)
            {
                case MatchState.Player1Moves:
                    State = MatchState.Player1Won;
                    return;
                case MatchState.Player2Moves:
                    State = MatchState.Player2Won;
                    return;
                default:
                    throw new InvalidOperationException("The match already ended.");
            }
        }

        public void Draw()
        {
            State = MatchState.Draw;
        }
    }
}

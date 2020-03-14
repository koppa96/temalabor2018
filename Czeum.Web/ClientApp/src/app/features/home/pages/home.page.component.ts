import { Component, OnInit } from '@angular/core';
import { GameType, StatisticsDto } from '../../../shared/clients';

@Component({
  selector: 'app-home.page',
  templateUrl: './home.page.component.html',
  styleUrls: ['./home.page.component.scss']
})
export class HomePageComponent implements OnInit {
  mockStatistics: StatisticsDto = new StatisticsDto({
    playedGames: 152,
    wonGames: 101,
    favouriteGame: GameType.Chess,
    playedGamesOfFavourite: 43,
    wonGamesOfFavourite: 22,
    mostPlayedWithName: 'Gipsz Jakab'
  });

  constructor() { }

  ngOnInit() {
  }

}

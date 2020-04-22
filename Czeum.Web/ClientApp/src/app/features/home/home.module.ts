import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomePageComponent } from './pages/home.page.component';
import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { HomeComponent } from './components/home/home.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { AchivementsComponent } from './components/achivements/achivements.component';
import { QuickActionsComponent } from './components/quick-actions/quick-actions.component';
import { StatisticsService } from './services/statistics.service';

@NgModule({
  declarations: [
    HomePageComponent,
    HomeComponent,
    StatisticsComponent,
    AchivementsComponent,
    QuickActionsComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    SharedModule
  ],
  providers: [
    StatisticsService
  ]
})
export class HomeModule { }

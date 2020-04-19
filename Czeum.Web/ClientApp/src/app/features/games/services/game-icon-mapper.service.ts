import { Injectable } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-common-types';
import { faChessPawn, faCircle, faTimes } from '@fortawesome/free-solid-svg-icons';

@Injectable({
  providedIn: 'root'
})
export class GameIconMapperService {
  mapIcon(gameIdentifier: number): IconDefinition {
    switch (gameIdentifier) {
      case 1: return faChessPawn;
      case 0: return faCircle;
      default: return faTimes;
    }
  }
}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  username = 'Példa Pál';
  submenuOpen = false;

  constructor() { }

  ngOnInit() {
  }

  closeSubmenu() {
    this.submenuOpen = false;
  }

  toggleSubmenu() {
    this.submenuOpen = !this.submenuOpen;
  }
}

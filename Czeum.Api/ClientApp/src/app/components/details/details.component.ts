import {AfterViewInit, Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css']
})
export class DetailsComponent implements OnInit, AfterViewInit {
  @ViewChild('chess', { static: false }) chessTitle: ElementRef;
  @ViewChild('connect4', { static: false }) connect4Title: ElementRef;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.route.queryParams.subscribe(
      query => {
        if (query.scrollTo === 'Chess') {
          this.chessTitle.nativeElement.scrollIntoView();
        } else if (query.scrollTo === 'Connect4') {
          this.connect4Title.nativeElement.scrollIntoView();
        }
      }
    );
  }

}

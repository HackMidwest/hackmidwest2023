import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-penny-art-home',
  templateUrl: './penny-art-home.component.html',
  styleUrls: ['./penny-art-home.component.scss']
})
export class PennyArtHomeComponent implements OnInit {
  public images!: any[];
  public inspireimages!: any[];
  upload = false;

  responsiveOptions:any[] = [
  {
      breakpoint: '1024px',
      numVisible: 5
  },
  {
      breakpoint: '768px',
      numVisible: 3
  },
  {
      breakpoint: '560px',
      numVisible: 1
  }
];

  constructor() { }

  ngOnInit(): void {
    this.images = [];
    this.inspireimages = [];
    for (let i = 1; i < 10; i++) {
      this.images.push(`https://picsum.photos/${i}00`);
    }

    for (let i = 11; i < 20; i++) {
      this.inspireimages.push(`https://picsum.photos/${i}00`);
    }
    // this.images.push('https://picsum.photos/200');
    // this.images.push('https://picsum.photos/300');

    // this.inspireimages.push('https://picsum.photos/400');
    // this.inspireimages.push('https://picsum.photos/500');
  }

  onUpload(e: Event) {
    this.upload = true;
  }

}

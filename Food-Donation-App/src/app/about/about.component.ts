import { Component } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent {
  business:boolean = false;
  ngOnInit(): void {
    let storedBusiness = localStorage.getItem("business")
    this.business = storedBusiness != undefined ? JSON.parse(storedBusiness) : false;
  }
}

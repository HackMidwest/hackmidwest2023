import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit{

  
  constructor(private formBuilder:FormBuilder, private http:HttpClient, private appComponent:AppComponent) {}
  userName!: string;
  id!: number;


  ngOnInit(): void {
    this.userName = this.appComponent.username;
    this.id = this.appComponent.id;

    var getURL = "https://fooddonationapi.azurewebsites.net/ClaimedPosts/" + this.id;

    console.log(getURL);


    //this.http.get(getURL);
  }


  newsItems = [
    {
      category: 'Bakery',
      categoryColor: 'text-info',
      title: 'Baked Daily, Available for All',
      description: 'Our unsold baked goods are up for donation. Every bread and pastry nourishes someone in need',
      date: '09.24.2023',
      imageUrl: '/assets/logos/bagels_01.png',
      profileImage: '/assets/logos/bakery_logo.png',
      profileName: 'B.B Bakery',
    }
  ]

}

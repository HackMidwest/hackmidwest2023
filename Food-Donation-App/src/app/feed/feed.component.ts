import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { UnclaimedPostModel } from 'src/model';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})

export class FeedComponent implements OnInit {

  constructor(private http:HttpClient) {}

  getURL: string = "https://fooddonationapi.azurewebsites.net/UnclaimedPosts";

  showButton = false;

  ngOnInit(): void {
    //var val = JSON.parse(this.getUnclaimed()) as UnclaimedPostModel;
    //console.log(val);
  }


  getUnclaimed () : Object {
    
    var val = this.http.get(this.getURL);

    

    return val;
  }


  newsItems = [
    {
      id: 0,
      claimed: false,
      category: 'Bakery',
      categoryColor: 'text-info',
      title: 'Baked Daily, Available for All',
      description: 'Our unsold baked goods are up for donation. Every bread and pastry nourishes someone in need',
      date: '09.24.2023',
      imageUrl: '/assets/logos/bagels_01.png',
      profileImage: '/assets/logos/bakery_logo.png',
      profileName: 'B.B Bakery',
    },
    {
      id: 0,
      claimed: false,
      category: 'Deli',
      categoryColor: 'text-info',
      title: 'Gourmet Donations for the Community',
      description: 'We are offering our deli surpluses to those in need. Join us at DnD Deli in this noble cause',
      date: '09.22.2023',
      imageUrl: '/assets/logos/deli_02.png',
      profileImage: '/assets/logos/deli_logo.png',
      profileName: 'DnD Deli',
    },
    {
      id: 0,
      claimed: false,
      category: 'Vegetarian',
      categoryColor: 'text-info',
      title: 'From our Farms, with Love',
      description: 'Our excess vegetables are fresh, organic, and ready for donation. Together, lets reduce hunger',
      date: '09.20.2023',
      imageUrl: '/assets/logos/vegetables_01.png',
      profileImage: '/assets/logos/vegetable_logo.png',
      profileName: 'The Greenhouse',
    },
    {
      id: 0,
      claimed: false,
      category: 'BBQ',
      categoryColor: 'text-info',
      title: 'Grilled with Care, Shared with Love',
      description: 'We at BB Smokehouse are donating our BBQ leftovers. A little flavor can make a big difference',
      date: '09.18.2023',
      imageUrl: '/assets/logos/bbq_sandwiches_02.png',
      profileImage: '/assets/logos/bbq_logo_2.png',
      profileName: 'BB Smokehouse',
    },
    // ... other news items
  ];
  
  lastAssignedId: number = 0;

  claimItem(item: any): void {
    item.claimed = true;
    this.lastAssignedId++;
    item.id = this.lastAssignedId;
  }

}

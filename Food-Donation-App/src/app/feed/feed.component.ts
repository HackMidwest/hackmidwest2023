import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})

export class FeedComponent {
  showButton = false;

  newsItems = [
    {
      category: 'Bakery',
      categoryIcon: 'fas fa-plane',
      categoryColor: 'text-info',
      title: 'Baked Daily, Available for All',
      description: 'Our unsold baked goods are up for donation. Every bread and pastry nourishes someone in need',
      date: '09.24.2023',
      imageUrl: '/assets/logos/bagels_01.png',
      profileImage: '/assets/logos/bakery_logo.png',
      profileName: 'B.B Bakery',
    },
    {
      category: 'Deli',
      categoryIcon: 'fas fa-plane',
      categoryColor: 'text-info',
      title: 'Gourmet Donations for the Community',
      description: 'We are offering our deli surpluses to those in need. Join us at DnD Deli in this noble cause',
      date: '09.22.2023',
      imageUrl: '/assets/logos/deli_02.png',
      profileImage: '/assets/logos/deli_logo.png',
      profileName: 'DnD Deli',
    },
    {
      category: 'Vegetarian',
      categoryIcon: 'fas fa-plane',
      categoryColor: 'text-info',
      title: 'From our Farms, with Love',
      description: 'Our excess vegetables are fresh, organic, and ready for donation. Together, lets reduce hunger',
      date: '09.20.2023',
      imageUrl: '/assets/logos/vegetables_01.png',
      profileImage: '/assets/logos/vegetable_logo.png',
      profileName: 'The Greenhouse',
    },
    {
      category: 'BBQ',
      categoryIcon: 'fas fa-plane',
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
}

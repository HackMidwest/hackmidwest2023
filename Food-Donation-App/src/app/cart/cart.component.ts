import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { UnclaimedPostModel } from '../models/UnclaimedPostModel';
import { Observable, tap } from 'rxjs';
import { FeedModel } from '../models/FeedModel';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit{


  constructor(private formBuilder:FormBuilder, private http:HttpClient, private appComponent:AppComponent) {}
  userName!: string;
  id!: number;

  newsItems: FeedModel[] = [];

  async ngOnInit() {
    this.userName = this.appComponent.username;
    this.id = this.appComponent.id;

    // let data = await this.getClaimedPosts() as UnclaimedPostModel[];
    // console.log("claimed posts: " + data);
    await this.getClaimedPosts();
  }

  async getClaimedPosts(): Promise<any>{
    var getURL = "https://fooddonationapi.azurewebsites.net/ClaimedPosts/" + this.id;
    var claimed = await this.http.get(getURL).toPromise() as UnclaimedPostModel[];

    this.newsItems = [];
    claimed.forEach(element => {

      this.newsItems.push(
        {
        category: element.type,
        title: element.title,
        description: element.text,
        date: element.time.toString(),
        imageUrl: element.foodTypePictureUrl,
        profileImage: element.profilePictureUrl,
        profileName: element.author,
        Id: element.id
        })
    });
  }
}

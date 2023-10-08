import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { UnclaimedPostModel } from '../models/UnclaimedPostModel';
import { FeedModel } from '../models/FeedModel';
import { ClaimPostModel } from '../models/ClaimPostModel';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})

export class FeedComponent implements OnInit {

  constructor(private http:HttpClient) {}

  getURL: string = "https://fooddonationapi.azurewebsites.net/UnclaimedPosts";

  showButton = false;

  newsItems: FeedModel[] = [];
  business:boolean = false;
  userId:number = -1;

  async ngOnInit() {
    let storedBusiness = localStorage.getItem("business");
    console.log(storedBusiness);
    this.business = storedBusiness != undefined ? JSON.parse(storedBusiness) : false;
    console.log(this.business);
    let storedUserId = localStorage.getItem("id");
    console.log(storedUserId);
    this.userId = storedUserId != undefined ? JSON.parse(storedUserId) as number : -1;

    await this.getUnclaimed();
  }

  async getUnclaimed () {
    var unclaimed = await this.http.get(this.getURL).toPromise() as UnclaimedPostModel[];
    console.log("received unclaimed: ");
    console.log(unclaimed);

    this.newsItems = [];
    unclaimed.forEach(element => {
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
        });
    });
  }

  lastAssignedId: number = 0;

  async claimItem(item: FeedModel) {
    console.log("selected item: " + item);
    await this.claimRequest(this.userId, item.Id);
    await this.getUnclaimed();
  }

  async claimRequest(profileId: number, postId: number) : Promise<any>{

    var headers = {'Content-Type' : 'application/json' };
    var body: ClaimPostModel =  {
      ProfileId: profileId,
      PostId: postId
    }
    console.log(body);
    return await this.http.post("https://fooddonationapi.azurewebsites.net/ClaimPost", JSON.stringify(body), {headers}).toPromise() as Promise<any>;
  }

}

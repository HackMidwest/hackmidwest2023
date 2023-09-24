import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { url } from 'inspector';

@Injectable({
  providedIn: 'root'
})
export class PennyartService {

  private myArtGetUrl = 'https://pa-api.azurewebsites.net/api/Doc/bob';

  constructor(private httpClient: HttpClient) { }

  getMyArt() {
    return this.httpClient.get(this.myArtGetUrl);
    }
}

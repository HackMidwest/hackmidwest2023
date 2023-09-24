import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private httpclient:HttpClient) { }

 


  async login(usrnm: string, passwd: string) : Promise<any>{

    var headers = {'Content-Type' : 'application/json' };
    var body = "{\"name\":\""+ usrnm +"\", \"password\":\""+ passwd +"\"}";
    return await this.httpclient.post("https://fooddonationapi.azurewebsites.net/Login", body, {headers}).toPromise() as Promise<any>;
  }



}

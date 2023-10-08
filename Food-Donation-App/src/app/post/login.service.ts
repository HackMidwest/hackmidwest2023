import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginRequestModel } from '../models/LoginRequestModel';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private httpclient:HttpClient) { }




  async login(username: string, password: string) : Promise<any>{

    var headers = {'Content-Type' : 'application/json' };
    var body: LoginRequestModel =  {
      name: username,
      password: password
    }
    return await this.httpclient.post("https://fooddonationapi.azurewebsites.net/Login", JSON.stringify(body), {headers}).toPromise() as Promise<any>;
  }



}

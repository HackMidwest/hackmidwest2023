import {Component, OnInit} from '@angular/core';
import {FormBuilder} from '@angular/forms';
import {HttpClient, HttpClientModule, HttpHeaders} from '@angular/common/http';
import {AppComponent} from '../app.component';
import { PostModel } from '../models/PostModel';

declare var bootstrap: any;

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {

  postURL: string = "https://fooddonationapi.azurewebsites.net/CreatePost";
  getByAuthorURL: string = "";

  constructor(private formBuilder: FormBuilder, private http: HttpClient, private appComponent: AppComponent) {
  }

  userName!: string;
  id!: number;
  pictureUrl!: string;


  showModal: boolean = false;


  httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  }

  ngOnInit(): void {
    this.userName = this.appComponent.username;
    this.id = this.appComponent.id;
    this.pictureUrl = this.appComponent.pictureUrl;
  }

  postForm = this.formBuilder.group({
    titleForm: this.formBuilder.control('',{nonNullable:true}),
    textForm: this.formBuilder.control('',{nonNullable:true}),
    imageForm: this.formBuilder.control('',{nonNullable:true}),
    type: this.formBuilder.control('',{nonNullable:true})
  })

  toggleModal() {
    this.showModal = !this.showModal;
  }

  onSubmit() {
    var title = this.postForm.controls['titleForm'].value;
    var text = this.postForm.controls['textForm'].value;
    var image = this.postForm.controls['imageForm'].value;
    var type = this.postForm.controls['type'].value;
    var date = (new Date()).toJSON();
    var profilePicture = this.getProfilePicture();
    var zipcode = this.getZipCode();

    const postModel: PostModel =  {
      title: title,
      text: text,
      time: date,
      author: this.userName,
      type: type,
      zipCode: 64017,
      claimant: -1
    };

    console.log(postModel);
    this.http.post(this.postURL, JSON.stringify(postModel), this.httpOptions).subscribe(data => {

      console.log(data);
    })
  }


  getProfilePicture() {
    var response = this.http.get(this.getByAuthorURL);

    return "";
  }

  getZipCode() {
    var response = this.http.get(this.getByAuthorURL);

    return "";
  }

  closeAndReset() {
    this.toggleModal(); // Close the modal
    this.postForm.reset(); // Reset the form
  }

}

import {Component} from '@angular/core';
import {FormBuilder} from '@angular/forms';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {AppComponent} from '../app.component';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent {
  showModal: boolean = false;

  constructor(private formBuilder: FormBuilder, private http: HttpClient, private appComponent: AppComponent) {
  }

  postForm = this.formBuilder.group({
    textForm: this.formBuilder.control(''),
    imageForm: this.formBuilder.control(''),
    type: this.formBuilder.control('')
  })

  toggleModal() {
    this.showModal = !this.showModal;
  }

  onSubmit() {
    var text = this.postForm.controls['textForm'].value;
  }

  closeAndReset() {
    this.toggleModal(); // Close the modal
    this.postForm.reset(); // Reset the form
  }

}

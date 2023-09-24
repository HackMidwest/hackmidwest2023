// import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { PennyartService } from '../service/pennyart.service';
import { IMyArt } from '../model/pennyart.model';
import { MessageService } from 'primeng/api';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-penny-art-home',
  templateUrl: './penny-art-home.component.html',
  styleUrls: ['./penny-art-home.component.scss'],
  providers: [MessageService],
})
export class PennyArtHomeComponent implements OnInit {
  public images!: IMyArt[];
  public inspireimages!: any[];
  upload = true;
  display: boolean = false;
  greetText!: string;
  selectedImage!: IMyArt;
  @ViewChild('greetings') greetings!: Dialog;

  constructor(
    private pennyartService: PennyartService,
    private messageService: MessageService,
    private primengConfig: PrimeNGConfig
  ) {}

  ngOnInit(): void {
    this.primengConfig.ripple = true;
    this.images = [];
    this.inspireimages = [];

    this.pennyartService.getMyArt().subscribe((resp: any) => {
      this.images = resp;
      this.inspireimages = resp;
    });

    // this.httpClient.get('https://pa-api.azurewebsites.net/api/Doc/bob')
    //   .subscribe((resp: any) => {
    //     this.images = resp;
    // })

    // for (let i = 1; i < 10; i++) {
    //   this.images.push(`https://picsum.photos/${i}00`);
    // }

    // for (let i = 11; i < 20; i++) {
    //   this.inspireimages.push(`https://picsum.photos/${i}00`);
    // }
  }

  onUpload(e: Event) {
    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: 'Image Uploaded Succesfully',
    });
    this.upload = true;
    this.pennyartService.getMyArt().subscribe((resp: any) => {
      this.images = resp;
    });
  }

  showDialog(item: IMyArt) {
    this.display = true;
    this.selectedImage = item;
  }

  sendGreetings(e: Event) {
    this.greetings.close(e);
    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: 'Greeting Sent Succesfully',
    });
  }
}

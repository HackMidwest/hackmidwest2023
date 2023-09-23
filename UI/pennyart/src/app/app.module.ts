import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FileUploadModule } from 'primeng/fileupload';
import {HttpClientModule} from '@angular/common/http';
import { PennyArtHomeComponent } from './penny-art-home/penny-art-home.component';
import { ImageModule } from 'primeng/image';
import { FieldsetModule } from 'primeng/fieldset';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GalleriaModule } from 'primeng/galleria';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    AppComponent,
    PennyArtHomeComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    FileUploadModule,
    HttpClientModule,
    ImageModule,
    FieldsetModule,
    BrowserAnimationsModule,
    GalleriaModule
  ],
  exports: [
    FileUploadModule,
    ImageModule,
    FieldsetModule,
    GalleriaModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

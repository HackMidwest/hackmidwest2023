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
import { DialogModule } from 'primeng/dialog';
import {InputTextareaModule} from 'primeng/inputtextarea';
import {ButtonModule} from 'primeng/button';
import { PennyartService } from './service/pennyart.service';
import { ToastModule } from 'primeng/toast';
import {RippleModule} from 'primeng/ripple';

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
    GalleriaModule,
    DialogModule,
    InputTextareaModule,
    ButtonModule,
    ToastModule,
    RippleModule
  ],
  exports: [
    FileUploadModule,
    ImageModule,
    FieldsetModule,
    GalleriaModule,
    DialogModule,
    InputTextareaModule,
    ButtonModule,
    ToastModule,
    RippleModule
  ],
  providers: [PennyartService],
  bootstrap: [AppComponent]
})
export class AppModule { }

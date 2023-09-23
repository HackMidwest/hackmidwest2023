import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PennyArtHomeComponent } from './penny-art-home/penny-art-home.component';

const routes: Routes = [
  { path: '', component: PennyArtHomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

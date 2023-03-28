import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { PaquetListComponent } from './paquet-list/paquet-list.component';
import { PaquetFormComponent } from './paquet-form/paquet-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { Routes,RouterModule } from '@angular/router';
import { CardFormComponent } from './card-form/card-form.component';
import { CardListComponent } from './card-list/card-list.component';

export const appRootList:Routes = [
  {path:'',component:PaquetListComponent},
  {path:'paquet-list',component:PaquetListComponent},
  {path:'paquet-form',component:PaquetFormComponent},
  {path:'card-form/:packetName',component:CardFormComponent},
  {path:'card-list',component:CardListComponent},
];

@NgModule({
  declarations: [
    AppComponent,
    PaquetListComponent,
    PaquetFormComponent,
    CardFormComponent,
    CardListComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRootList),
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot(appRootList)
  ],
  exports:[
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }

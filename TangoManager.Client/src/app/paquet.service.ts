import { Injectable } from '@angular/core';
import { HttpClient } from'@angular/common/http';
import { PaquetRecord } from './models/PaquetRecord';
import { Observable } from 'rxjs';
import { CardsRecord } from './models/CardsRecord';

@Injectable({
  providedIn: 'root'
})
export class PaquetService {
readonly rootURL='https://localhost:7107';

constructor(private http: HttpClient){}
getPaquetRecords(){
  return this.http.get(this.rootURL + '/api/Paquets');
  }
  addRecordPaquet(paquet:PaquetRecord){
    return this.http.post(this.rootURL + '/api/Paquets',paquet);
      }
 addRecordCard(card:CardsRecord){
return this.http.post(this.rootURL + '/api/Paquets/Card',card);
  }
  getCardRecords(){
    return this.http.get(this.rootURL + '/api/Paquets/Card');
    }
}

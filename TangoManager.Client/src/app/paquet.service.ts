import { Injectable } from '@angular/core';
import { HttpClient } from'@angular/common/http';
import { CardsCollection, PaquetRecord } from './models/PaquetRecord';
import { Observable } from 'rxjs';

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
deleteRecordPaquet(packetName:String){
  return this.http.delete(this.rootURL + '/api/Paquets/' +packetName);
}
addRecordCard(card:CardsCollection){
return this.http.put(this.rootURL + '/api/Paquets/Card',card);
  }
getCardRecords(){
  return this.http.get(this.rootURL + '/api/Paquets/Card');
  }
addQuizRecord(packetName:Object){
  return this.http.post(this.rootURL + '/api/Quiz',packetName);
    }
}


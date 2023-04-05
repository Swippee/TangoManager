import { Injectable } from '@angular/core';
import { HttpClient } from'@angular/common/http';
import { CardsCollection, PaquetRecord } from './models/PaquetRecord';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
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
  return this.http.post(this.rootURL + '/api/Paquets',paquet).pipe(catchError(this.handleError));;
  
  }
deleteRecordPaquet(packetName:String){
  return this.http.delete(this.rootURL + '/api/Paquets/' +packetName);
}
addRecordCard(card:CardsCollection){
return this.http.put(this.rootURL + '/api/Paquets/' + card.packetName,card).pipe(retry(1),catchError(this.handleError));
  }
getCardRecords(){
  return this.http.get(this.rootURL + '/api/Paquets/Card');
  }
addQuizRecord(packetName:Object){
  return this.http.post(this.rootURL + '/api/Quiz',packetName);
  }
  handleError(error:any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // server-side error
      // errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.error}`;
    }
    console.log(errorMessage);
    // return throwError(() => {
    //     return errorMessage;
    // });
    return throwError(error);
  }
}
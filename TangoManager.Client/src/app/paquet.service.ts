import { Injectable } from '@angular/core';
import { HttpClient } from'@angular/common/http';
import { PaquetRecord } from './models/PaquetRecord';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaquetService {
readonly rootURL='https://localhost:7107';

constructor(private http: HttpClient){}
 getRecords(){
  return this.http.get(this.rootURL + '/Paquets');
  }
 getRecordsB(): Observable<PaquetRecord[]>{
  return this.http.get<PaquetRecord[]>(this.rootURL + '/Paquet/Test');
  }
 getRecordC(): Observable<PaquetRecord>{
    return this.http.get<PaquetRecord>(this.rootURL + '/Paquet/Detail/Famille');
  }
}

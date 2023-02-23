import { Injectable } from '@angular/core';
import {HttpClient} from'@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PaquetService {
readonly rootURL='https//localhost:7051'
constructor(private http: HttpClient){}
  getRecords(){
    return this.http.get(this.rootURL + '/Paquet');
  }
}

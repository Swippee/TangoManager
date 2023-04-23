import { Component, OnInit } from '@angular/core';
import { PaquetService } from '../paquet.service';
import { PaquetRecord } from '../models/PaquetRecord';
import * as moment  from 'node_modules/moment/moment.js';
import { Router } from '@angular/router';
import { Observable, map } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-paquet-list',
  templateUrl: './paquet-list.component.html',
  styleUrls: ['./paquet-list.component.css']
})
export class PaquetListComponent  {
  records:PaquetRecord[]=[];
  // tests:TestRecord[] ;
  // result:TestRecord;
  canDisplayData=false;
constructor(
  private paquetService: PaquetService,
  private router: Router,
  private cookie: CookieService 
  ){}
ngOnInit():void {
   
   this.getAllRecords();
  
}

  getAllRecords() {
    var date = moment();
      this.paquetService.getPaquetRecords().subscribe((data) => {
      this.canDisplayData = true;
      console.log(data);
      this.records = data as PaquetRecord[];

      this.records.forEach(record => {
        record.rootEntity.lastModification=moment(record.rootEntity.lastModification as moment.MomentInput).format('YYYY/D/M');
        
      });
    })
  }
  goToCreatePaquet(){
    this.router.navigate(['/','paquet-form']);
  }
  
  onDelete(packetName: String){
    if (confirm(`Voulez vous vraiment supprimer le Paquet ${packetName}.?`)){
      this.paquetService.deleteRecordPaquet(packetName).subscribe((data)=>{
      this.getAllRecords();
      });
    }
    else this.getAllRecords();
  }
  redirectToCreateCard(packetEntity: PaquetRecord){
    this.router.navigateByUrl('/card-form/'+packetEntity);
  }

  redirectToStartQuiz(packetName: String){
    this.paquetService.addPacketLock(packetName).subscribe((res)=>{
      let tokenValue= res as {token:string}
      this.cookie.set('TOKENUSER_VALUE',tokenValue.token)
      this.router.navigateByUrl('/quiz-form/'+packetName);
      },
      (error: HttpErrorResponse)=>{
        alert(error.error.messageShort);
      }
    )
    
  }
}


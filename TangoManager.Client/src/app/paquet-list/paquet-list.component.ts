import { Component, OnInit } from '@angular/core';
import { PaquetService } from '../paquet.service';
import { PaquetRecord } from '../models/PaquetRecord';
import * as moment  from 'node_modules/moment/moment.js';
import { Router } from '@angular/router';

@Component({
  selector: 'app-paquet-list',
  templateUrl: './paquet-list.component.html',
  styleUrls: ['./paquet-list.component.css']
})
export class PaquetListComponent  {
  records:PaquetRecord[]=[];
  canDisplayData=false;
constructor(
  private paquetService: PaquetService,
  private router: Router 
  ){}
ngOnInit():void {
   
   this.getAllRecords();
  
}

  getAllRecords() {
    var date = moment();
      this.paquetService.getPaquetRecords().subscribe((data) => {
      this.canDisplayData = true;
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
}


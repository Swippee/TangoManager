import { Component, OnInit } from '@angular/core';
import { PaquetService } from '../paquet.service';
import { PaquetRecord } from '../models/PaquetRecord';
@Component({
  selector: 'app-paquet-list',
  templateUrl: './paquet-list.component.html',
  styleUrls: ['./paquet-list.component.css']
})
export class PaquetListComponent {
  records:PaquetRecord[]=[];
  record:PaquetRecord | undefined;
  canDisplayData=false;
constructor(private paquetService: PaquetService){}
ngOnInit():void{
   this.getAllRecords();
  // this.getAllRecordsB();
  //this.getRecordByName();
  
}

  getAllRecords() {
      this.paquetService.getRecords().subscribe((data) => {
      this.canDisplayData = true;
      this.records = data as PaquetRecord[];
      console.log(data);
      console.log(this.records);
      console.log(this.records[0]);
      console.log(this.records[0].Nom);
    })
    console.log("Sortie");
  }
  getAllRecordsB() {
    this.paquetService.getRecordsB().subscribe({
      next:(records) =>{
        console.log(records);
        console.log(records[0]);
        console.log(records[0].Description);
      },
      error:(response)=>{
        console.log(response);
      },
      
    })
  console.log("Sortie");
  }
  getRecordByName() {
    this.paquetService.getRecordC().subscribe({
      next:(record) =>{
        console.log(record);   
        console.log(record.Nom);             
      },
      error:(response)=>{
        console.log(response);
      },
      
    })
  console.log("Sortie");
  }
}

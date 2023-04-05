import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaquetService } from '../paquet.service';
import { Router,ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
@Component({
  selector: 'app-paquet-form',
  templateUrl: './paquet-form.component.html',
  styleUrls: ['./paquet-form.component.css']
})
export class PaquetFormComponent implements OnInit {
public addRecordPaquet:FormGroup;
submitButton="Ajouter un paquet";

constructor(private fb:FormBuilder,
  private paquetService : PaquetService,
  private router: Router){}
ngOnInit(): void {
  this.emptyForm();
}

emptyForm(){
  this.addRecordPaquet = this.fb.group({
    name:['',[Validators.required]],
    description : ['',[Validators.required]]
  });
}
get registerFormControl():{[key: string]:AbstractControl}{
  return this.addRecordPaquet.controls;
}
onSubmit(){
this.paquetService.addRecordPaquet(this.addRecordPaquet.value).subscribe((res)=>{
  console.log(res);
  console.log(typeof res);
  this.emptyForm();

  this.router.navigate(['/paquet-list']);
  },
  (error: HttpErrorResponse)=>{
    alert(error.error);
  }
)

}
}

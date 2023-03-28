import { Component,OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaquetService } from '../paquet.service';
import { Router,ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-card-form',
  templateUrl: './card-form.component.html',
  styleUrls: ['./card-form.component.css']
})
export class CardFormComponent implements OnInit{
  public addRecordCard:FormGroup;
  submitButton="Ajouter une carte";
  public test : string;
  constructor(private fb:FormBuilder,
    private paquetService : PaquetService,
    private router: Router,
    private activatedRoute:ActivatedRoute){}
  ngOnInit(): void {
    this.emptyForm();
  }
  
  emptyForm(){
    console.log(this.activatedRoute.snapshot.paramMap.get('packetName')?.toString());
    
    this.addRecordCard = this.fb.group({
      packetName:this.activatedRoute.snapshot.paramMap.get('packetName')?.toString(),
      question : ['',[Validators.required]],
      answer : ['',[Validators.required]]
    });
  }
  get registerFormControl():{[key: string]:AbstractControl}{
    return this.addRecordCard.controls;
  }
  onSubmit(){
  this.paquetService.addRecordCard(this.addRecordCard.value).subscribe((res)=>{
    console.log(res);
    this.emptyForm();
    this.router.navigate(['/paquet-list']);
  })
  
  }
  }

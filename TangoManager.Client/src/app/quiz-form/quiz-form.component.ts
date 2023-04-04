import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaquetService } from '../paquet.service';
import { Router,ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-quiz-form',
  templateUrl: './quiz-form.component.html',
  styleUrls: ['./quiz-form.component.css']
})

export class QuizFormComponent implements OnInit {

  public packetName: String;
  public Quiz : Object
  constructor(private fb:FormBuilder,
    private paquetService : PaquetService,
    private router: Router,
    private activatedRoute:ActivatedRoute){}

  ngOnInit(): void {
    this.CreateQuiz();
  }
  CreateQuiz(){
    console.log("start creation quiz");
     this.Quiz={
      packetName:this.activatedRoute.snapshot.paramMap.get('packetName')?.toString()
    }
    this.paquetService.addQuizRecord(this.Quiz).subscribe((res)=>{
      console.log(res);})

  }
}

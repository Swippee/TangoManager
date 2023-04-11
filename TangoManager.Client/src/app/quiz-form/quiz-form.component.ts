import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaquetService } from '../paquet.service';
import { Router,ActivatedRoute } from '@angular/router';
import { QuizRecord } from '../models/QuizRecord';

@Component({
  selector: 'app-quiz-form',
  templateUrl: './quiz-form.component.html',
  styleUrls: ['./quiz-form.component.css']
})

export class QuizFormComponent implements OnInit {

  public packetName: String;
  public CreateQuizObject : Object;
  public Quiz : QuizRecord;
  public StateOfQuiz: String;
  public answerQuiz:FormGroup;
  public currentIndex=0;
  constructor(private fb:FormBuilder,
    private paquetService : PaquetService,
    private router: Router,
    private activatedRoute:ActivatedRoute){}

  ngOnInit(): void {
    this.CreateQuiz();
  }
  CreateQuiz(){
    console.log("start creation quiz");
     this.CreateQuizObject={
      packetName:this.activatedRoute.snapshot.paramMap.get('packetName')?.toString()
    }
    this.paquetService.addQuizRecord(this.CreateQuizObject).subscribe((res)=>{
      this.Quiz = res as QuizRecord;
      console.log(this.Quiz);
      //Démarrage
      if (this.Quiz.quiz.currentCardId==this.Quiz.packet.cardsCollection[0].id && this.Quiz.quiz.currentState=="Active")
      this.StateOfQuiz="Start";
      else if (this.Quiz.quiz.currentState=="Finished")
      this.StateOfQuiz="End";
      else this.StateOfQuiz="Running";
    })
  }
  StartQuiz(){ 
    if (this.Quiz.quiz.quizCardsCollection.length==0){
      this.currentIndex=1;
    }
    else this.currentIndex=this.Quiz.quiz.quizCardsCollection.length +1;
    this.StateOfQuiz="Running";  
    this.answerQuiz = this.fb.group({
      quizId:this.Quiz.quiz.id,
      question : this.Quiz.packet.cardsCollection
              .filter( (item) =>{return item.id==this.Quiz.quiz.currentCardId})
              .map((item)=>{return item.question}),

      answer : ['',[Validators.required]]
    });
}
onSubmitAnswer(){
  this.paquetService.answerQuizRecord(this.answerQuiz.value).subscribe((res)=>{

    this.Quiz = res as QuizRecord;
    console.log(this.Quiz);
 
  
    if (this.Quiz.quiz.currentState=="Finished")
    this.StateOfQuiz="End";
    else {
      // Affichage bonne/ mauvaise réponse?

      // Lancement Next Quiz
      this.StartQuiz()
    }

   
  })
}
}

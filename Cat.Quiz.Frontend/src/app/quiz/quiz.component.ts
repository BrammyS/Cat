import { Component, OnInit } from '@angular/core';
import {Observable} from 'rxjs';
import {Quiz} from '../models/quiz';
import {QuizService} from '../services/quiz.service';

@Component({
  selector: 'app-quiz',
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.scss']
})
export class QuizComponent implements OnInit {

  quiz: Observable<Quiz> = this.quizService.getQuiz(1);

  constructor(private quizService: QuizService) {

  }

  ngOnInit() {
  }

}

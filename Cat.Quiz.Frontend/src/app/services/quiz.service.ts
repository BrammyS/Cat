import { Injectable } from '@angular/core';
import {Observable, of} from 'rxjs';
import {defaultQuiz, Quiz} from '../models/quiz';

@Injectable({
  providedIn: 'root'
})
export class QuizService {

  constructor() { }

  getQuizzes(): Observable<number[]> {
    return of([1]);
  }


  getQuiz(id: number): Observable<Quiz>  {
    debugger;
    return of(defaultQuiz);
  }

}



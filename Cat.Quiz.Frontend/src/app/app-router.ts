import {RouterModule, Routes} from '@angular/router';
import {QuizComponent} from './quiz/quiz.component';
import {NgModule} from '@angular/core';

export const appRoutes: Routes = [
  {
    path: 'quiz',
    component: QuizComponent
  },

];

@NgModule({
  imports: [RouterModule.forChild(appRoutes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

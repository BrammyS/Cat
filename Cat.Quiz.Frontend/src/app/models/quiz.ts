export class Quiz implements BaseModel {
  id: number;
  personalities: Personality[] = [];
  questions: Question[] = [];
}

export class Question implements BaseModel {
  id: number;
  order: number;
  answers: Answer[] = [];
  question: string;
  type: QuestionType | undefined;
  required: boolean;
}


export class Answer implements BaseModel  {
  id: number;
  answer: string;
  personalities: Personality[] = [];
}

export class Personality implements BaseModel  {
    id: number;
    name: string;

    constructor(id: number, name: string) {
      this.id = id;
      this.name = name;
    }
}

export interface BaseModel {
  id: number;
}

export enum QuestionType {
  multipleChoice = 1,
  textInput = 2,
}

const orderOfHostilia = new Personality(1, 'Hostilia');
const orderOfReciprocus = new Personality(2, 'Reciprocus');
const orderOfSocius = new Personality(3, 'Socius');
const orderOfDoctrina = new Personality(4, 'Doctrina');
const orders = [orderOfHostilia, orderOfReciprocus, orderOfSocius, orderOfDoctrina];

const discordNameQuestion = new Question();
discordNameQuestion.id = 1;
discordNameQuestion.question = 'What is your discord tag (including #)';
discordNameQuestion.type = QuestionType.textInput;
discordNameQuestion.required = true;
discordNameQuestion.order = 1;

const normalQuestion = new Question();
discordNameQuestion.id = 2;
discordNameQuestion.question = 'Aw yis 5 vs 1 remaining what do ?';
discordNameQuestion.type = QuestionType.textInput;
discordNameQuestion.required = true;
discordNameQuestion.order = 1;

const answer1 = new Answer();
answer1.id = 1;
answer1.answer = 'Sprint towards the sounds of gunfire';
answer1.personalities = [orderOfHostilia];

const answer2 = new Answer();
answer2.id = 2;
answer2.answer = 'Flick throughg cameras or track him down through info';
answer2.personalities = [orderOfSocius];

const answer3 = new Answer();
answer3.id = 3;
answer3.answer = 'Hold my angle. He\'ll probably come through this way';
answer3.personalities = [orderOfReciprocus];

const answer4 = new Answer();
answer4.id = 4;
answer4.answer = 'I\'m sure one of my mates will get him anyway';
answer4.personalities = [orderOfSocius];

const answer5 = new Answer();
answer4.id = 5;
answer4.answer = 'It\'s now a race with my team to get them!';
answer4.personalities = [orderOfHostilia, orderOfSocius];

normalQuestion.answers = [answer1, answer2, answer3, answer4, answer5];

const defaultQuiz = new Quiz();
defaultQuiz.personalities = orders;
defaultQuiz.questions = [discordNameQuestion, normalQuestion];

export {defaultQuiz};


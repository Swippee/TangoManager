import { CardsCollection, PaquetEntity } from "./PaquetRecord";

export interface QuizRecord {
    quiz:   Quiz;
    packet: PaquetEntity;
    currentCard:              CardsCollection;
    answeredCards:            CardsCollection[];
    correctlyAnsweredCards:   CardsCollection[];
    incorrectlyAnsweredCards: CardsCollection[];
}
export interface Quiz {
    id:                  number;
    packetName:          string;
    currentCardId:       number;
    currentState:        string;
    creationDate:        Date;
    totalScore:          number;
    quizCardsCollection: QuizCardsCollection[];
}
export interface QuizCardsCollection {
    idCard:    number;
    idQuiz:    number;
    isCorrect: boolean;
}

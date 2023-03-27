import { CardsRecord } from "./CardsRecord";

export interface PaquetRecord {
    name: String;
    description : String;
    lastModification : String;
    lastQuiz: Date;
    cardsCollection: CardsRecord[];
}
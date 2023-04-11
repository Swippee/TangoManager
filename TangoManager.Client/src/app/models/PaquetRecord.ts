
export interface PaquetRecord {
    rootEntity: PaquetEntity;
    addedCards: any[];
}

export interface PaquetEntity {
    name:             string;
    description:      string;
    lastModification: string;
    lastQuiz:         string;
    cardsCollection:  CardsCollection[];
}

export interface CardsCollection {
    id:               number;
    packetName:       string;
    question:         string;
    answer:           string;
    score:            number;
    lastModification: string;
    lastQuiz: string        
}
// export interface TestRecord {
//     packet: RootEntity;
//     addedCards: any[];
// }

export interface PaquetRecord {
    rootEntity: RootEntity;
    addedCards: any[];
}

export interface RootEntity {
    name:             string;
    description:      string;
    lastModification: string;
    cardsCollection:  CardsCollection[];
}

export interface CardsCollection {
    id:               number;
    packetName:       string;
    question:         string;
    answer:           string;
    score:            number;
    lastModification: Date;
}

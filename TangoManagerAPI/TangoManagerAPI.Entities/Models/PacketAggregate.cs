using System;
using System.Collections.Generic;
using System.Linq;
using TangoManagerAPI.Entities.Events;
using TangoManagerAPI.Entities.Exceptions;

namespace TangoManagerAPI.Entities.Models
{
    public class PacketAggregate : IAggregateRoot<PaquetEntity>
    {
        public PaquetEntity RootEntity { get; }

        private readonly  ICollection<AEvent> _eventsCollection;

        public IEnumerable<CarteEntity> AddedCards => _addedCardsCollection;
        private readonly ICollection<CarteEntity> _addedCardsCollection;

        public PacketAggregate(PaquetEntity packetEntity)
        {
            _eventsCollection = new List<AEvent>();
            _addedCardsCollection = new List<CarteEntity>();
            RootEntity = packetEntity;
        }

        public IEnumerable<AEvent> AddCard(CarteEntity cardEntity)
        {
            var existingCard = RootEntity.CardsCollection.FirstOrDefault(x => x.Equals(cardEntity));
            if (existingCard != null) 
               throw new CardAlreadyExistsInPacketException($"Card {cardEntity.Id} - {cardEntity.Question} already exists in packet {RootEntity.Name}!");

            _addedCardsCollection.Add(cardEntity);
            RootEntity.CardsCollection.Add(cardEntity);
            RootEntity.LastModification = DateTime.UtcNow;
            _eventsCollection.Add(new PacketUpdatedEvent(RootEntity));
            return _eventsCollection;
        }
    }
}

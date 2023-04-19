import { Interception } from "cypress/types/net-stubbing"
import { PaquetListComponent} from "src/app/paquet-list/paquet-list.component"
import { PaquetRecord} from "src/app/models/PaquetRecord"
import { PaquetService } from 'src/app/paquet.service';

// const paquetService = new PaquetService()
// const PaquetRecord = PaquetService.getPaquetRecords() 
// https://www.chaijs.com/plugins/chai-sorted/

beforeEach('Accessing the Home Page',() => {
  cy.intercept("https://localhost:7107/api/Paquets").as('call')
  cy.visit('/')
})

describe('Headings correct', () => {
  it('Confirms the headings of the table', () => {
    const headings =['Nom du paquet','Description','Date du dernier Quiz','Nombre de cartes','Action']
    cy.get('table thead th').then(($th) => {
      const texts = Cypress._.map($th,'innerText')
      expect(texts,'headings').to.deep.eq(headings)
    })
  })
})
describe('CreatePacket navigation',()=>{
  it('Navigate toward the component packet-form',() =>{

  cy.get('a[data-cy="createPacket"]').click()
  cy.url().should('include','paquet-form')
  })
})
describe('Add card navigation via packetLink',()=>{
  it('Navigate to the componenent card-form',() =>{
  cy.get('#addCardLink')
  .first()
  .click()
  cy.url().should('include','card-form/cheval')

  })
})
describe('Call API back-end return code 200',()=>{
  it ('Confirms that the back-end API TangoManager is called with success',()=>{
      cy.wait('@call').its('response.statusCode').should('eq',200) 
  })
})

describe('Start Quiz visibility restricted',()=>{
  it('Start Quizz should not be visible if there is no card in the packet',()=>{
    cy.get('tbody')
      .find('tr')
      .each((item,index)=>{
        cy.wrap(item)
        .find('#cardsCollection')
        .then(($ele)=>{
          if ($ele.text()!="0" )
          cy.wrap(item).find('#linkToQuiz').should('exist')
          else
          cy.wrap(item).find('#linkToQuiz').should('not.exist')
        })
    })
  })
})


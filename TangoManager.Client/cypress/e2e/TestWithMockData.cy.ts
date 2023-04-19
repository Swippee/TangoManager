beforeEach('Accessing the Home Page',() => {
  let dynamicStatusCodeStub = 404;
    cy.intercept('GET', 'https://localhost:7107/api/Paquets', { 
      fixture: 'getAllPacket.json',
      statusCode:dynamicStatusCodeStub})
      .as('mockData')
})

describe('Call API back-end HIJACK FORCED FALSE',()=>{
  it ('Test cas KO erreur back-end',()=>{
    cy.visit("/")
    cy.wait('@mockData').its('response.statusCode').should('eq',404) 
    // cy.wait('@mockData').then(xhr => {
    //     console.log(xhr)
    //     expect(xhr.response?.statusCode.valueOf()).to.equal(200)
    // })
  })
})

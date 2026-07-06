describe('Portfolio E2E Tests', () => {
  it('should load the homepage and check main elements', () => {
    cy.visit('/');

    cy.get('nav').should('be.visible');

    cy.contains('Home').should('be.visible');
    cy.contains('Contato').should('be.visible');

    cy.contains('Contato').click();
    
    cy.get('#contato').should('be.visible');

    cy.get('input[placeholder="Seu nome"]').type('Testador Cypress');
    cy.get('input[placeholder="seu@email.com"]').type('teste@cypress.io');
    cy.get('textarea[placeholder="Sua mensagem..."]').type('Esta é uma mensagem de teste automatizado disparada pelo Cypress.');

    cy.get('button').contains('Enviar mensagem').click();

    cy.contains('Obrigado pelo contato!', { timeout: 10000 }).should('be.visible');
  });
});

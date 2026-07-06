describe('Admin Edit Mode E2E', () => {
  it('should authenticate via mock and edit the profile name', () => {
    // 1. Intercepta a chamada de validação do OTP e força um sucesso (Bypass)
    cy.intercept('GET', '**/api/auth/verify*', {
      statusCode: 200,
      body: { sessionToken: 'fake-jwt-token-cypress' }
    }).as('verifyAuth');

    // 2. Intercepta a chamada de salvamento de perfil para não sujar o banco real
    cy.intercept('PUT', '**/api/profile', {
      statusCode: 200,
      body: { message: 'Profile updated successfully' }
    }).as('updateProfile');

    // Navega diretamente para a tela de verificação OTP
    cy.visit('/auth/verify');
    cy.wait(1500); // Pausa leitura humana

    // Digita um código qualquer
    cy.get('input[placeholder="000000"]').type('123456', { delay: 150 });
    cy.wait(500);
    
    // Clica no botão de verificar
    cy.get('button').contains('Verificar código').click();

    // Aguarda a interceptação do login falso
    cy.wait('@verifyAuth');

    // Verifica se fomos redirecionados para a Home
    cy.url().should('eq', Cypress.config().baseUrl + '/');
    cy.wait(2000); // Pausa humana analisando a tela editável

    // Na Home em modo edição, o texto do Hero vira um campo de input
    // Vamos procurar o input que guarda o título/nome dentro da seção Hero
    cy.get('.hero-text app-inline-editor input').first().as('nameEditor');
    
    // Verifica se o campo de edição está visível (Edit Mode ativado)
    cy.get('@nameEditor').should('be.visible');

    // Limpa o conteúdo atual, escreve um novo e aperta Enter (salvar)
    cy.get('@nameEditor').clear().type('Editado via Cypress E2E{enter}', { delay: 80 });

    // Aguarda que o frontend dispare a requisição HTTP PUT para salvar
    cy.wait('@updateProfile').then((interception) => {
      // Valida se o frontend mandou o payload correto para o backend
      expect(interception.request.body.name).to.include('Editado via Cypress E2E');
    });

    // Como o Angular atualizou os Signals, o valor do input deve permanecer o novo
    cy.get('@nameEditor').should('have.value', 'Editado via Cypress E2E');
    cy.wait(2000); // Pausa final para visualização no vídeo
  });
});

describe('Admin Operations E2E', () => {
  beforeEach(() => {
    // Intercepta a chamada de validação do OTP e força um sucesso (Bypass)
    cy.intercept('GET', '**/api/auth/verify*', {
      statusCode: 200,
      body: { sessionToken: 'fake-jwt-token-cypress' }
    }).as('verifyAuth');

    // Navega diretamente para a tela de verificação OTP e faz login
    cy.visit('/auth/verify');
    cy.wait(1000); // Pausa para leitura humana
    cy.get('input[placeholder="000000"]').type('123456', { delay: 100 });
    cy.wait(500);
    cy.get('button').contains('Verificar código').click();
    cy.wait('@verifyAuth');
    cy.url().should('eq', Cypress.config().baseUrl + '/');
    cy.wait(1000); // Pausa após entrar na home
  });

  it('should add, edit and delete a skill', () => {
    // 1. Mocks da API de Skills
    cy.intercept('POST', '**/api/skills', {
      statusCode: 201,
      body: { id: 9999, name: 'Cypress Skill', category: 0, description: 'Mock', proficiencyLevel: 90, displayOrder: 99 }
    }).as('createSkill');

    cy.intercept('PUT', '**/api/skills/9999', {
      statusCode: 200,
      body: { message: 'Skill updated' }
    }).as('updateSkill');

    cy.intercept('DELETE', '**/api/skills/9999', {
      statusCode: 204
    }).as('deleteSkill');

    // Navega até a seção de resumo (skills)
    cy.get('#resumo').scrollIntoView({ duration: 1000 });
    cy.wait(1500); // Pausa humana

    // Clica no botão de adicionar habilidade
    cy.get('.add-item .add-btn').click();
    cy.wait(1000);

    // Preenche o formulário
    cy.get('input#skillName').type('Cypress Skill', { delay: 50 });
    cy.wait(500);
    cy.get('textarea#skillDesc').type('Testing skill addition', { delay: 50 });
    cy.wait(1000);
    cy.get('button.btn-save').contains('Salvar Habilidade').click();

    // Aguarda chamada de criação
    cy.wait('@createSkill');
    cy.wait(1500);

    // Em EditMode, o nome está dentro de um input. Vamos encontrá-mo filtrando pelo valor.
    cy.get('h5 input').filter((k, el) => (el as HTMLInputElement).value === 'Cypress Skill').as('skillTitleInput');
    cy.get('@skillTitleInput').scrollIntoView().should('be.visible');
    cy.wait(1000);

    // Edita a habilidade recém criada
    cy.get('@skillTitleInput').clear().type('Cypress Skill Editada{enter}', { delay: 50 });
    cy.wait('@updateSkill');
    cy.wait(1500);
    
    cy.get('h5 input').filter((k, el) => (el as HTMLInputElement).value === 'Cypress Skill Editada').as('skillTitleInputEdited');
    cy.get('@skillTitleInputEdited').scrollIntoView().should('be.visible');
    cy.wait(1000);

    // Deleta a habilidade (o botão de deletar é irmão do app-inline-editor dentro de h5)
    cy.get('@skillTitleInputEdited').parents('h5').find('.delete-btn').click();
    cy.wait(1000);
    
    // Confirma na modal
    cy.get('app-confirm-dialog button').contains('Excluir').click();
    
    // Aguarda chamada de exclusão
    cy.wait('@deleteSkill');
    cy.wait(1000);

    // Verifica se sumiu
    cy.get('h5 input').filter((k, el) => (el as HTMLInputElement).value === 'Cypress Skill Editada').should('not.exist');
    cy.wait(1500);
  });

  it('should add, edit and delete a project', () => {
    // 1. Mocks da API de Projetos
    cy.intercept('POST', '**/api/projects', {
      statusCode: 201,
      body: { id: 8888, title: 'Cypress Project', description: 'Mock', technologies: 'Cypress, Testing', displayOrder: 99 }
    }).as('createProject');

    cy.intercept('PUT', '**/api/projects/8888', {
      statusCode: 200,
      body: { message: 'Project updated' }
    }).as('updateProject');

    cy.intercept('DELETE', '**/api/projects/8888', {
      statusCode: 204
    }).as('deleteProject');

    // Navega até a seção de portfólio
    cy.get('#portfolio').scrollIntoView({ duration: 1000 });
    cy.wait(1500);

    // Clica no botão de adicionar projeto
    cy.get('.projects-grid .add-card').click();
    cy.wait(1000);

    // Preenche o formulário
    cy.get('input#projectTitle').type('Cypress Project', { delay: 50 });
    cy.wait(500);
    cy.get('input#projectTechs').type('Cypress, Testing', { delay: 50 });
    cy.wait(500);
    cy.get('textarea#projectDesc').type('Project added via e2e', { delay: 50 });
    cy.wait(1000);
    cy.get('button.btn-save').contains('Salvar Projeto').click();

    // Aguarda chamada de criação
    cy.wait('@createProject');
    cy.wait(1500);

    // Encontra o input com o valor do projeto
    cy.get('h4 input').filter((k, el) => (el as HTMLInputElement).value === 'Cypress Project').as('projectTitleInput');
    cy.get('@projectTitleInput').should('be.visible');
    cy.wait(1000);

    // Edita o projeto recém criado
    cy.get('@projectTitleInput').clear().type('Cypress Project Editado{enter}', { delay: 50 });
    cy.wait('@updateProject');
    cy.wait(1500);
    
    cy.get('h4 input').filter((k, el) => (el as HTMLInputElement).value === 'Cypress Project Editado').as('projectTitleInputEdited');
    cy.get('@projectTitleInputEdited').should('be.visible');
    cy.wait(1000);

    // Deleta o projeto
    cy.get('@projectTitleInputEdited').parents('.project-card').find('.delete-btn').click();
    cy.wait(1000);
    
    // Confirma na modal
    cy.get('app-confirm-dialog button').contains('Excluir').click();
    
    // Aguarda chamada de exclusão
    cy.wait('@deleteProject');
    cy.wait(1000);

    // Verifica se sumiu
    cy.get('h4 input').filter((k, el) => (el as HTMLInputElement).value === 'Cypress Project Editado').should('not.exist');
    cy.wait(1500);
  });
});

# Relatório de Testes do Backend (API)

Este documento detalha as suítes de testes unitários criadas para o backend do Portfólio, focando na camada de serviços (Services). Foram utilizadas as bibliotecas `xUnit`, `Moq` e o provedor `InMemoryDatabase` do Entity Framework Core.

## 1. ProjectsServiceTests

Este arquivo testa as regras de negócio para as operações relacionadas a Projetos.

- **GetAllProjectsAsync_ShouldReturnSortedProjects:** Verifica se os projetos são retornados do banco de dados ordenados pela propriedade `DisplayOrder`.
- **CreateProjectAsync_ShouldAddProject:** Valida se a criação de um projeto insere o novo registro no banco de dados e retorna o DTO correto.
- **UpdateProjectAsync_ShouldModifyProject:** Garante que a atualização de um projeto existente modifica seus dados no banco e retorna a versão atualizada.
- **UpdateProjectAsync_ShouldReturnNull_WhenNotExists:** Assegura que tentar atualizar um projeto com ID inexistente retorna `null`.
- **DeleteProjectAsync_ShouldRemoveProject_AndReturnTrue:** Testa a remoção bem-sucedida de um projeto e a confirmação (`true`).
- **DeleteProjectAsync_ShouldReturnFalse_WhenNotExists:** Verifica se a tentativa de excluir um ID inválido retorna `false`.

## 2. SkillsServiceTests

Avalia o comportamento das operações de gerenciamento de Habilidades.

- **GetAllSkillsAsync_ShouldReturnSortedSkills:** Semelhante a Projetos, mas focado na ordenação correta das habilidades.
- **CreateSkillAsync_ShouldAddSkill:** Testa a adição de um novo registro na tabela `Skills`.
- **UpdateSkillAsync_ShouldModifySkill:** Garante que os campos modificados de uma habilidade sejam refletidos no banco de dados corretamente.
- **UpdateSkillAsync_ShouldReturnNull_WhenNotExists:** Trata o cenário de atualização de um registro inexistente (retorna `null`).
- **DeleteSkillAsync_ShouldRemoveSkill:** Valida se a exclusão remove fisicamente o registro (esperado `true`).
- **DeleteSkillAsync_ShouldReturnFalse_WhenNotExists:** Confirma o comportamento de exclusão com falha segura (`false`).

## 3. SocialLinksServiceTests

Testa as operações de Links Sociais (GitHub, LinkedIn, etc.).

- **GetAllLinksAsync_ShouldReturnSortedLinks:** Garante a ordenação pela ordem de exibição (`DisplayOrder`).
- **CreateLinkAsync_ShouldAddLink:** Valida a persistência de um novo link.
- **UpdateLinkAsync_ShouldModifyLink:** Verifica se as alterações de plataforma ou URL são aplicadas com sucesso.
- **UpdateLinkAsync_ShouldReturnNull_WhenNotExists:** Garante tratamento seguro para IDs inexistentes (retorna `null`).
- **DeleteLinkAsync_ShouldRemoveLink:** Checa se o registro é removido corretamente (`true`).
- **DeleteLinkAsync_ShouldReturnFalse_WhenNotExists:** Valida a recusa ao tentar remover o que não existe (`false`).

## 4. ContactServiceTests

Este serviço possui uma arquitetura diferente, pois depende do envio de emails. Foi utilizado a biblioteca `Moq` para "simular" o envio de e-mails, sem enviar de verdade durante a bateria de testes.

- **SendMessageAsync_ShouldReturnSuccess_WhenEmailSent:** 
  - **O que testa:** Injeta um envio de e-mail falso (`Mock`) que sempre tem sucesso.
  - **Verificação:** Checa se a função `SendEmailAsync` foi chamada exatamente 1 vez com os parâmetros corretos (Nome, Assunto, Mensagem) e garante que o método principal retorne `true`. Além disso, garante que a mensagem foi persistida no banco local.
- **SendMessageAsync_ShouldReturnFalse_WhenExceptionThrown:**
  - **O que testa:** Configura o Mock do provedor de E-mail para forçar um erro (simulando falha de SMTP).
  - **Verificação:** Garante que o serviço capture o erro de envio sem falhar a aplicação e retorne `false`.

## 5. ProfileServiceTests (Legado)

Os testes originais do perfil, mantidos e aprovados.

- **GetProfileAsync_ShouldReturnNull_WhenProfileDoesNotExist:** Verifica se o perfil principal não existe e não retorna nada.
- **GetProfileAsync_ShouldReturnProfile_WhenItExists:** Testa o retorno do perfil quando há cadastro.
- **UpdateProfileAsync_ShouldUpdate_AndReturnProfileDto:** Atualiza dados do perfil.

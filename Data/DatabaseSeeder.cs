using MyHub.Entities;
using MyHub.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyHub.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            //using var scope = serviceProvider.CreateScope();
            //var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User<Guid>>>();

            //// Verifica se o banco já possui usuários cadastrados para não duplicar o seed
            //if (await context.Users.AnyAsync())
            //{
            //    return;
            //}

            //// ==========================================
            //// 1. POPULAR USUÁRIOS (Tabela: Users)
            //// ==========================================
            //// Nota: No ApplicationDbContext atual não utilizamos tabelas do ASP.NET Core Identity (como IdentityRole ou AspNetUserRoles).
            //// Os papéis/roles são gerenciados por quadro (Board) através do enum BoardRole na tabela intermediária UserBoard.
            //var adminUser = new User<Guid>
            //{
            //    Id = Guid.NewGuid(),
            //    UserName = "admin",
            //    Name = "Administrador / Tech Lead",
            //    Email = "admin@kaanboard.com",
            //    PhoneNumber = "+55 11 99999-0001",
            //    CreatedAt = DateTimeOffset.UtcNow,
            //    UpdatedAt = DateTimeOffset.UtcNow,
            //    FlAtivo = true
            //};
            //adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123*");
    
            //var memberUser = new User<Guid>
            //{
            //    Id = Guid.NewGuid(),
            //    UserName = "carlos_dev",
            //    Name = "Carlos Silva - Desenvolvedor",
            //    Email = "carlos.dev@kaanboard.com",
            //    PhoneNumber = "+55 11 99999-0002",
            //    CreatedAt = DateTimeOffset.UtcNow,
            //    UpdatedAt = DateTimeOffset.UtcNow,
            //    FlAtivo = true
            //};
            //memberUser.PasswordHash = passwordHasher.HashPassword(memberUser, "Admin@123*");

            //var viewerUser = new User<Guid>
            //{
            //    Id = Guid.NewGuid(),
            //    UserName = "ana_qa",
            //    Name = "Ana Costa - QA & Viewer",
            //    Email = "ana.qa@kaanboard.com",
            //    PhoneNumber = "+55 11 99999-0003",
            //    CreatedAt = DateTimeOffset.UtcNow,
            //    UpdatedAt = DateTimeOffset.UtcNow,
            //    FlAtivo = true
            //};
            //viewerUser.PasswordHash = passwordHasher.HashPassword(viewerUser, "Qa@123*");

            //await context.Users.AddRangeAsync(adminUser, memberUser, viewerUser);
            //await context.SaveChangesAsync();

            //// ==========================================
            //// 2. POPULAR QUADRO (Tabela: Boards)
            //// ==========================================
            //var boardId = Guid.NewGuid();
            //var board = new Board
            //{
            //    IdBoard = boardId,
            //    Name = "Projeto Alpha - Gestão de Tarefas (KaanBoard)",
            //    Background_Color = "#1E293B"
            //};
            //await context.Boards.AddAsync(board);
            //await context.SaveChangesAsync();

            //// ==========================================
            //// 3. VINCULAR USUÁRIOS AO QUADRO (Tabela: UserBoard)
            //// ==========================================
            //// Relacionamento N:N com definição do papel (BoardRole) de cada membro no quadro
            //var userBoards = new List<UserBoard>
            //{
            //    new()
            //    {
            //        IdUser = adminUser.Id,
            //        IdBoard = boardId,
            //        UserRole = BoardRole.Owner
            //    },
            //    new()
            //    {
            //        IdUser = memberUser.Id,
            //        IdBoard = boardId,
            //        UserRole = BoardRole.Member
            //    },
            //    new()
            //    {
            //        IdUser = viewerUser.Id,
            //        IdBoard = boardId,
            //        UserRole = BoardRole.Viewer
            //    }
            //};
            //await context.UserBoard.AddRangeAsync(userBoards);

            //// ==========================================
            //// 4. POPULAR COLUNAS (Tabela: Columns)
            //// ==========================================
            //var colTodoId = Guid.NewGuid();
            //var colInProgressId = Guid.NewGuid();
            //var colReviewId = Guid.NewGuid();
            //var colDoneId = Guid.NewGuid();

            //var columns = new List<Column>
            //{
            //    new() { IdColumn = colTodoId, IdBoard = boardId, TxTitle = "A Fazer", Nrposition = 1 },
            //    new() { IdColumn = colInProgressId, IdBoard = boardId, TxTitle = "Em Andamento", Nrposition = 2 },
            //    new() { IdColumn = colReviewId, IdBoard = boardId, TxTitle = "Em Revisão", Nrposition = 3 },
            //    new() { IdColumn = colDoneId, IdBoard = boardId, TxTitle = "Concluído", Nrposition = 4 }
            //};
            //await context.Columns.AddRangeAsync(columns);

            //// ==========================================
            //// 5. POPULAR TAREFAS (Tabela: TaskItem)
            //// ==========================================
            //var task1Id = Guid.NewGuid();
            //var task2Id = Guid.NewGuid();
            //var task3Id = Guid.NewGuid();

            //var tasks = new List<TaskItem>
            //{
            //    new()
            //    {
            //        IdTaskItem = task1Id,
            //        IdColumn = colDoneId,
            //        TxTitle = "Estruturar Banco de Dados e Entidades",
            //        TxDescription = "Finalizar mapeamento das entidades no Entity Framework Core, configurar chaves primárias e estrangeiras e rodar as migrations.",
            //        NrPosition = 1,
            //        FlCompleted = true,
            //        DueDate = DateTimeOffset.UtcNow.AddDays(-1),
            //        UpdatedAt = DateTimeOffset.UtcNow
            //    },
            //    new()
            //    {
            //        IdTaskItem = task2Id,
            //        IdColumn = colInProgressId,
            //        TxTitle = "Implementar Autenticação com JWT",
            //        TxDescription = "Criar endpoints de login e cadastro na API, validando credenciais no banco e retornando o token Bearer.",
            //        NrPosition = 1,
            //        FlCompleted = false,
            //        DueDate = DateTimeOffset.UtcNow.AddDays(3),
            //        UpdatedAt = DateTimeOffset.UtcNow
            //    },
            //    new()
            //    {
            //        IdTaskItem = task3Id,
            //        IdColumn = colTodoId,
            //        TxTitle = "Desenvolver Componentes Visuais do Quadro (Kanban)",
            //        TxDescription = "Implementar interface de drag-and-drop para movimentação das tarefas entre colunas.",
            //        NrPosition = 1,
            //        FlCompleted = false,
            //        DueDate = DateTimeOffset.UtcNow.AddDays(7),
            //        UpdatedAt = DateTimeOffset.UtcNow
            //    }
            //};
            //await context.TaskItem.AddRangeAsync(tasks);
            //await context.SaveChangesAsync();

            //// ==========================================
            //// 6. POPULAR HISTÓRICO DE TAREFAS (Tabela: TaskItemUserHistory)
            //// ==========================================
            //// A chave primária desta tabela é composta por (Iduser, IdTaskItem).
            //var taskHistories = new List<TaskItemUserHistory>
            //{
            //    new()
            //    {
            //        Iduser = adminUser.Id,
            //        IdTaskItem = task1Id,
            //        ActionDate = DateTimeOffset.UtcNow.AddHours(-10),
            //        TxAction = "Criou e moveu a tarefa para a coluna Concluído."
            //    },
            //    new()
            //    {
            //        Iduser = memberUser.Id,
            //        IdTaskItem = task2Id,
            //        ActionDate = DateTimeOffset.UtcNow.AddHours(-2),
            //        TxAction = "Assumiu a tarefa e moveu para Em Andamento."
            //    }
            //};
            //await context.TaskItemUserHistory.AddRangeAsync(taskHistories);

            //// ==========================================
            //// 7. POPULAR COMENTÁRIOS (Tabela: Comments)
            //// ==========================================
            //var comment1Id = Guid.NewGuid();
            //var comment2Id = Guid.NewGuid();

            //var comments = new List<Comment>
            //{
            //    new()
            //    {
            //        IdComment = comment1Id,
            //        IdUser = adminUser.Id,
            //        IdTaskItem = task2Id,
            //        TxComment = "A estrutura do serviço TokenService já está configurada com a chave do appsettings.",
            //        TxEmojis = "👍,🚀",
            //        DtCreation = DateTimeOffset.UtcNow.AddHours(-1),
            //        IsDeleted = false
            //    },
            //    new()
            //    {
            //        IdComment = comment2Id,
            //        IdUser = memberUser.Id,
            //        IdTaskItem = task2Id,
            //        TxComment = "Entendido! Já estou finalizando o controller de autenticação.",
            //        TxEmojis = "💻,🔥",
            //        DtCreation = DateTimeOffset.UtcNow.AddMinutes(-30),
            //        IsDeleted = false
            //    }
            //};
            //await context.Comments.AddRangeAsync(comments);
            //await context.SaveChangesAsync();

            //// ==========================================
            //// 8. POPULAR HISTÓRICO DE COMENTÁRIOS (Tabela: CommentHistory)
            //// ==========================================
            //var commentHistories = new List<CommentHistory>
            //{
            //    new()
            //    {
            //        IdCommentHistory = Guid.NewGuid(),
            //        IdComment = comment1Id,
            //        IdUser = adminUser.Id,
            //        DtAction = DateTimeOffset.UtcNow.AddHours(-1),
            //        TxAction = "Comentário criado no card da tarefa."
            //    },
            //    new()
            //    {
            //        IdCommentHistory = Guid.NewGuid(),
            //        IdComment = comment2Id,
            //        IdUser = memberUser.Id,
            //        DtAction = DateTimeOffset.UtcNow.AddMinutes(-30),
            //        TxAction = "Resposta adicionada com sucesso."
            //    }
            //};
            //await context.CommentHistory.AddRangeAsync(commentHistories);
            //await context.SaveChangesAsync();
        }
    }
}

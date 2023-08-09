CREATE PROC SP_AltTodoItem
    @Name			VARCHAR(200),
    @IsCompleted	BIT,
	@Id				INT
AS
BEGIN
	UPDATE TodoItem 
	SET Name = @Name, 
		IsCompleted = @IsCompleted
	WHERE Id = @Id

	SELECT 'Tarefa de Id: ' + CAST(@Id AS VARCHAR) + ', foi atualizado com sucesso!'
END
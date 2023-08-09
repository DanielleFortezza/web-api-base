CREATE PROC SP_DelTodoItem
	@Id           INT
AS
BEGIN
	DELETE FROM TodoItem 	
	WHERE Id = @Id

	SELECT 'Tarefa de Id: ' + CAST(@Id AS VARCHAR) + ', foi deletada com sucesso.'
END
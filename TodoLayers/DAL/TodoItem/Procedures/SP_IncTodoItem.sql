CREATE PROC SP_IncTodoItem
    @Name			VARCHAR(200),
    @IsCompleted    BIT
AS
BEGIN
	INSERT INTO TodoItem (Name, IsCompleted) 
	VALUES (@Name, @IsCompleted)

	SELECT SCOPE_IDENTITY()
END
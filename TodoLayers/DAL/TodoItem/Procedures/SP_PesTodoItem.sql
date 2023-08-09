CREATE PROC SP_PesTodoItem
AS
BEGIN
	SELECT Id, Name, IsCompleted 
	FROM TodoItem
END
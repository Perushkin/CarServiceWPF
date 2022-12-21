CREATE PROCEDURE [dbo].[sp_InsertOrder]
	@orderId int,
	@clientId	int,
	@carModel   nvarchar(50),
    @workType   int,
    @price    MONEY,
    @employeeName  int,
    @completed TINYINT

AS
	INSERT INTO orderT(orderId, clientId, carModel, workType, price, employeeName, completed)
	VALUES (@orderId, @clientId, @carModel, @workType, @price, @employeeName, @completed)

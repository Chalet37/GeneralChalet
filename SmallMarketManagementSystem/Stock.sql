-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE NewStock(@CommodityName NCHAR(50), 
						@UnitPrice MONEY, 
						@Amount INTEGER, 
						@SupplierID INTEGER, 
						@StockDate DATETIME,
						@CommodityID INTEGER OUTPUT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF

	SELECT @CommodityID = ISNULL(MAX(CommodityID),0) + 1
	FROM StorageList

	INSERT INTO StorageList(CommodityID, CommodityName, UnitPrice, StorageAmount, SupplierID)
	VALUES( @CommodityID, @CommodityName, @UnitPrice, @Amount, @SupplierID )

	INSERT INTO StockRecord(CommodityID, StockAmount, StorageAmount, StockUnitPrice, StockDate, SupplierID)
	VALUES( @CommodityID, @Amount, @Amount, @UnitPrice, @StockDate, @SupplierID )
END
GO

CREATE PROCEDURE StockUpdate(@CommodityID INTEGER,@Amount INTEGER, @StockDate DATETIME)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF

	UPDATE StorageList
		SET StorageAmount = StorageAmount + @Amount
		WHERE CommodityID = @CommodityID

	INSERT INTO StockRecord(CommodityID, StockAmount, StorageAmount, StockDate)
	VALUES( @CommodityID, @Amount, @Amount, @StockDate)
END
GO



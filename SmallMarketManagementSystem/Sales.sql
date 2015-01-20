USE [SmallMarketManagementSystem]
GO
/****** Object:  StoredProcedure [dbo].[Sell]    Script Date: 2014/10/16 17:31:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Sell](@CommodityID INTEGER, 
					@Amount INTEGER, 
					@SalesDate DATETIME,
					@EmployeeID INTEGER) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    UPDATE StorageList
	SET StorageAmount = StorageAmount - @Amount
	WHERE CommodityID = @CommodityID

INSERT INTO SalesRecord(CommodityID, SalesAmount, SalesDate, EmployeeID)
VALUES(@CommodityID, @Amount, @SalesDate, @EmployeeID)
END

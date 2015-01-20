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
CREATE PROCEDURE NewSupplier(@SupplierName NCHAR(50), 
							@SupplierAddress NCHAR(50), 
							@SupplierTel INTEGER, 
							@SupplierID INTEGER OUTPUT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

	SELECT @SupplierID = ISNULL( MAX( SupplierID ), 0 ) + 1
	FROM SuppliersInfo

	INSERT INTO SuppliersInfo(SupplierID, SupplierName, SupplierAddress, SupplierTel)
	VALUES(@SupplierID, @SupplierName, @SupplierAddress, @SupplierTel) 
END
GO

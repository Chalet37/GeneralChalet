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
CREATE PROCEDURE NewEmployee(@EmployeeName NCHAR(50), 
							@EmployeeAddress NCHAR(50), 
							@EmployeeTel INTEGER, 
							@EmployeeID INTEGER OUTPUT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF

	SELECT @EmployeeID = ISNULL( MAX(EmployeeID), 0 ) + 1
	FROM EmployeeInfo

	INSERT INTO EmployeeInfo(EmployeeID, EmployeeName, EmployeeAddress, EmployeeTel)
	VALUES(@EmployeeID,@EmployeeName, @EmployeeAddress, @EmployeeTel)
END
GO
